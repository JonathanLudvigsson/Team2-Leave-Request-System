using System.Net;
using EmployeeLeaveAPI.Interfaces;
using EmployeeLeaveAPI.Models;
using Hangfire;
using System.Net.Mail;

namespace EmployeeLeaveAPI.Services;

public class EmailService : IEmailService
{
    private readonly IRepository<User> _userRepository;
    private readonly ILogger<EmailService> _logger;
    private readonly IRepository<Email> _emailRepository;
    private readonly IConfiguration _configuration;

    public EmailService(IRepository<User> userRepository, ILogger<EmailService> logger,
        IRepository<Email> emailRepository, IConfiguration configuration)
    {
        _userRepository = userRepository;
        _logger = logger;
        _emailRepository = emailRepository;
        _configuration = configuration;
    }

    public async Task<(bool isSuccess, string message, Email? email)> CreateEmail(int userId)
    {
        try
        {
            var user = _userRepository.Get(userId).Result;

            if (user == null) return (false, "User not found", null);

            var email = new Email
            {
                To = user.Email,
                Subject = "Your leave request has been approved",
                Body = "Your leave request has been approved"
            };

            return (true, "Email created", email);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error creating email");
            return (false, "Error creating email", null);
        }
    }

    public async Task<(bool isSuccess, string message)> SaveEmailToDbAsync(Email email)
    {
        try
        {
            await _emailRepository.Create(email);
            return (true, "Email saved to db");
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error saving email to db");
            return (false, "Error saving email to db");
        }
    }

    public async Task<(bool isSuccess, string message)> EnqueueEmail(Email email)
    {
        try
        {
            BackgroundJob.Enqueue(() => SendEmail(email));
            return (true, "Email enqueued");
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error enqueuing email");
            return (false, "Error enqueuing email");
        }
    }

    public async Task<(bool isSuccess, string message)> SendEmail(Email email)
    {
        try
        {
            email.Status = EmailStatus.Sent;
            email.SentDate = DateTime.Now;

            string emailFrom = _configuration["Email:From"] ??
                               throw new ArgumentNullException("Email:From configuration missing");

            string emailPassword = _configuration["Email:Password"] ??
                                   throw new ArgumentNullException("Email:Password configuration missing");
            string host = _configuration["Email:Host"] ??
                          throw new ArgumentNullException("Email:Host configuration missing");
            int port = int.Parse(_configuration["Email:Port"] ?? "587");

            using (MailMessage mail = new MailMessage(emailFrom, email.To, email.Subject, email.Body))
            using (SmtpClient smtp = new SmtpClient(host, port))
            {
                smtp.Credentials = new NetworkCredential(emailFrom, emailPassword);
                smtp.EnableSsl = true;
                await smtp.SendMailAsync(mail);
            }

            await _emailRepository.Update(email.Id, email);
            BackgroundJob.Delete(email.Id.ToString());
            return (true, "Email sent");
        }
        catch (Exception e)
        {
            email.Status = EmailStatus.Failed;
            await _emailRepository.Update(email.Id, email);
            _logger.LogError(e, "Error sending email");
            return (false, "Error sending email");
        }
    }
}