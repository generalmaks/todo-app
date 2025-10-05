import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import {environment} from '../../environment';
import { Task } from '../interfaces/task';
import {Observable, tap} from 'rxjs';
import { jwtDecode} from 'jwt-decode';

@Injectable({
  providedIn: 'root'
})
export class TaskService {
  apiUrl = environment.apiUrl + '/Tasks';

  constructor(private http: HttpClient) { }

  getAllTasks() {
    return this.http.get<Task[]>(`${this.apiUrl}`);
  }

  postNewTask(task: Task) {
    return this.http.post<Task>(`${this.apiUrl}`, task);
  }

  updateTask(task: Task) {
    return this.http.put(`${this.apiUrl}/{task.id}`, task);
  }

  deleteTask(task: Task) {
    return this.http.delete(`${this.apiUrl}/${task.id}`);
  }
}
