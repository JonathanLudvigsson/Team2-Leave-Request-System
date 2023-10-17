import {Component} from '@angular/core';
import {Router} from '@angular/router';
import {DecodedToken} from '../models/decodedtoken';
import {BaseService} from 'src/app/services/baseservice';
import {Request} from 'src/app/models/request';
import {AuthService} from "../services/auth.service";


@Component({
  selector: 'app-user',
  templateUrl: './user.component.html',
  styleUrls: ['./user.component.css']
})
export class UserComponent {

  myToken: DecodedToken = {
    Email: '',
    FirstName: '',
    IsAdmin: "false",
    UserId: '-1',
    aud: '',
    exp: '',
    iss: ''
  }

  newRequest: Request = {
    requestID: '',
    leaveStatus: '0',
    userID: '',
    leaveTypeID: '',
    startDate: '',
    endDate: ''
  };

  request: Request = {
    requestID: "",
    userID: "",
    leaveTypeID: "",
    leaveStatus: "",
    startDate: "",
    endDate: ""
  }

  requests?: any[];

  public statusLabels: string[] = ['Pending', 'Approved', 'Declined'];

  baseUrl: string = 'https://localhost:7268/api/'

  constructor(private router: Router, private baseService: BaseService, private authService: AuthService) {

  }

  ngOnInit() {
    this.AuthenticateUser();
  }

  AuthenticateUser() {
    this.authService.isLoggedIn$.subscribe(response => {
      if (!response) {
        this.router.navigate(['/']);
      }
    });
    this.authService.isAdmin$.subscribe(response => {
        if (response) {
          this.router.navigate(['/']);
        }
      }
    );
  }

  getRequestById(id: any) {
    if (id) {
      this.baseService.GetArray("request/user/", id).subscribe((response) => {
        this.requests = response;
        console.log(response)
      })
    }
  }

  onSubmit() {
    this.createRequest(this.newRequest);
  }

  createRequest(request: Request) {
    this.baseService.Create<Request>("request/post/", request).subscribe((response) => {
      this.request = response;
    });
  }
}

