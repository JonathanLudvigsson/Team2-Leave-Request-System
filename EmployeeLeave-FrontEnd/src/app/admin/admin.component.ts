import { Component } from '@angular/core';
import { BaseService } from '../services/baseservice';
import { Request } from '../models/request'
import { LeaveType } from '../models/leavetype';

@Component({
  selector: 'app-admin',
  templateUrl: './admin.component.html',
  styleUrls: ['./admin.component.css']
})
export class AdminComponent {

  constructor(private baseService: BaseService) {

  }

  requests: Request[] = []

  newLeave: LeaveType = {
    leaveTypeID: '-1',
    name: '',
    maximumDays: ''
  }

  ngOnInit() {
    this.GetAllRequests()
  }

  GetAllRequests() {
    this.baseService.GetAll<Request>("request").subscribe(response => {
      this.requests = response
    })
  }

  ApproveOrDenyRequest(request: Request, approved: boolean) {
    if (approved == true) {
      request.leaveStatus = '1'
    }
    else {
      request.leaveStatus = '2'
    }
    this.baseService.Update<Request>("request/update/", request.requestID, request).subscribe(response => {
      console.log(response)
    })
  }

  CreateLeaveType() {
    this.baseService.Create<LeaveType>("leavetypes", this.newLeave).subscribe(response => {

    })
  }

}
