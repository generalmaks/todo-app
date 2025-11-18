import {Component} from '@angular/core';
import {Router} from '@angular/router';
import {LeftSidebarButtonComponent} from '../../components/left-sidebar-button/left-sidebar-button.component';
import {NgForOf} from '@angular/common';
import {AuthService} from '../../services/auth.service';

interface MenuItem {
  label: string;
  icon?: string;
  route?: string;
  action?: () => void;
}

@Component({
  selector: 'app-left-sidebar',
  imports: [
    LeftSidebarButtonComponent,
    NgForOf
  ],
  templateUrl: './left-sidebar.component.html'
})
export class LeftSidebarComponent {
  constructor(public router: Router, private auth: AuthService) {
  }

  // getter runs every time Angular checks for changes
  get menuItems(): MenuItem[] {
    return [
      {label: 'Tasks', route: '/tasklist'},
      {
        label: this.auth.isLoggedIn() ? 'Logout' : 'Login',
        action: () => this.logout()
      },
      {label: 'Alert!', action: () => alert('Alert!')}
    ];
  }

  logout() {
    this.auth.logout();
    this.router.navigate(['/login']);
  }
}
