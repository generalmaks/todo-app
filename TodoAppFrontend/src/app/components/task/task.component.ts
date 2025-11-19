import {Component, Input, EventEmitter, Output} from '@angular/core';
import {TaskService} from '../../services/task.service';
import {TaskItem} from '../../interfaces/task-item';
import {CommonModule, DatePipe} from '@angular/common';
import {FormsModule} from '@angular/forms';

@Component({
  selector: 'app-task',
  imports: [
    DatePipe,
    FormsModule,
    CommonModule,
  ],
  templateUrl: './task.component.html',
  styleUrl: './task.component.css'
})
export class TaskComponent {
  @Input() task!: TaskItem;

  @Output() taskDeleted = new EventEmitter<number>();

  constructor(private taskService: TaskService) {
  }

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

  deleteTask() {
    this.taskService.deleteTask(this.task).subscribe({
      next: response => {
        console.log("Successfully deleted task: " + response);
        this.taskDeleted.emit(this.task.id)
      },
      error: error => {
        console.error(error);
      }
    })
  }

  updateName() {
    if (this.task.name.trim()) {
      this.updateTask();
    }
  }

  updateCategory() {

  }
}
