import {Injectable, signal} from '@angular/core';
import {HttpClient, HttpHeaders} from '@angular/common/http';
import {environment} from '../../environment';
import {TaskItem} from '../interfaces/task-item';
import {AuthService} from './auth.service';
import {tap} from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class TaskService {
  apiUrl = environment.apiUrl + '/tasks';

  private taskSignal = signal<TaskItem[]>([]);

  readonly tasks = this.taskSignal.asReadonly();

  constructor(private http: HttpClient,
              private authService: AuthService) {
  }

  getTasksByUser(userEmailId: string) {
    return this.http.get<TaskItem[]>(`${this.apiUrl}/${userEmailId}`, {headers: this.getHeaders()})
      .pipe(
        tap(tasks => {
          this.taskSignal.set(tasks)
        })
      );
  }

  postNewTask(task: TaskItem) {
    return this.http.post<TaskItem>(`${this.apiUrl}`, task, {headers: this.getHeaders()})
      .pipe(
        tap(newTask => {
          this.taskSignal.update(currentTasks => [...currentTasks, newTask]);
        }),
      );
  }

  updateTask(task: TaskItem) {
    return this.http.put<TaskItem>(`${this.apiUrl}/${task.id}`, task, {headers: this.getHeaders()})
      .pipe(
        tap((updatedTask: TaskItem) => {
          this.taskSignal.update((tasks: TaskItem[]) =>
            tasks.map((t: TaskItem) => t.id === updatedTask.id ? updatedTask : t)
          );
        })
      );
  }

  deleteTask(task: TaskItem) {
    return this.http.delete(`${this.apiUrl}/${task.id}`, {headers: this.getHeaders()})
      .pipe(
        // 4. Remove it from the signal instantly
        tap(() => {
          this.taskSignal.update(tasks => tasks.filter(t => t.id !== task.id));
        })
      );
  }

  private getHeaders(): HttpHeaders {
    const headers = new HttpHeaders({
      'Authorization': `Bearer ${this.authService.getToken()}`
    })
    return headers;
  }
}
