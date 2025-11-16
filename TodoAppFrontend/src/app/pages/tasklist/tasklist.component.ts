import { Component, OnInit } from '@angular/core';
import { TaskComponent } from '../../components/task/task.component';
import { TaskService } from '../../services/task.service';
import { NgForOf } from '@angular/common';
import { TaskItem } from '../../interfaces/task-item';
import {AuthService} from '../../services/auth.service';

@Component({
  selector: 'app-tasklist',
  imports: [
    TaskComponent,
    NgForOf
  ],
  templateUrl: './tasklist.component.html',
  styleUrls: ['./tasklist.component.css']
})
export class TasklistComponent implements OnInit {
  tasks: TaskItem[] = []

  constructor(
    private taskService: TaskService,
    private authService: AuthService) {}

  ngOnInit() {
    const userEmail = this.authService.getEmail();
    if(!userEmail){
      console.error('Not logged in')
      return;
    }

    this.taskService.getTasksByUser(userEmail).subscribe({
      next: tasksInfo => {
        this.tasks = tasksInfo;
      },
      error: err => console.error('Error fetching tasks: ', err)
    });
  }
}
