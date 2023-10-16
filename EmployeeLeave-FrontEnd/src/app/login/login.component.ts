import { Component } from '@angular/core';
import { BaseService } from '../services/baseservice'
import { User } from '../models/user'
import { LogInResult } from '../models/loginresult';
import { LogInDTO } from '../models/logindto';
import { LogInService } from '../services/loginservice';
import jwtDecode, * as jwt_decode from 'jwt-decode';
import { Router } from '@angular/router';
import { DecodedToken } from '../models/decodedtoken';


@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent {

  logInResult: LogInResult = {
    isSuccess: false,
    message: '',
    token: ''
  }

  logInDTO: LogInDTO = {
    email: '',
    password: ''
  }

  constructor(private logInService: LogInService, private router: Router) { }

  users: User[] = [];

  Login(): void {
    let decodedToken: DecodedToken

    this.logInService.LogIn(this.logInDTO).subscribe(response => {
      this.logInResult = response

      if (response.isSuccess) {
        localStorage.setItem("sut22UserToken", response.token);
        var token = localStorage.getItem("sut22UserToken")

        if (token != null) {
          decodedToken = jwt_decode.default(token)
          console.log(decodedToken)
        }

        try {
          if (decodedToken.IsAdmin) {
            console.log(decodedToken.IsAdmin)
            this.router.navigate(['admin']);
          }
          else {
            console.log(decodedToken.IsAdmin)
            this.router.navigate(['user']);
          }
        }
        catch (e) {

        }
      }
      console.log(this.logInResult)
    })
  }
}
