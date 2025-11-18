import {Component} from '@angular/core';
import {FormControl, FormGroup, FormsModule, ReactiveFormsModule, Validators} from '@angular/forms';
import {NgIf} from '@angular/common';
import {AuthService} from '../../services/auth.service';
import {Router, RouterLink} from '@angular/router';

@Component({
  selector: 'app-login-menu',
  imports: [
    FormsModule,
    ReactiveFormsModule,
    NgIf,
    RouterLink
  ],
  templateUrl: './login-menu.component.html',
  styleUrl: './login-menu.component.css'
})
export class LoginMenuComponent {
  submitted = false;

  form: FormGroup = new FormGroup({
    "email": new FormControl('', [Validators.required, Validators.email]),
    "password": new FormControl('', [Validators.required, Validators.minLength(8)]),
  })

  constructor(
    private authService: AuthService,
    private router: Router,) {
  }

  onSubmit() {
    this.submitted = true;
    if (this.form.valid) {
      this.authService.login(this.form.value.email, this.form.value.password).subscribe({
        next: res => {
          console.log('Success: ', res);
          this.router.navigate(['/tasklist']);
        },
        error: err => {
          console.error('Error status: ', err);
        }
      })
    }
  }
}
