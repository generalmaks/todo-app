import { Routes } from '@angular/router';
import {AppComponent} from './app.component';
import {TasklistComponent} from './pages/tasklist/tasklist.component';
import {RegisterMenuComponent} from './pages/register-menu/register-menu.component';
import {LoginMenuComponent} from './pages/login-menu/login-menu.component';

export const routes: Routes = [
  {path: '', component: AppComponent},
  { path: 'tasklist', component: TasklistComponent },
  { path: 'register', component: RegisterMenuComponent},
  { path: 'login', component: LoginMenuComponent},
  { path: 'register', component: RegisterMenuComponent},
];
