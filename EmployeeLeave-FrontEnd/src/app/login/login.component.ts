import { Component } from '@angular/core';
import { BaseService } from '../services/baseservice'
import { User } from '../models/user'

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent {
  email = "";
  password = "";

  constructor(private baseService: BaseService) { }

  users: User[] = [];

  Login() {
    console.log(this.email, this.password);
  }
}
