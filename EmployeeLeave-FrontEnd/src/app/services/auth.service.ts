import {Injectable} from '@angular/core';
import {BehaviorSubject} from 'rxjs';
import * as jwt_decode from 'jwt-decode';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  public _isLoggedIn = new BehaviorSubject<boolean>(this.hasToken());
  public _isAdmin = new BehaviorSubject<boolean>(this.isAdmin());
  isLoggedIn$ = this._isLoggedIn.asObservable();
  isAdmin$ = this._isAdmin.asObservable();

  constructor() {
  }

  private hasToken(): boolean {
    return !!localStorage.getItem('sut22UserToken');
  }

  private isAdmin(): boolean {
    let token = localStorage.getItem('sut22UserToken');
    if (token != null) {
      const decodedToken: any = jwt_decode.default(token);
      return decodedToken.IsAdmin === 'true';
    }
    return false;
  }

  logout() {
    localStorage.removeItem('sut22UserToken');
    this._isLoggedIn.next(false);
  }
}
