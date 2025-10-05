import { Component } from '@angular/core';
import {TaskComponent} from '../../components/task/task.component';
import {TaskService} from '../../services/task.service';
import {NgForOf} from '@angular/common';
import {Task} from '../../interfaces/task';

@Component({
  selector: 'app-tasklist',
  imports: [
    TaskComponent,
    NgForOf
  ],
  templateUrl: './tasklist.component.html',
  styleUrl: './tasklist.component.css'
})
export class TasklistComponent {
  tasks: Task[] = []

  constructor(public taskService: TaskService) {}

  ngOnInit() {
    this.taskService.getAllTasks().subscribe(tasksInfo => {
      this.tasks = tasksInfo
    })
  }
}
