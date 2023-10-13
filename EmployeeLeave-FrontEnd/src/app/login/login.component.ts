import { Component } from '@angular/core';
import { BaseService } from '../services/baseservice'
import { User } from '../models/user'
import { LogInResult } from '../models/loginresult';
import { LogInDTO } from '../models/logindto';
import { LogInService } from '../services/loginservice';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent {

  logInResult: LogInResult = {
    isSuccess:false,
    message: '',
    token:''
  }

  logInDTO: LogInDTO = {
    email: '',
    password:''
  }

  constructor(private logInService: LogInService) { }

  users: User[] = [];

  Login(): void{
    this.logInService.LogIn(this.logInDTO).subscribe(response => {
      this.logInResult = response
      if (response.isSuccess) {
        localStorage.setItem("sut22UserToken", response.token)
      }
      console.log(this.logInResult)
    })
  }
}
