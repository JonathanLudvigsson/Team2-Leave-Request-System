import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HomeComponent } from './home/home.component';
import { RouterModule, Routes } from '@angular/router';
import { UserComponent } from './user/user.component';
import { LoginComponent } from './login/login.component';
import { AuthGuard } from './auth.guard';
import { LandingComponent } from './landing/landing.component';
import { AdminComponent } from './admin/admin.component';
import { ViewUsersComponent } from './view-users/view-users.component';
import { ConfigLeaveTypesComponent } from './config-leave-types/config-leave-types.component';
 

const routes: Routes = [
  { path: '', component: LandingComponent },
  { path: 'login', component: LoginComponent },
  { path: 'home', component: HomeComponent },
  { path: 'user', component: UserComponent, canActivate: [AuthGuard] },
  { path: 'admin', component: AdminComponent, canActivate: [AuthGuard] },
  { path: 'viewUsers', component: ViewUsersComponent },
  { path:'configLeaveTypes', component:ConfigLeaveTypesComponent}
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
