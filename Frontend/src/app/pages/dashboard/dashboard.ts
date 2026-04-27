import { Component, OnInit, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { OrderService, Order } from '../../services/order';
import { ProductService, Product } from '../../services/product';
import { CustomerService, Customer } from '../../services/customer';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './dashboard.html'
})
export class DashboardComponent implements OnInit {

  private orderService = inject(OrderService);
  private productService = inject(ProductService);
  private customerService = inject(CustomerService);

  totalRevenue = signal(0);
  totalOrders = signal(0);
  totalProducts = signal(0);
  totalCustomers = signal(0);

  recentOrders = signal<Order[]>([]);
  lowStockProducts = signal<Product[]>([]);

  ngOnInit(): void {
    this.orderService.getAll().subscribe(orders => {
      this.totalOrders.set(orders.length);
      this.totalRevenue.set(orders.reduce((sum, order) => sum + order.totalAmount, 0));
      this.recentOrders.set(orders.slice(-5).reverse());
    });

    this.productService.getAll().subscribe(products => {
      this.totalProducts.set(products.length);
      this.lowStockProducts.set(products.filter(p => p.stockQuantity < 10).slice(0, 5));
    });

    this.customerService.getAll().subscribe(customers => {
      this.totalCustomers.set(customers.length);
    });
  }
}
