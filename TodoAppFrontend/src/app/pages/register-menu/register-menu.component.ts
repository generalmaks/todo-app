import { Component } from '@angular/core';
import {FormControl, FormGroup, FormsModule, ReactiveFormsModule, Validators} from '@angular/forms';
import {NgIf} from '@angular/common';
import {AuthService} from '../../services/auth.service';
import {RouterLink} from '@angular/router';

@Component({
  selector: 'app-register-menu',
  imports: [
    FormsModule,
    NgIf,
    ReactiveFormsModule,
    RouterLink
  ],
  templateUrl: './register-menu.component.html',
  styleUrl: './register-menu.component.css'
})
export class RegisterMenuComponent {
  submitted = false;

  form: FormGroup = new FormGroup({
    "email": new FormControl('', [Validators.required, Validators.email]),
    "password": new FormControl('', [Validators.required, Validators.minLength(8)]),
  })

  constructor(private authService: AuthService) {
  }

  onSubmit(){
    this.submitted = true;
    if(this.form.valid){
      this.authService.register(this.form.value.email, this.form.value.password).subscribe({
        next: res => {
          console.log('Success: ', res);
        },
        error: err => {
          console.error('Error status: ', err);
        }
      })
    }
  }
}
