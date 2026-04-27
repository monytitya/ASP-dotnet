import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Supplier } from './supplier';
import { Product } from './product';

export interface PurchaseItem {
  id: number;
  purchaseId: number;
  productId: number;
  quantity: number;
  costPrice: number;
  product?: Product;
}

export interface Purchase {
  id: number;
  supplierId: number;
  purchaseDate: Date | string;
  supplier?: Supplier;
  purchaseItems: PurchaseItem[];
}

@Injectable({
  providedIn: 'root'
})
export class PurchaseService {
  private http = inject(HttpClient);
  private apiUrl = 'http://localhost:5122/api/purchases';

  getAll(): Observable<Purchase[]> {
    return this.http.get<Purchase[]>(this.apiUrl);
  }

  getById(id: number): Observable<Purchase> {
    return this.http.get<Purchase>(`${this.apiUrl}/${id}`);
  }

  create(purchase: Partial<Purchase>): Observable<Purchase> {
    return this.http.post<Purchase>(this.apiUrl, purchase);
  }

  update(id: number, purchase: Purchase): Observable<void> {
    return this.http.put<void>(`${this.apiUrl}/${id}`, purchase);
  }

  delete(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }
}
