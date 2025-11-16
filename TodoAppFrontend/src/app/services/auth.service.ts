import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import {environment} from '../../environment';
import {Observable, tap} from 'rxjs';
import { jwtDecode} from 'jwt-decode';
import {IAuthResponse, User} from '../interfaces/user';


@Injectable({
  providedIn: 'root'
})
export class AuthService {
  apiUrl = environment.apiUrl;
  tokenKey = environment.tokenKey;

  constructor(private http: HttpClient) { }

  register(email: string, unhashedPassword: string): Observable<User> {
    return this.http.post<User>(`${this.apiUrl}/users`, { email, unhashedPassword });
  }

  login(email: string, unhashedPassword: string): Observable<IAuthResponse> {
    return this.http.post<IAuthResponse>(`${this.apiUrl}/login`, { email, unhashedPassword })
      .pipe(tap(response => {
        localStorage.setItem(this.tokenKey, response.token);
      }));
  }

  logout() {
    localStorage.removeItem(this.tokenKey);
  }

  getToken() {
    return localStorage.getItem(this.tokenKey);
  }

  isLoggedIn() {
    return !!this.getToken();
  }

  getClaim(claim: string): string | null {
    const token = this.getToken();
    if (!token) return null;

    try {
      const decoded: any = jwtDecode(token);
      return decoded[claim] || null;
    } catch (e) {
      console.error('Invalid JWT: ', e);
      return null;
    }
  }

// Then:
  getEmail() {
    return this.getClaim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress");
  }
}
