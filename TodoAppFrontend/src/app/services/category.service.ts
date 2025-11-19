import {Injectable, signal} from '@angular/core';
import {AuthService} from './auth.service';
import {HttpClient, HttpHeaders} from '@angular/common/http';
import {Category} from '../interfaces/category';
import {environment} from '../../environment';

@Injectable({
  providedIn: 'root'
})
export class CategoryService {
  private apiUrl = environment.apiUrl + '/categories';

  constructor(
    private auth: AuthService,
    private http: HttpClient
  ) {
  }

  getCategoriesByUserEmail(email: string) {
    return this.http.get<Category[]>(`${this.apiUrl}/${email}`, {headers: this.getHeaders()})
  }

  private getHeaders(): HttpHeaders {
    const headers = new HttpHeaders({
      'Authorization': `Bearer ${this.auth.getToken()}`
    })
    return headers;
  }
}
