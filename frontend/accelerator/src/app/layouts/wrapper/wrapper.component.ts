import {Component, OnInit} from '@angular/core';
import {MessageService} from "primeng/api";

@Component({
  selector: 'app-wrapper',
  templateUrl: './wrapper.component.html',
  styleUrls: ['./wrapper.component.sass']
})
export class WrapperComponent implements OnInit {

  constructor(
    private messageService: MessageService
  ) {
  }

  ngOnInit(): void {
    document.body.setAttribute('data-sidebar', 'dark');
    document.body.removeAttribute('data-layout-size');
    document.body.removeAttribute('data-layout');
    document.body.removeAttribute('data-topbar');
    document.body.classList.remove('auth-body-bg');
  }

  /**
   * On mobile toggle button clicked
   */
  onToggleMobileMenu() {
    document.body.classList.toggle('sidebar-enable');
    document.body.classList.toggle('vertical-collpsed');

    if (window.screen.width <= 768) {
      document.body.classList.remove('vertical-collpsed');
    }
  }

  /**
   * on settings button clicked from topbar
   */
  onSettingsButtonClicked() {
    document.body.classList.toggle('right-bar-enabled');
  }


  /**
   * Уведомление "Еще не рализовано"
   */
  static notImplemented(messageService: MessageService) {
    messageService.add({
      severity: 'info',
      summary: 'Ещё не рализовано',
    })
  }
}
