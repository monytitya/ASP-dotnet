import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class WeatherService {
  private http = inject(HttpClient);
  private apiUrl = 'http://localhost:5122/weatherforecast';

  getWeather(): Observable<any[]> {
    return this.http.get<any[]>(this.apiUrl);
  }
}
