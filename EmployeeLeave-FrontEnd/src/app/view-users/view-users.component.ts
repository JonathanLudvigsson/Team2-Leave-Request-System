import { Component } from '@angular/core';
import { BaseService } from '../services/baseservice';
import { Request } from '../models/request'
import { LeaveType } from '../models/leavetype';
import { User } from '../models/user';
import { AuthService } from "../services/auth.service";
import { Router } from '@angular/router';
import { RequestDTO } from '../models/requestdto';
import { UserLeaveBalance } from '../models/userleavebalance';
import { ApprovedLeave } from '../models/approvedleave';
import { LeaveTypeDTO } from '../models/leavetypedto';
import { LeaveTypeService } from '../services/leavetypeservice';


@Component({
  selector: 'app-view-users',
  templateUrl: './view-users.component.html',
  styleUrls: ['./view-users.component.css']
})
export class ViewUsersComponent {
  constructor(private baseService: BaseService, private authService: AuthService, private leaveService: LeaveTypeService, private router: Router) {

  }

  users: User[] = []

  ngOnInit() {
    this.GetAllUsers();
  }


  GetAllUsers() {
    this.baseService.GetAll<User>("users").subscribe(response => {
      this.users = response.filter(u => !u.isAdmin)
    })
  }


}
