import { Component } from '@angular/core';
import { BaseService } from '../services/baseservice'
import { User } from '../models/user'
import { LogInResult } from '../models/loginresult';
import { LogInDTO } from '../models/logindto';
import { LogInService } from '../services/loginservice';
import jwtDecode, * as jwt_decode from 'jwt-decode';
import { Router } from '@angular/router';


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
    this.logInService.LogIn(this.logInDTO).subscribe(response => {
      this.logInResult = response
      if (response.isSuccess) {
        localStorage.setItem("sut22UserToken", response.token);

        try {

          this.router.navigate(['user']);
        } catch (e) {

        }
      }
      console.log(this.logInResult)
    })
  }
}
