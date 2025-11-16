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
}
