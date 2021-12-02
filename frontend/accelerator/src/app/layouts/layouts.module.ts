import {NgModule} from '@angular/core';
import {CommonModule} from '@angular/common';
import {RouterModule} from '@angular/router';

import {FooterComponent} from "./footer/footer.component";
import {SidebarComponent} from "./sidebar/sidebar.component";
import {TopbarComponent} from "./topbar/topbar.component";
import {WrapperComponent} from './wrapper/wrapper.component';
import {PerfectScrollbarModule} from "ngx-perfect-scrollbar";
import {RightsidebarComponent} from './rightsidebar/rightsidebar.component';
import {ClickOutsideModule} from "ng-click-outside";
import {OverlayPanelModule} from "primeng/overlaypanel";
import {BadgeModule} from "primeng/badge";
import {DividerModule} from "primeng/divider";
import {ToastModule} from "primeng/toast";
import {MatMenuModule} from "@angular/material/menu";
import {ButtonModule} from "primeng/button";
import {SidebarModule} from "primeng/sidebar";
import {PagesModule} from "../pages/pages.module";
import {RequestForInnovationComponent} from "../pages/request-for-innovation/request-for-innovation.component";

@NgModule({
  declarations: [TopbarComponent, FooterComponent, SidebarComponent, WrapperComponent, RightsidebarComponent],
  imports: [
    CommonModule,
    RouterModule,
    PerfectScrollbarModule,
    ClickOutsideModule,
    OverlayPanelModule,
    BadgeModule,
    DividerModule,
    ToastModule,
    MatMenuModule,
    ButtonModule,
    SidebarModule,
    PagesModule
  ],
  exports: [TopbarComponent, FooterComponent, SidebarComponent, WrapperComponent]
})
export class LayoutsModule {
}

