import { Component, EventEmitter, Input, Output } from '@angular/core';
import {NgIf} from '@angular/common';

@Component({
  selector: 'app-left-sidebar-button',
  imports: [],
  templateUrl: './left-sidebar-button.component.html',
  styleUrl: './left-sidebar-button.component.css'
})
export class LeftSidebarButtonComponent {
  @Input() label: string = ''
  @Input() route: string | null = null;
  action: Function | null = null;

  @Output() clicked = new EventEmitter<void>();

  onClick() {
    this.clicked.emit();
  }
}
