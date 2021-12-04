import {Component, EventEmitter, OnInit, Output} from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {AuthService} from "../../../services/auth/auth.service";
import {MessageService} from "primeng/api";
import {WrapperComponent} from "../wrapper/wrapper.component";
import {ProjectsService} from "../../../services/projects/projects.service";
import {ProjectServiceEventData} from "../../../services/projects/data/ProjectServiceEventData";
import {ProjectServiceEventType} from "../../../services/projects/data/ProjectServiceEventType";


@Component({
  selector: 'app-topbar',
  templateUrl: './topbar.component.html',
  styleUrls: ['./topbar.component.sass']
})

export class TopbarComponent implements OnInit {

  // @Output() mobileMenuButtonClicked = new EventEmitter();
  @Output() settingsButtonClicked = new EventEmitter();


  /**
   * Количество новых уведомлений
   */
  countNewNotifications: number = 0;


  constructor(private http: HttpClient,
              private messageService: MessageService,
              private authService: AuthService,
              private projectsService: ProjectsService) {
    this.projectsService.onEvents.subscribe(p => this.onEvents(p));
  }

  ngOnInit(): void {
  }

  // /**
  //  * Toggle the menu bar when having mobile screen
  //  */
  // toggleMobileMenu(event: any) {
  //   event.preventDefault();
  //   this.mobileMenuButtonClicked.emit();
  // }
  displaySidebar: boolean = false;

  /**
   * Выход
   */
  logout() {
    this.authService.logout();
  }

  /**
   * Клик по кнопе "Поиск"
   * @param event
   */
  onClickSearch(event: MouseEvent) {
    window.location.href = '/';
    // WrapperComponent.notImplemented(this.messageService);
  }

  /**
   * Клик по кнопе "Избранное"
   * @param event
   */
  onClickFavorites(event: MouseEvent) {
    WrapperComponent.notImplemented(this.messageService);
  }


  private onEvents(p: ProjectServiceEventData) {
    switch (p.type) {
      case ProjectServiceEventType.SendRequestInvest:
        this.displaySidebar = false;
        break;
    }
  }

  openPage(link: string) {
    window.location.href = link;
  }

  onClickPerson() {
    WrapperComponent.notImplemented(this.messageService);
  }
}
