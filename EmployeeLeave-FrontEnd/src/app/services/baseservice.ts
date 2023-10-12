import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";

@Injectable({
  providedIn: 'root'
})

export class BaseService {
  url: string = 'https://localhost:7268/api/'

  constructor(private http: HttpClient) {

  }

  GetAll<T>(urlType: string): Observable<T[]> {
    return this.http.get<T[]>(this.url + urlType)
  }

  Get<T>(urlType: string, id: string): Observable<T> {
    return this.http.get<T>(this.url + id)
  }

}
