import { Component, HostListener, OnInit, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ProductService, Product } from '../../services/product';
import { CategoryService, Category } from '../../services/category';
import { SupplierService, Supplier } from '../../services/supplier';
import { UploadService } from '../../services/upload.service';

@Component({
  selector: 'app-inventory',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './inventory.html',
  styleUrls: ['./inventory.css']
})
export class InventoryComponent implements OnInit {

  private service = inject(ProductService);
  private categoryService = inject(CategoryService);
  private supplierService = inject(SupplierService);
  private uploadService = inject(UploadService);

  invObj = signal<Product[]>([]);
  categories = signal<Category[]>([]);
  suppliers = signal<Supplier[]>([]);
  Math = Math;

  totalItems = signal(0);
  totalQuantity = signal(0);
  totalValue = signal(0);

  page = 1;
  pageSize = 5;

  openedItem: Product | null = null;
  showModal = false;
  modalMode: 'VIEW' | 'EDIT' | 'ADD' = 'ADD';

  form: Partial<Product> = this.resetForm();
  loading = false;

  ngOnInit(): void {
    this.loadData();
    this.loadLookups();
  }

  loadData() {
    this.loading = true;
    this.service.getAll().subscribe({
      next: data => {
        this.invObj.set(data);
        this.calculateStats(data);
      },
      error: err => console.error(err),
      complete: () => this.loading = false
    });
  }

  loadLookups() {
    this.categoryService.getAll().subscribe(res => this.categories.set(res));
    this.supplierService.getAll().subscribe(res => this.suppliers.set(res));
  }

  calculateStats(data: Product[]) {
    this.totalItems.set(data.length);
    this.totalQuantity.set(data.reduce((acc, curr) => acc + curr.stockQuantity, 0));
    this.totalValue.set(data.reduce((acc, curr) => acc + (curr.stockQuantity * curr.price), 0));
  }

  get paginatedData(): Product[] {
    const start = (this.page - 1) * this.pageSize;
    return this.invObj().slice(start, start + this.pageSize);
  }

  get totalPages(): number {
    return Math.ceil(this.invObj().length / this.pageSize);
  }

  nextPage() { if (this.page < this.totalPages) this.page++; }
  prevPage() { if (this.page > 1) this.page--; }
  goToPage(p: number) { this.page = p; }
  getPagesArray(): number[] { return Array.from({ length: this.totalPages }, (_, i) => i + 1); }

  toggleMenu(item: Product, event: Event) {
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

  edit(item: Product) {
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

  view(item: Product) {
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

  delete(item: Product) {
    if (confirm('Delete this item?')) {
      this.service.delete(item.id).subscribe({
        next: () => this.loadData(),
        error: err => console.error(err)
      });
    }
  }

  onFileSelected(event: any) {
    const file: File = event.target.files[0];
    if (file) {
      this.uploadService.upload(file).subscribe({
        next: res => this.form.imageUrl = res.url,
        error: err => console.error('Upload failed', err)
      });
    }
  }

  save() {
    if (!this.form.name || !this.form.categoryId || !this.form.supplierId) {
      alert('Please fill out all required fields, including Category and Supplier.');
      return;
    }

    if (this.modalMode === 'EDIT' && this.form.id) {
      this.service.update(this.form.id, this.form as Product).subscribe({
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

  resetForm(): Partial<Product> {
    return {
      id: 0,
      name: '',
      description: '',
      stockQuantity: 0,
      price: 0,
      categoryId: 0,
      supplierId: 0,
      imageUrl: ''
    };
  }
}
