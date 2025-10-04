import { Component } from '@angular/core';
import {Router, RouterOutlet} from '@angular/router';
import {AuthService} from './services/auth.service';
import {LeftSidebarComponent} from './sidebars/left-sidebar/left-sidebar.component';
import {RightSidebarComponent} from './sidebars/right-sidebar/right-sidebar.component';

@Component({
  selector: 'app-root',
  imports: [
    LeftSidebarComponent,
    RightSidebarComponent,
    RouterOutlet
  ],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent {
  title = 'TodoAppFrontend';

  constructor(private router: Router, private auth: AuthService) { }
  ngOnInit() {
    this.auth.isLoggedIn() ? this.router.navigate(['/tasklist']) : this.router.navigate(['/login']);
  }
}
