import {Component, OnInit} from '@angular/core';
import {TaskService} from '../../services/task.service';
import {CommonModule} from '@angular/common';
import {FormsModule} from '@angular/forms';
import {AuthService} from '../../services/auth.service';
import {CategoryService} from '../../services/category.service';
import {Category} from '../../interfaces/category';
import {TaskItem} from '../../interfaces/task-item';

@Component({
  selector: 'app-task-create',
  imports: [CommonModule, FormsModule],
  templateUrl: './task-create.component.html',
  styleUrl: './task-create.component.css'
})
export class TaskCreateComponent implements OnInit {
  newTaskName = '';
  newTaskCategory = 0;
  newTaskDescription = '';
  categories: Category[] = [];
  IsImportant: boolean = false
  dueDate: Date = new Date();

  minDate: Date = new Date();

  constructor(private taskService: TaskService,
              private auth: AuthService,
              private categoryService: CategoryService) {
  }

  ngOnInit() {
    this.minDate = new Date();
    this.updateTaskList()
  }

  private updateTaskList() {
    let userEmail = this.auth.getEmail()!
    this.categoryService.getCategoriesByUserEmail(userEmail)
      .subscribe({
        next: (data) => {
          this.categories = data;
        },
        error: (err) => console.error('Failed to load categories', err)
      });
  }

  createTask() {
    if (!this.newTaskName.trim()) return;

    const newTaskPayload: TaskItem = {
      id: 0,
      userEmailId: this.auth.getEmail()!,
      name: this.newTaskName,
      description: this.newTaskDescription,
      isImportant: this.IsImportant,
      isCompleted: false,
      categoryId: this.newTaskCategory,
      dueDate: new Date(this.dueDate).toISOString()
    };

    this.taskService.postNewTask(newTaskPayload as any).subscribe({
      next: () => {
        this.newTaskName = '';
        this.newTaskCategory = 0;
        this.updateTaskList();
      },
      error: (err) => {
        console.error('Backend rejected the data:', err.error);
      }
    });
  }

  toggleIsImportant() {
    this.IsImportant = !this.IsImportant;
  }

  protected readonly Date = Date;
}
