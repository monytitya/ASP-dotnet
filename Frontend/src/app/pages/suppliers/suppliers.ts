import { Component, HostListener, OnInit, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { SupplierService, Supplier } from '../../services/supplier';

@Component({
  selector: 'app-suppliers',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './suppliers.html'
})
export class SuppliersComponent implements OnInit {

  private service = inject(SupplierService);
  suppliers = signal<Supplier[]>([]);
  Math = Math;

  page = 1;
  pageSize = 5;

  openedItem: Supplier | null = null;
  showModal = false;
  modalMode: 'VIEW' | 'EDIT' | 'ADD' = 'ADD';

  form: Partial<Supplier> = this.resetForm();
  loading = false;

  ngOnInit(): void {
    this.loadData();
  }

  loadData() {
    this.loading = true;
    this.service.getAll().subscribe({
      next: data => this.suppliers.set(data),
      error: err => console.error(err),
      complete: () => this.loading = false
    });
  }

  get paginatedData(): Supplier[] {
    const start = (this.page - 1) * this.pageSize;
    return this.suppliers().slice(start, start + this.pageSize);
  }

  get totalPages(): number {
    return Math.ceil(this.suppliers().length / this.pageSize);
  }

  nextPage() { if (this.page < this.totalPages) this.page++; }
  prevPage() { if (this.page > 1) this.page--; }
  goToPage(p: number) { this.page = p; }
  getPagesArray(): number[] { return Array.from({ length: this.totalPages }, (_, i) => i + 1); }

  toggleMenu(item: Supplier, event: Event) {
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

  edit(item: Supplier) {
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

  view(item: Supplier) {
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

  delete(item: Supplier) {
    if (confirm('Delete this supplier?')) {
      this.service.delete(item.id).subscribe({
        next: () => this.loadData(),
        error: err => console.error(err)
      });
    }
  }

  save() {
    if (this.modalMode === 'EDIT' && this.form.id) {
      this.service.update(this.form.id, this.form as Supplier).subscribe({
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

  resetForm(): Partial<Supplier> {
    return { id: 0, name: '', contactInfo: '', address: '' };
  }
}
