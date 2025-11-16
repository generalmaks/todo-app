export interface TaskItem {
  id: string;
  name: string;
  description?: string;
  isCompleted: boolean;
  isImportant: boolean;
  dueDate: Date;
  category: string;
}
