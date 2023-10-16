import { Component } from '@angular/core';
import { Router } from '@angular/router';
import jwtDecode, * as jwt_decode from 'jwt-decode';
import { DecodedToken } from '../models/decodedtoken';

@Component({
  selector: 'app-user',
  templateUrl: './user.component.html',
  styleUrls: ['./user.component.css']
})
export class UserComponent {

  myToken: DecodedToken = {
    Email: '',
    FirstName: '',
    IsAdmin: false,
    UserId: '-1',
    aud: '',
    exp: '',
    iss: ''
  }

  constructor(private router: Router) {

  }

  ngOnInit() {
    this.decodeToken()
    console.log(this.myToken)
  }

  decodeToken() {
    var token = localStorage.getItem("sut22UserToken")
    if (token != null) {
      const decodedToken:DecodedToken = jwt_decode.default(token)
      this.myToken = decodedToken
    } else {
      this.router.navigate(['/']);
    }

  }
}
