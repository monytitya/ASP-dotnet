import { Component, HostListener, OnInit, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Employee } from './employee.model';
import { EmployeeService } from './employee.service';
import { UploadService } from '../../services/upload.service';

@Component({
  selector: 'app-employee',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './employee.html',
  styleUrls: ['./employee.css']
})
export class EmployeeComponent implements OnInit {

  private service = inject(EmployeeService);
  private uploadService = inject(UploadService);
  empObj = signal<Employee[]>([]);

  // Statistics
  totalEmployees = signal(0);
  totalSalary = signal(0);
  avgSalary = signal(0);
  highestSalary = signal(0);

  page = 1;
  pageSize = 5;

  openedItem: Employee | null = null;

  showModal = false;
  modalMode: 'VIEW' | 'EDIT' | 'ADD' = 'ADD';

  form: Employee = this.resetForm();

  loading = false;

  ngOnInit(): void {
    this.loadData();
  }

  // ================= LOAD DATA =================
  loadData() {
    this.loading = true;
    this.service.getAll().subscribe({
      next: data => {
        this.empObj.set(data);
        this.calculateStats(data);
      },
      error: err => {
        console.error(err);
        alert('Failed to load employees: ' + (err.message || 'Unknown error'));
      },
      complete: () => this.loading = false
    });
  }

  calculateStats(data: Employee[]) {
    this.totalEmployees.set(data.length);
    const sum = data.reduce((acc, curr) => acc + curr.salary, 0);
    this.totalSalary.set(sum);
    this.avgSalary.set(data.length > 0 ? sum / data.length : 0);
    this.highestSalary.set(data.length > 0 ? Math.max(...data.map(e => e.salary)) : 0);
  }

  // ================= PAGINATION =================
  get paginatedData(): Employee[] {
    const start = (this.page - 1) * this.pageSize;
    return this.empObj().slice(start, start + this.pageSize);
  }

  get totalPages(): number {
    return Math.ceil(this.empObj().length / this.pageSize);
  }

  nextPage() {
    if (this.page < this.totalPages) this.page++;
  }

  prevPage() {
    if (this.page > 1) this.page--;
  }

  goToPage(p: number) {
    this.page = p;
  }

  getPagesArray(): number[] {
    return Array.from({ length: this.totalPages }, (_, i) => i + 1);
  }

  toggleMenu(item: Employee, event: Event) {
    event.stopPropagation();
    this.openedItem = this.openedItem === item ? null : item;
  }

  @HostListener('document:click')
  closeMenu() {
    this.openedItem = null;
  }

  addNew() {
    this.modalMode = 'ADD';
    this.form = this.resetForm();
    this.showModal = true;
  }

  edit(item: Employee) {
    this.modalMode = 'EDIT';

    this.service.getById(item.id).subscribe({
      next: res => {
        this.form = res;
        this.showModal = true;
      },
      error: err => {
        console.error(err);
        alert('Failed to fetch employee details: ' + (err.error?.message || err.message));
      }
    });

    this.openedItem = null;
  }

  view(item: Employee) {
    this.modalMode = 'VIEW';

    this.service.getById(item.id).subscribe({
      next: res => {
        this.form = res;
        this.showModal = true;
      },
      error: err => {
        console.error(err);
        alert('Failed to fetch employee details: ' + (err.error?.message || err.message));
      }
    });

    this.openedItem = null;
  }

  delete(item: Employee) {
    if (confirm('Delete this employee?')) {
      this.service.delete(item.id).subscribe({
        next: () => this.loadData(),
        error: err => {
          console.error(err);
          alert('Failed to delete employee: ' + (err.error?.message || err.message));
        }
      });
    }
  }

  save() {
    if (this.modalMode === 'EDIT') {
      this.service.update(this.form).subscribe({
        next: () => {
          this.loadData();
          this.showModal = false;
        },
        error: err => {
          console.error(err);
          alert('Failed to update employee: ' + (err.error?.message || err.message));
        }
      });
    } else {
      this.service.save(this.form).subscribe({
        next: () => {
          this.loadData();
          this.showModal = false;
        },
        error: err => {
          console.error(err);
          alert('Failed to save employee: ' + (err.error?.message || err.message));
        }
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

  resetForm(): Employee {
    return {
      id: 0,
      name: '',
      email: '',
      salary: 0,
      imageUrl: ''
    };
  }
}
