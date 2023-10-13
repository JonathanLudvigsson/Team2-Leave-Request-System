import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HomeComponent } from './home/home.component';
import { RouterModule, Routes } from '@angular/router';
import { UserComponent } from './user/user.component';
import { LoginComponent } from './login/login.component';
import { AuthGuard } from './auth.guard';
 

const routes: Routes = [
  { path: '', component: LoginComponent },
  { path: 'adadada', component: UserComponent },
  { path: 'home', component: HomeComponent },
  { path: 'user', component: UserComponent, canActivate: [AuthGuard] },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
