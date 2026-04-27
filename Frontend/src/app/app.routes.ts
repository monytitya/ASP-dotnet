import { Routes } from '@angular/router';
import { EmployeeComponent } from './pages/employee/employee';
import { InventoryComponent } from './pages/inventory/inventory';

export const routes: Routes = [
  { path: 'dashboard', loadComponent: () => import('./pages/dashboard/dashboard').then(c => c.DashboardComponent) },
  { path: 'employees', component: EmployeeComponent },
  { path: 'inventory', component: InventoryComponent },
  { path: 'sales', loadComponent: () => import('./pages/sales/sales').then(c => c.SalesComponent) },
  { path: 'purchases', loadComponent: () => import('./pages/purchases/purchases').then(c => c.PurchasesComponent) },
  { path: 'customers', loadComponent: () => import('./pages/customers/customers').then(c => c.CustomersComponent) },
  { path: 'suppliers', loadComponent: () => import('./pages/suppliers/suppliers').then(c => c.SuppliersComponent) },
  { path: 'categories', loadComponent: () => import('./pages/categories/categories').then(c => c.CategoriesComponent) },
  { path: '', redirectTo: '/dashboard', pathMatch: 'full' }
];
