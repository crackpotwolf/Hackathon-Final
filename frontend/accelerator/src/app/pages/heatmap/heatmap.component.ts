import { Component, OnInit } from '@angular/core';
import { HttpClient } from "@angular/common/http";
import {jsonmap} from './datamap';

declare var ymaps:any;

@Component({
  selector: 'app-heatmap',
  templateUrl: './heatmap.component.html',
  styleUrls: ['./heatmap.component.sass']
})

export class HeatmapComponent implements OnInit {

  constructor(private http: HttpClient) { }

  public data:any = [];

  public map: any;

  text: string;

  results: string[] = ["22"];

  search(event) {

  }

  ngOnInit(): void {

    this.results = ["22", "ff"];

    console.log(this.results);

    ymaps.ready(['Heatmap']).then(function init() {
      var obj = jsonmap;

      var myMap = new ymaps.Map('YMapsID', {
        center: [55.733835, 37.588227],
        zoom: 11,
        controls: []
      });

      var data = [];

      for (var i = 0; i < obj.length; i++) {
        data.push([obj[i].Cells.geoData.coordinates[0], obj[i].Cells.geoData.coordinates[1]])
      }

      var heatmap = new ymaps.Heatmap(data, {

        // Радиус влияния.
        radius: 15,

        // Нужно ли уменьшать пиксельный размер точек при уменьшении зума. False - не нужно.
        dissipating: false,

        // Прозрачность тепловой карты.
        opacity: 0.8,

        // Прозрачность у медианной по весу точки.
        intensityOfMidpoint: 0.2,

        // JSON описание градиента.
        gradient: {
          0.1: 'rgba(128, 255, 0, 0.7)',
          0.2: 'rgba(255, 255, 0, 0.8)',
          0.7: 'rgba(234, 72, 58, 0.9)',
          1.0: 'rgba(162, 36, 25, 1)'
        }
      });
      heatmap.setMap(myMap);
    });
  }
}
