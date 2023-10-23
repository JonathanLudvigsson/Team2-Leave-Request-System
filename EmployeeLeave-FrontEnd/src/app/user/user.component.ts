import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { DecodedToken } from '../models/decodedtoken';
import { BaseService } from 'src/app/services/baseservice';
import { Request } from 'src/app/models/request';
import { AuthService } from "../services/auth.service";
import * as jwt_decode from 'jwt-decode';
import { LeaveType } from "../models/leavetype";
import { ApprovedLeave } from '../models/approvedleave';
import { UserLeaveBalance } from "../models/userleavebalance";
import { HttpClient } from '@angular/common/http';



@Component({
  selector: 'app-user',
  templateUrl: './user.component.html',
  styleUrls: ['./user.component.css']
})

export class UserComponent {

  requestStatus: string[] = ['Pending', 'Approved', 'Declined', ''];

  myToken: DecodedToken = {
    Email: '',
    FirstName: '',
    IsAdmin: "false",
    UserId: '-1',
    aud: '',
    exp: '',
    iss: ''
  }

  newRequest: Request = {
    requestID: '',
    leaveStatus: '0',
    userID: this.myToken.UserId,
    leaveTypeID: '',
    startDate: '',
    endDate: ''
  };

  request: Request = {
    requestID: "",
    userID: "",
    leaveTypeID: "",
    leaveStatus: "",
    startDate: "",
    endDate: ""
  }
  requests?: any[];
  public statusLabels: string[] = ['Pending', 'Approved', 'Declined'];
  showForm: boolean = false;
  baseUrl: string = 'https://localhost:7268/api/'
  public approvedRequests: number = 0;
  public pendingRequests: number = 0;
  public declinedRequests: number = 0;
  public userName: string = '';
  public userId: string = '';
  public leaveTypes: LeaveType[] = [];
  userLeaveBalanceData: any;





  constructor(private router: Router, private baseService: BaseService, private authService: AuthService, private http: HttpClient) {

  }

  ngOnInit() {
    this.decodeToken();
    this.authenticateUser();
    this.getRequestById(this.myToken.UserId);
    this.updateRequestNumbers();
    this.setLoggedInUser();
    this.fetchLeaveTypes();
    this.getUserLeaveBalance();
  }

  fetchLeaveTypes() {
    this.baseService.GetAll<LeaveType>("leavetypes").subscribe((response) => {
      this.leaveTypes = response;
    })
  }

  setLoggedInUser() {
    this.userName = this.myToken.FirstName;
    this.userId = this.myToken.UserId;
  }

  decodeToken() {
    var token = localStorage.getItem("sut22UserToken")
    if (token != null) {
      const decodedToken: DecodedToken = jwt_decode.default(token)
      this.myToken = decodedToken
    } else {
      this.router.navigate(['/']);
    }
  }

  authenticateUser() {
    this.authService.isLoggedIn$.subscribe(response => {
      if (!response) {
        this.router.navigate(['/']);
      }
    });
    this.authService.isAdmin$.subscribe(response => {
      if (response) {
        this.router.navigate(['/']);
      }
    }
    );
  }

  getLeaveTypeName(leaveTypeID: string): string {
    const leaveType = this.leaveTypes.find(type => type.leaveTypeID === leaveTypeID);
    return leaveType ? leaveType.name : 'N/A';  // return 'N/A' if the type is not found
  }


  getRequestById(id: any) {
    if (id) {
      this.baseService.GetArray("request/user/", id).subscribe((response) => {
        this.requests = response;
        this.updateRequestNumbers();
      })
    }
  }

  onSubmit() {
    this.createRequest(this.newRequest);
  }

  createRequest(request: Request) {
    request.userID = this.myToken.UserId;
    request.leaveStatus = 'Pending';
    this.baseService.Create<Request>("request/post/", request).subscribe((response) => {
      this.request = response;
      this.ngOnInit();
    });
  }

  getRequestCount(status: string): number {
    if (!this.requests) return 0;
    return this.requests.filter(request => request.leaveStatus === status).length;
  }

  updateRequestNumbers() {
    this.pendingRequests = this.getRequestCount('Pending');
    this.approvedRequests = this.getRequestCount('Approved');
    this.declinedRequests = this.getRequestCount('Declined');
  }

  deleteRequest(id: any) {
    if (id) {
      this.baseService.Delete<Request>('request/delete/', id).subscribe(
        (response) => {

          this.baseService.Delete<ApprovedLeave>("approved-leaves/request/", id).subscribe(response => {

          })
          this.ngOnInit();
        },
        (error) => {
          console.error('Error:', error);
        }
      );
    }
  }

  toggleForm() {
    this.showForm = !this.showForm;
  }

  getUserLeaveBalance() {
    const id = this.myToken.UserId;
    const url = `https://localhost:7268/api/user-leave-balances/user/${id}`;
    this.http.get(url).subscribe((response) => {
      this.userLeaveBalanceData = response;
      console.log(this.userLeaveBalanceData)
    }, (error) => {
      console.error('Error:', error);
    });
  }


}
