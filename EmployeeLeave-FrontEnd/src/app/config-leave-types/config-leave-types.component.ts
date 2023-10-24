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
  selector: 'app-config-leave-types',
  templateUrl: './config-leave-types.component.html',
  styleUrls: ['./config-leave-types.component.css']
})
export class ConfigLeaveTypesComponent {

  constructor(private baseService: BaseService, private authService: AuthService, private leaveService: LeaveTypeService, private router: Router) {

  }

  ngOnInit() {
    this.GetTotalLeaveTypeDays();
  }

  requests: Request[] = []

  requestsToShow: RequestDTO[] = []

  users: User[] = []

  newLeave: LeaveType = {
    leaveTypeID: '',
    name: '',
    maximumDays: ''
  }

  currentSort: string = ''

  leaveTypes: LeaveType[] = []

  leaveTypesTotalDays: LeaveTypeDTO[] = []

  startDate: string = ''
  endDate: string = ''

  GetAllLeaveTypes() {
    this.baseService.GetAll<LeaveType>("leavetypes").subscribe(response => {
      this.leaveTypes = response
    })
  }

  GetTotalLeaveTypeDays() {
    this.baseService.GetAll<LeaveTypeDTO>("leavetypes/daysused").subscribe(response => {
      this.leaveTypesTotalDays = response
    })
  }

  CreateLeaveType() {
    this.baseService.Create<LeaveType>("leavetypes", this.newLeave).subscribe(response => {
      this.GetTotalLeaveTypeDays()
    })
  }

  EditLeaveType(id: string) {
    this.baseService.Update<LeaveType>("leavetypes/", id, this.newLeave).subscribe(response => {
      this.GetTotalLeaveTypeDays()
    })
  }

  DeleteLeaveType(id: string) {
    this.baseService.Delete<LeaveType>("leavetypes/", id).subscribe(response => {
      this.GetTotalLeaveTypeDays()
    })
  }








}
