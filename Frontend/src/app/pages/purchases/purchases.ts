import { Component, HostListener, OnInit, inject, signal } from '@angular/core';
import { CommonModule, CurrencyPipe, DatePipe } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { PurchaseService, Purchase, PurchaseItem } from '../../services/purchase';
import { SupplierService, Supplier } from '../../services/supplier';
import { ProductService, Product } from '../../services/product';

@Component({
  selector: 'app-purchases',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './purchases.html'
})
export class PurchasesComponent implements OnInit {

  private purchaseService = inject(PurchaseService);
  private supplierService = inject(SupplierService);
  private productService = inject(ProductService);

  purchases = signal<Purchase[]>([]);
  suppliers = signal<Supplier[]>([]);
  products = signal<Product[]>([]);
  Math = Math;

  totalPurchases = signal(0);
  totalCost = signal(0);

  page = 1;
  pageSize = 5;

  openedItem: Purchase | null = null;
  showModal = false;
  modalMode: 'VIEW' | 'ADD' = 'ADD';
  loading = false;

  form: Partial<Purchase> = this.resetForm();

  ngOnInit(): void {
    this.loadData();
    this.loadLookups();
  }

  loadData() {
    this.loading = true;
    this.purchaseService.getAll().subscribe({
      next: data => {
        this.purchases.set(data);
        this.calculateStats(data);
      },
      error: err => console.error(err),
      complete: () => this.loading = false
    });
  }

  loadLookups() {
    this.supplierService.getAll().subscribe(res => this.suppliers.set(res));
    this.productService.getAll().subscribe(res => this.products.set(res));
  }

  calculateStats(data: Purchase[]) {
    this.totalPurchases.set(data.length);
    this.totalCost.set(data.reduce((acc, curr) => acc + curr.purchaseItems.reduce((a, c) => a + (c.quantity * c.costPrice), 0), 0));
  }

  get paginatedData(): Purchase[] {
    const start = (this.page - 1) * this.pageSize;
    return this.purchases().slice(start, start + this.pageSize);
  }

  get totalPages(): number {
    return Math.ceil(this.purchases().length / this.pageSize);
  }

  nextPage() { if (this.page < this.totalPages) this.page++; }
  prevPage() { if (this.page > 1) this.page--; }
  goToPage(p: number) { this.page = p; }
  getPagesArray(): number[] { return Array.from({ length: this.totalPages }, (_, i) => i + 1); }

  toggleMenu(item: Purchase, event: Event) {
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

  view(item: Purchase) {
    this.modalMode = 'VIEW';
    this.form = item;
    this.showModal = true;
    this.openedItem = null;
  }

  delete(item: Purchase) {
    if (confirm('Delete this purchase?')) {
      this.purchaseService.delete(item.id).subscribe({
        next: () => this.loadData(),
        error: err => console.error(err)
      });
    }
  }

  addPurchaseItem() {
    if (!this.form.purchaseItems) this.form.purchaseItems = [];
    this.form.purchaseItems.push({ id: 0, purchaseId: 0, productId: 0, quantity: 1, costPrice: 0 });
  }

  removePurchaseItem(index: number) {
    if (this.form.purchaseItems) {
      this.form.purchaseItems.splice(index, 1);
    }
  }

  onProductSelect(item: any) {
    // Optionally fetch default cost price, but usually it's input manually.
  }

  calculateTotal(): number {
    if (this.form.purchaseItems) {
      return this.form.purchaseItems.reduce((acc, curr) => acc + (curr.quantity * curr.costPrice), 0);
    }
    return 0;
  }

  save() {
    if (this.modalMode === 'ADD') {
      this.form.purchaseDate = new Date().toISOString().split('T')[0];
      this.purchaseService.create(this.form).subscribe({
        next: () => {
          this.loadData();
          this.showModal = false;
        },
        error: err => console.error(err)
      });
    }
  }

  resetForm(): Partial<Purchase> {
    return {
      supplierId: 0,
      purchaseItems: []
    };
  }
}
