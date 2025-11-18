import {User} from './user';

export interface Category {
  id: number;
  name: string;
  description?: string;
  userEmailId: string;
  user: User;
}
