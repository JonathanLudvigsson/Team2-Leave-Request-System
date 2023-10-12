import { Component } from '@angular/core';
import { BaseService } from '../services/baseservice'
import { User } from '../models/user'

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent {

  constructor(private baseService: BaseService) {

  }

  users: User[] = []

  ngOnInit() {
    this.GetAllUsers()
  }

  GetAllUsers(): void {
    this.baseService.GetAll<User>("users").subscribe(response => {
      this.users = response
      console.log(response)
    })
  }

}
