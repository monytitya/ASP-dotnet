import { Component, HostListener, OnInit, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { CustomerService, Customer } from '../../services/customer';

@Component({
  selector: 'app-customers',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './customers.html'
})
export class CustomersComponent implements OnInit {

  private service = inject(CustomerService);
  customers = signal<Customer[]>([]);
  Math = Math;

  page = 1;
  pageSize = 5;

  openedItem: Customer | null = null;
  showModal = false;
  modalMode: 'VIEW' | 'EDIT' | 'ADD' = 'ADD';

  form: Partial<Customer> = this.resetForm();
  loading = false;

  ngOnInit(): void {
    this.loadData();
  }

  loadData() {
    this.loading = true;
    this.service.getAll().subscribe({
      next: data => {
        this.customers.set(data);
        this.loading = false;
      },
      error: err => {
        console.error(err);
        this.loading = false;
      }
    });
  }

  get paginatedData(): Customer[] {
    const start = (this.page - 1) * this.pageSize;
    return this.customers().slice(start, start + this.pageSize);
  }

  get totalPages(): number {
    return Math.ceil(this.customers().length / this.pageSize);
  }

  nextPage() { if (this.page < this.totalPages) this.page++; }
  prevPage() { if (this.page > 1) this.page--; }
  goToPage(p: number) { this.page = p; }
  getPagesArray(): number[] { return Array.from({ length: this.totalPages }, (_, i) => i + 1); }

  toggleMenu(item: Customer, event: Event) {
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

  edit(item: Customer) {
    this.modalMode = 'EDIT';
    this.service.getById(item.id).subscribe({
      next: res => {
        this.form = res;
        this.showModal = true;
      },
      error: err => console.error(err)
    });
    this.openedItem = null;
  }

  view(item: Customer) {
    this.modalMode = 'VIEW';
    this.service.getById(item.id).subscribe({
      next: res => {
        this.form = res;
        this.showModal = true;
      },
      error: err => console.error(err)
    });
    this.openedItem = null;
  }

  delete(item: Customer) {
    if (confirm('Delete this customer?')) {
      this.service.delete(item.id).subscribe({
        next: () => this.loadData(),
        error: err => console.error(err)
      });
    }
  }

  save() {
    if (this.modalMode === 'EDIT' && this.form.id) {
      this.service.update(this.form.id, this.form as Customer).subscribe({
        next: () => {
          this.loadData();
          this.showModal = false;
        },
        error: err => console.error(err)
      });
    } else {
      this.service.create(this.form).subscribe({
        next: () => {
          this.loadData();
          this.showModal = false;
        },
        error: err => console.error(err)
      });
    }
  }

  resetForm(): Partial<Customer> {
    return { id: 0, name: '', phone: '', email: '' };
  }
}
