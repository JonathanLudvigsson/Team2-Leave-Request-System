import {Component} from '@angular/core';
import {User} from '../models/user'
import {LogInResult} from '../models/loginresult';
import {LogInDTO} from '../models/logindto';
import {LogInService} from '../services/loginservice';
import * as jwt_decode from 'jwt-decode';
import {Router} from '@angular/router';
import {DecodedToken} from '../models/decodedtoken';
import {AuthService} from "../services/auth.service";


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

  constructor(private logInService: LogInService, private router: Router, private authService: AuthService) {
  }

  users: User[] = [];

  errorMessage: string = ''

  Login(): void {
    let decodedToken: DecodedToken

    this.logInService.LogIn(this.logInDTO).subscribe(response => {
      this.logInResult = response

      if (response.isSuccess) {
        localStorage.setItem("sut22UserToken", response.token);
        var token = localStorage.getItem("sut22UserToken")

        if (token != null) {
          decodedToken = jwt_decode.default(token)
        }

        this.authService._isLoggedIn.next(true);

        try {
          if (decodedToken.IsAdmin === 'true') {
            this.authService._isAdmin.next(true);
            this.router.navigate(['admin']);
          } else {
            this.router.navigate(['user']);
          }
        } catch (e) {

        }
      }
      
    }
      , error => {
        this.errorMessage = "Incorrect email or password. Try again."
      })

  }
}
