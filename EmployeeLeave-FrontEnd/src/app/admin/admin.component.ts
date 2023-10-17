import { Component } from '@angular/core';
import { BaseService } from '../services/baseservice';
import { Request } from '../models/request'

@Component({
  selector: 'app-admin',
  templateUrl: './admin.component.html',
  styleUrls: ['./admin.component.css']
})
export class AdminComponent {

  constructor(private baseService: BaseService) {

  }

  requests: Request[] = []

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

}
