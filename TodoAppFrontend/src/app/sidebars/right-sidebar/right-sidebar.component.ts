import {Component} from '@angular/core';
import {CommonModule} from '@angular/common';
import {FormsModule} from '@angular/forms';
import {TaskCreateComponent} from '../../components/task-create/task-create.component';
import {CategoryCreateComponent} from '../../components/category-create/category-create.component';

@Component({
  selector: 'app-right-sidebar',
  imports: [CommonModule, FormsModule, TaskCreateComponent, CategoryCreateComponent],
  templateUrl: './right-sidebar.component.html',
  styleUrl: './right-sidebar.component.css'
})
export class RightSidebarComponent {
  selected: 'task' | 'category' | 'view' = 'task'
}
