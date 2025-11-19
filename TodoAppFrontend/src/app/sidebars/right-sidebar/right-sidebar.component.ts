import {Component, OnInit} from '@angular/core';
import {TaskService} from '../../services/task.service';
import {TaskItem} from '../../interfaces/task-item';
import {CommonModule} from '@angular/common';
import {FormsModule} from '@angular/forms';
import {AuthService} from '../../services/auth.service';
import {CategoryService} from '../../services/category.service';
import {Category} from '../../interfaces/category';

@Component({
  selector: 'app-right-sidebar',
  imports: [CommonModule, FormsModule],
  templateUrl: './right-sidebar.component.html',
  styleUrl: './right-sidebar.component.css'
})
export class RightSidebarComponent implements OnInit {
  newTaskName = '';
  newTaskCategory = 0;
  categories: Category[] = [];

  constructor(private taskService: TaskService,
              private auth: AuthService,
              private categoryService: CategoryService) {
  }

  ngOnInit() {
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
    ;
  }

  createTask() {
    if (!this.newTaskName.trim()) return;

    const newTaskPayload = {
      userEmailId: this.auth.getEmail(),
      name: this.newTaskName,
      description: "Description",
      isImportant: false,
      categoryId: this.newTaskCategory,
      dueDate: new Date().toISOString(),
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
}
