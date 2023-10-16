import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";

@Injectable({
  providedIn: 'root'
})

export class BaseService {
  baseUrl: string = 'https://localhost:7268/api/'

  constructor(private http: HttpClient) {

  }

  GetAll<T>(urlType: string): Observable<T[]> {
    return this.http.get<T[]>(this.baseUrl + urlType)
  }

  Get<T>(urlType: string, id: string): Observable<T> {
    return this.http.get<T>(this.baseUrl + urlType + id)
  }
  GetArray<T>(urlType: string, id: string): Observable<T[]> {
    return this.http.get<T[]>(this.baseUrl + urlType + id)
  }
  Update<T>(urlType: string, id: string, updatedEntity: T) {
    return this.http.put<T>(this.baseUrl + urlType + id, updatedEntity)
  }

  Create<T>(urlType: string, newEntity: T) {
    return this.http.post<T>(this.baseUrl + urlType, newEntity)
  }

  Delete<T>(urlType: string, id: string) {
    return this.http.delete<T>(this.baseUrl + urlType + id)
  }

}
