export interface TaskItem {
  id: number;
  name: string;
  description?: string;
  isCompleted: boolean;
  isImportant: boolean;
  dueDate: Date;
  category: string;
}
