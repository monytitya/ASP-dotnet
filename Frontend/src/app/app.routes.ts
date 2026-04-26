import { Routes } from '@angular/router';
import { EmployeeComponent } from './pages/employee/employee';

export const routes: Routes = [
  { path: 'employees', component: EmployeeComponent },
  { path: '', redirectTo: '/employees', pathMatch: 'full' }
];
