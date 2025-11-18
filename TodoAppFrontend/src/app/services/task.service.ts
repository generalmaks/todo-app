import {Injectable} from '@angular/core';
import {HttpClient, HttpHeaders} from '@angular/common/http';
import {environment} from '../../environment';
import {TaskItem} from '../interfaces/task-item';
import {AuthService} from './auth.service';

@Injectable({
  providedIn: 'root'
})
export class TaskService {
  apiUrl = environment.apiUrl + '/tasks';

  constructor(private http: HttpClient, private authService: AuthService) {
  }

  getTasksByUser(userEmailId: string) {
    let token: string | null = this.authService.getToken();

    const headers = new HttpHeaders({
      'Authorization': `Bearer ${this.authService.getToken()}`
    })

    return this.http.get<TaskItem[]>(`${this.apiUrl}/${userEmailId}`, {headers});
  }

  postNewTask(task: TaskItem) {
    let token: string | null = this.authService.getToken();

    const headers = new HttpHeaders({
      'Authorization': `Bearer ${this.authService.getToken()}`
    })

    return this.http.post<TaskItem>(`${this.apiUrl}`, task, { headers });
  }

  updateTask(task: TaskItem) {
    let token: string | null = this.authService.getToken();

    const headers = new HttpHeaders({
      'Authorization': `Bearer ${this.authService.getToken()}`
    })

    return this.http.put(`${this.apiUrl}/${task.id}`, task, {headers});
  }

  deleteTask(task: TaskItem) {
    let token: string | null = this.authService.getToken();

    const headers = new HttpHeaders({
      'Authorization': `Bearer ${this.authService.getToken()}`
    })

    return this.http.delete(`${this.apiUrl}/${task.id}`, {headers});
  }
}
