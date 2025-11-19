export interface TaskItem {
  id: number;
  userEmailId: string;
  name: string;
  description?: string;
  isCompleted: boolean;
  isImportant: boolean;
  dueDate: string;
  categoryId: number;
}
