import { Injectable } from '@angular/core';
import {HttpClient, HttpHeaders} from '@angular/common/http';
import {environment} from '../../environment';
import { TaskItem } from '../interfaces/task-item';
import {Observable, tap} from 'rxjs';
import { jwtDecode} from 'jwt-decode';
import {AuthService} from './auth.service';

@Injectable({
  providedIn: 'root'
})
export class TaskService {
  apiUrl = environment.apiUrl + '/tasks';

  constructor(private http: HttpClient, private authService: AuthService) { }

  getTasksByUser(userEmailId: string) {
    let token: string | null = this.authService.getToken();

    const headers = new HttpHeaders({
      'Authorization': `Bearer ${this.authService.getToken()}`
    })

    return this.http.get<TaskItem[]>(`${this.apiUrl}/${userEmailId}`, {headers});
  }

  postNewTask(task: TaskItem) {
    return this.http.post<TaskItem>(`${this.apiUrl}`, task);
  }

  updateTask(task: TaskItem) {
    return this.http.put(`${this.apiUrl}/{task.id}`, task);
  }

  deleteTask(task: TaskItem) {
    return this.http.delete(`${this.apiUrl}/${task.id}`);
  }
}
