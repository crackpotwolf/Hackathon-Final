import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import {MainComponent} from "./main/main.component";
import {ProjectComponent} from "./project/project.component";
import {HeatmapComponent} from "./heatmap/heatmap.component";
import {FavoritesComponent} from "./main/favorites/favorites.component";

const routes: Routes = [
  {
    path: '',
    children: [
      {
        path: '',
        component: MainComponent
      },
      {
        path: 'project/:guid',
        component: ProjectComponent
      },
      {
        path: 'heat-map',
        component: HeatmapComponent
      },
      {
        path: 'favorites',
        component: FavoritesComponent
      },
    ]
  },
  {
    path: '**',
    redirectTo: '',
    pathMatch: 'full'
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class PagesRoutingModule {
}
