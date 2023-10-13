import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { LogInDTO } from "../models/logindto";
import { LogInResult } from "../models/loginresult";

@Injectable({
  providedIn: 'root'
})

export class LogInService {
  baseUrl: string = 'https://localhost:7268/api/auth/login'

  constructor(private http: HttpClient) {

  }

  LogIn(logInDTO: LogInDTO): Observable<LogInResult> {
    return this.http.post<LogInResult>(this.baseUrl, logInDTO)
  }
}
