import {Component} from '@angular/core';
import {Router} from '@angular/router';
import {DecodedToken} from '../models/decodedtoken';
import {BaseService} from 'src/app/services/baseservice';
import {Request} from 'src/app/models/request';
import {AuthService} from "../services/auth.service";
import * as jwt_decode from 'jwt-decode';


@Component({
  selector: 'app-user',
  templateUrl: './user.component.html',
  styleUrls: ['./user.component.css']
})

export class UserComponent {

  requestStatus: string[] = ['Pending', 'Approved', 'Declined', ''];

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

  showForm = false;

  baseUrl: string = 'https://localhost:7268/api/'

  constructor(private router: Router, private baseService: BaseService, private authService: AuthService) {

  }

  ngOnInit() {
    this.decodeToken();
    this.authenticateUser();
    this.getRequestById(this.myToken.UserId)
  }

  decodeToken() {
    var token = localStorage.getItem("sut22UserToken")
    if (token != null) {
      const decodedToken: DecodedToken = jwt_decode.default(token)
      this.myToken = decodedToken
    } else {
      this.router.navigate(['/']);
    }
  }

  authenticateUser() {
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

  getRequestCount(status: string): number {
    return this.requestStatus.filter(request => request === status).length;
  }
}
