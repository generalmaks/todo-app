import {Component, Input} from '@angular/core';
import {TaskService} from '../../services/task.service';
import {TaskItem} from '../../interfaces/task-item';
import {DatePipe} from '@angular/common';

@Component({
  selector: 'app-task',
  imports: [
    DatePipe
  ],
  templateUrl: './task.component.html',
  styleUrl: './task.component.css'
})
export class TaskComponent {
  @Input() task!: TaskItem;

  constructor(private taskService: TaskService) {}

  toggleCompleted() {
    this.task.isCompleted = !this.task.isCompleted;
    this.updateTask()
  }

  toggleImportant() {
    this.task.isImportant = !this.task.isImportant;
    this.updateTask()
  }

  updateTask() {
    this.taskService.updateTask(this.task).subscribe({
      next: response => {
        console.log(response);
      },
      error: error => {
        console.error(error);
      }
    })
  }
}
