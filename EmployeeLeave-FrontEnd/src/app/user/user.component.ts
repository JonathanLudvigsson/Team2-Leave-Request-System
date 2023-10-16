import {Component} from '@angular/core';
import {Router} from '@angular/router';
import jwtDecode, * as jwt_decode from 'jwt-decode';
import {DecodedToken} from '../models/decodedtoken';
import {BaseService} from 'src/app/services/baseservice';
import {Request} from 'src/app/models/request';


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
  request: Request = {
    requestID: "",
    userID: "",
    leaveTypeID: "",
    leaveStatus: "",
    startDate: "",
    endDate: ""
  }
  requests?: any[];
  baseUrl: string = 'https://localhost:7268/api/'

  constructor(private router: Router, private baseService: BaseService) {

  }

  ngOnInit() {
    this.decodeToken()
    console.log(this.myToken)
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

  getRequestById(id: any) {
    if (id) {
      this.baseService.GetArray("request", id).subscribe((response) => {
        this.requests = response;
      })
    }
  }
}

