import { Component, HostListener, OnInit, inject, signal } from '@angular/core';
import { CommonModule, CurrencyPipe, DatePipe } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { OrderService, Order, OrderItem } from '../../services/order';
import { CustomerService, Customer } from '../../services/customer';
import { ProductService, Product } from '../../services/product';

@Component({
  selector: 'app-sales',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './sales.html'
})
export class SalesComponent implements OnInit {

  private orderService = inject(OrderService);
  private customerService = inject(CustomerService);
  private productService = inject(ProductService);

  orders = signal<Order[]>([]);
  customers = signal<Customer[]>([]);
  products = signal<Product[]>([]);
  Math = Math;

  // Statistics
  totalOrders = signal(0);
  totalRevenue = signal(0);

  page = 1;
  pageSize = 5;

  openedItem: Order | null = null;
  showModal = false;
  modalMode: 'VIEW' | 'ADD' = 'ADD';
  loading = false;

  form: Partial<Order> = this.resetForm();

  ngOnInit(): void {
    this.loadData();
    this.loadLookups();
  }

  loadData() {
    this.loading = true;
    this.orderService.getAll().subscribe({
      next: data => {
        this.orders.set(data);
        this.calculateStats(data);
      },
      error: err => console.error(err),
      complete: () => this.loading = false
    });
  }

  loadLookups() {
    this.customerService.getAll().subscribe(res => this.customers.set(res));
    this.productService.getAll().subscribe(res => this.products.set(res));
  }

  calculateStats(data: Order[]) {
    this.totalOrders.set(data.length);
    this.totalRevenue.set(data.reduce((acc, curr) => acc + curr.totalAmount, 0));
  }

  get paginatedData(): Order[] {
    const start = (this.page - 1) * this.pageSize;
    return this.orders().slice(start, start + this.pageSize);
  }

  get totalPages(): number {
    return Math.ceil(this.orders().length / this.pageSize);
  }

  nextPage() { if (this.page < this.totalPages) this.page++; }
  prevPage() { if (this.page > 1) this.page--; }
  goToPage(p: number) { this.page = p; }
  getPagesArray(): number[] { return Array.from({ length: this.totalPages }, (_, i) => i + 1); }

  toggleMenu(item: Order, event: Event) {
    event.stopPropagation();
    this.openedItem = this.openedItem === item ? null : item;
  }

  @HostListener('document:click')
  closeMenu() { this.openedItem = null; }

  addNew() {
    this.modalMode = 'ADD';
    this.form = this.resetForm();
    this.showModal = true;
  }

  view(item: Order) {
    this.modalMode = 'VIEW';
    this.form = item;
    this.showModal = true;
    this.openedItem = null;
  }

  delete(item: Order) {
    if (confirm('Delete this order?')) {
      this.orderService.delete(item.id).subscribe({
        next: () => this.loadData(),
        error: err => console.error(err)
      });
    }
  }

  addOrderItem() {
    if (!this.form.orderItems) this.form.orderItems = [];
    this.form.orderItems.push({ id: 0, orderId: 0, productId: 0, quantity: 1, price: 0 });
  }

  removeOrderItem(index: number) {
    if (this.form.orderItems) {
      this.form.orderItems.splice(index, 1);
      this.calculateTotal();
    }
  }

  onProductSelect(item: any) {
    const prod = this.products().find(p => p.id == item.productId);
    if (prod) {
      item.price = prod.price;
      this.calculateTotal();
    }
  }

  calculateTotal() {
    if (this.form.orderItems) {
      this.form.totalAmount = this.form.orderItems.reduce((acc, curr) => acc + (curr.quantity * curr.price), 0);
    }
  }

  save() {
    if (this.modalMode === 'ADD') {
      this.form.orderDate = new Date().toISOString().split('T')[0];
      this.orderService.create(this.form).subscribe({
        next: () => {
          this.loadData();
          this.showModal = false;
        },
        error: err => console.error(err)
      });
    }
  }

  resetForm(): Partial<Order> {
    return {
      customerId: 0,
      totalAmount: 0,
      orderItems: []
    };
  }
}
