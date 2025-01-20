import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { StatisticEntry } from '../models/statistics-entry.model';
import { Observable, map, catchError, throwError } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class StatisticsService {
  private baseUrl = 'http://localhost:5000/api/statistics';

  constructor(private http: HttpClient) {}

  getStatistics(): Observable<StatisticEntry[]> {
    return this.http.get<StatisticEntry[]>(this.baseUrl).pipe(
      map((data: StatisticEntry[]) => data),
      catchError(this.handleError)
    );
  }

  private handleError(error: HttpErrorResponse): Observable<never> {
    let errorMessage = `Error: ${error.error.message}`;
    console.error(errorMessage);
    return throwError(() => new Error(errorMessage));
  }
}
