import {Component} from '@angular/core';
import {FormsModule} from '@angular/forms';
import {HttpClient} from '@angular/common/http';
import {CategoryService} from '../../services/category.service';
import {AuthService} from '../../services/auth.service';

@Component({
  selector: 'app-category-create',
  imports: [
    FormsModule
  ],
  templateUrl: './category-create.component.html',
  styleUrl: './category-create.component.css'
})
export class CategoryCreateComponent {
  newCategoryName: string = '';
  newCategoryDescription: string = '';

  constructor(private categoryService: CategoryService,
              private auth: AuthService) {
  }

  createCategory() {
    const newCategory = {
      name: this.newCategoryName,
      description: this.newCategoryDescription,
      userEmailId: this.auth.getEmail()
    }

    this.categoryService.postCategory(newCategory).subscribe({
      next: data => {
        this.newCategoryName = '';
        this.newCategoryDescription = '';
      }, error: err => {
        console.error(err)
      }
    })
  }
}
