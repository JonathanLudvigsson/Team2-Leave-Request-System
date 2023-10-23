import {Component} from '@angular/core';
import {BaseService} from '../services/baseservice';
import {Request} from '../models/request'
import {LeaveType} from '../models/leavetype';
import {User} from '../models/user';
import {AuthService} from "../services/auth.service";
import {Router} from '@angular/router';
import {RequestDTO} from '../models/requestdto';
import {UserLeaveBalance} from '../models/userleavebalance';
import { ApprovedLeave } from '../models/approvedleave';
import { LeaveTypeDTO } from '../models/leavetypedto';

@Component({
  selector: 'app-admin',
  templateUrl: './admin.component.html',
  styleUrls: ['./admin.component.css']
})
export class AdminComponent {

  constructor(private baseService: BaseService, private authService: AuthService, private router: Router) {

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

  ngOnInit() {
    this.GetAllRequests()
    this.GetAllUsers()
    this.GetTotalLeaveTypeDays()
    this.AuthenticateAdmin()
  }

  AuthenticateAdmin() {
    this.authService.isLoggedIn$.subscribe(response => {
      if (!response) {
        this.router.navigate(['/']);
      }
    });
    this.authService.isAdmin$.subscribe(response => {
      if (!response) {
        this.router.navigate(['/']);
      }
    });
  }

  GetAllUsers() {
    this.baseService.GetAll<User>("users").subscribe(response => {
      this.users = response.filter(u => !u.isAdmin)
    })
  }

  GetAllRequests() {
    this.baseService.GetAll<Request>("request").subscribe(response => {
      this.currentSort = ''
      this.requests = response
      this.requestsToShow = this.requests as RequestDTO[]
      this.requestsToShow.forEach(request => {
        this.baseService.Get<User>("users/", request.userID).subscribe(user => {
          request.userName = user.name
        })
        this.baseService.Get<LeaveType>("leavetypes/", request.leaveTypeID).subscribe(leavetype => {
          request.leaveTypeName = leavetype.name
        })
        this.baseService.Get<UserLeaveBalance[]>("user-leave-balances/user/", request.userID).subscribe(balance => {
          const foundBalance = balance.find(b => b.leaveTypeId == request.leaveTypeID);
          if (foundBalance) {
            request.daysLeft = foundBalance.daysLeft;
          } else {
            console.error("No balance found for user " + request.userID + " and leave type " + request.leaveTypeID);
          }
        })
      })

    })
  }

  SortRequests(status: string) {
    this.baseService.GetAll<Request>("request").subscribe(response => {
      this.requests = response.filter(r => r.leaveStatus == status)
      this.requestsToShow = this.requests as RequestDTO[]
      this.requestsToShow.forEach(request => {
        this.baseService.Get<User>("users/", request.userID).subscribe(user => {
          request.userName = user.name
        })
        this.baseService.Get<LeaveType>("leavetypes/", request.leaveTypeID).subscribe(leavetype => {
          request.leaveTypeName = leavetype.name
        })
        this.baseService.Get<UserLeaveBalance[]>("user-leave-balances/user/", request.userID).subscribe(balance => {
          const foundBalance = balance.find(b => b.leaveTypeId == request.leaveTypeID);
          if (foundBalance) {
            request.daysLeft = foundBalance.daysLeft;
          } else {
            console.error("No balance found for user " + request.userID + " and leave type " + request.leaveTypeID);
          }
        })

      })

    })
  }

  ApproveOrDenyRequest(request: Request, approved: boolean) {
    if (approved == true) {
      request.leaveStatus = '1'
    } else {
      request.leaveStatus = '2'
    }
    this.baseService.Update<Request>("request/update/", request.requestID, request).subscribe(response => {
      console.log(response)
    })
  }

  DeleteRequest(id: string) {
    this.baseService.Delete<Request>("request/delete/", id).subscribe(response => {
      if (this.baseService.Get<ApprovedLeave>("approved-leaves/request/", response.requestID)) {
        this.baseService.Delete<ApprovedLeave>("approved-leaves/request/", id).subscribe(response => {

        })
      }

    })
  }

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
    })
  }

  EditLeaveType(id: string) {
    this.baseService.Update<LeaveType>("leavetypes/", id, this.newLeave).subscribe(response => {
    })
  }

  DeleteLeaveType(id: string) {
    this.baseService.Delete<LeaveType>("leavetypes/", id).subscribe(response => {
    })
  }

}
