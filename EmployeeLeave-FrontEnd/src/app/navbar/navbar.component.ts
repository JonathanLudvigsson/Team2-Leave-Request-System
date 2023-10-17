import {Component} from '@angular/core';
import {Router} from '@angular/router';
import {BaseService} from "../services/baseservice";
import {AuthService} from "../services/auth.service";

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.css']
})
export class NavbarComponent {
  isLoggedIn: boolean = false;
  isAdmin: boolean = false;

  constructor(private router: Router, private baseService: BaseService, private authService: AuthService) {
    this.authService.isLoggedIn$.subscribe(response => {
      this.isLoggedIn = response;
    });

    this.authService.isAdmin$.subscribe(response => {
      this.isAdmin = response;
    });
  }

  logout() {
    this.authService.logout();
    this.router.navigate(['/']);
  }
}
