import { UserRole } from './enum/user-role.enum';
import {TaskItem} from './task-item';
import {Category} from './category';

export interface User {
  email: string;
  passwordHash: string;
  role: UserRole;
  tasks: TaskItem[];
  categories: Category[];
}

export interface IAuthResponse {
  token: string;
  email: string;
}
