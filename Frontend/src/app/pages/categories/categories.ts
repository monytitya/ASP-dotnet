import { Component, HostListener, OnInit, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { CategoryService, Category } from '../../services/category';

@Component({
  selector: 'app-categories',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './categories.html'
})
export class CategoriesComponent implements OnInit {

  private service = inject(CategoryService);
  categories = signal<Category[]>([]);
  Math = Math;

  page = 1;
  pageSize = 5;

  openedItem: Category | null = null;
  showModal = false;
  modalMode: 'VIEW' | 'EDIT' | 'ADD' = 'ADD';

  form: Partial<Category> = this.resetForm();
  loading = false;

  ngOnInit(): void {
    this.loadData();
  }

  loadData() {
    this.loading = true;
    this.service.getAll().subscribe({
      next: data => this.categories.set(data),
      error: err => console.error(err),
      complete: () => this.loading = false
    });
  }

  get paginatedData(): Category[] {
    const start = (this.page - 1) * this.pageSize;
    return this.categories().slice(start, start + this.pageSize);
  }

  get totalPages(): number {
    return Math.ceil(this.categories().length / this.pageSize);
  }

  nextPage() { if (this.page < this.totalPages) this.page++; }
  prevPage() { if (this.page > 1) this.page--; }
  goToPage(p: number) { this.page = p; }
  getPagesArray(): number[] { return Array.from({ length: this.totalPages }, (_, i) => i + 1); }

  toggleMenu(item: Category, event: Event) {
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

  edit(item: Category) {
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

  view(item: Category) {
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

  delete(item: Category) {
    if (confirm('Delete this category?')) {
      this.service.delete(item.id).subscribe({
        next: () => this.loadData(),
        error: err => console.error(err)
      });
    }
  }

  save() {
    if (this.modalMode === 'EDIT' && this.form.id) {
      this.service.update(this.form.id, this.form as Category).subscribe({
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

  resetForm(): Partial<Category> {
    return { id: 0, name: '' };
  }
}
