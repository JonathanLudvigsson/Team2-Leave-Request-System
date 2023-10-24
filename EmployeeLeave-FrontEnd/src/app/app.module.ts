import { HttpClientModule } from '@angular/common/http';
import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule } from '@angular/forms';
import { AppComponent } from './app.component';
import { AppRoutingModule } from './app-routing.module';
import { HomeComponent } from './home/home.component';
import { UserComponent } from './user/user.component';
import { LoginComponent } from './login/login.component';
import { NavbarComponent } from './navbar/navbar.component';
import { LandingComponent } from './landing/landing.component';
import { AdminComponent } from './admin/admin.component';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { ViewUsersComponent } from './view-users/view-users.component';
import { ConfigLeaveTypesComponent } from './config-leave-types/config-leave-types.component';

@NgModule({
  declarations: [
    AppComponent,
    HomeComponent,
    UserComponent,
    LoginComponent,
    NavbarComponent,
    LandingComponent,
    AdminComponent,
    ViewUsersComponent,
    ConfigLeaveTypesComponent
  ],
  imports: [
    BrowserModule, HttpClientModule, AppRoutingModule, FormsModule, NgbModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
