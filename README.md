
# Group School project: Leave Request System  

## Access

- Access the Website here https://proud-hill-0fce4a510.4.azurestaticapps.net/ to try it out.
  
Login credentials to test functionallity:
- Email: `employee-test@employee-leave-test.com` Password: `test`  
- Email: `admin-test@employee-leave-test.com` Password: `test`  


## Description

The Leave Request System is a web-based application designed to facilitate the management of leave requests in a workplace. This system allows employees to log in to their accounts, submit leave requests, and provides administrators with the capability to approve or deny these requests. Upon approval, the employee's leave days will be appropriately deducted.

## Features

- **User Authentication:** Employees and administrators can log in to their respective accounts.
- **Leave Request Submission:** Employees can submit leave requests with date ranges and reasons.
- **Leave Request Approval:** Administrators can approve or deny leave requests.
- **Leave Deduction:** Approved requests will deduct the appropriate leave days from the employee's balance.
- **Email Notification:** Employees will recive an email when a request is handled by an admin.

## Technologies

The Leave Request System is built using the following technologies:

- **Backend API:** C# with a Minimal API approach, utilizing Dependency Injection and the Repository Pattern.
- **Frontend:** Angular for accessing the API endpoints.
- **Styling:** Basic HTML/CSS with Bootstrap as the primary library for styling.
- **Testing:** XUnit and NSubstitute for unit testing
- **Version control:** Git and Github
- **CI/CD:** Github Actions
- **Deployment:** Azure
- **Other:** Nuget package Hangfire is used to handle the queing and sending of emails. Entity Framework Core and MS SQL Server for data storage.

## CI/CD

We have used Github Actions for Continuous Integration, before new branches are merged to the master branch automated builds and tests are run to check that the new code does not add errors or break unit tests. If the builds or tests fail the contributing developers will be notified. Github Actions also supports Continuous Deployment along with Azure.

## Kanban Board

For handling our workflow and tracking progress, we used a Kanban board. You can view and explore our board on Trello:

- [Team 2's Kanban Board](https://trello.com/b/DRPcEHku/team-2)

## Contributors

- [Jonathan Ludvigsson](https://github.com/JonathanLudvigsson)
- [Zchmenke](https://github.com/Zchmenke)
- [Qvarnstr0m](https://github.com/qvarnstr0m)



