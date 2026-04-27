import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Inventory } from './inventory.model';

@Injectable({
  providedIn: 'root'
})
export class InventoryService {
  private http = inject(HttpClient);
  private apiUrl = 'http://localhost:5122/api/inventory';

  getAll() {
    return this.http.get<Inventory[]>(this.apiUrl);
  }

  getById(id: number) {
    return this.http.get<Inventory>(`${this.apiUrl}/${id}`);
  }

  save(data: Inventory) {
    return this.http.post(this.apiUrl, data);
  }

  update(data: Inventory) {
    return this.http.put(`${this.apiUrl}/${data.id}`, data);
  }

  delete(id: number) {
    return this.http.delete(`${this.apiUrl}/${id}`);
  }
}
