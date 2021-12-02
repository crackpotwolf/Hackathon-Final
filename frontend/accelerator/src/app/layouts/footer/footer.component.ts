import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-footer',
  templateUrl: './footer.component.html',
  styleUrls: ['./footer.component.sass']
})
export class FooterComponent implements OnInit {

  constructor() { }

  ngOnInit(): void {
  }

  currentYear() {
    let startYear = '2021';
    let current = new Date().getFullYear().toString()

    if(startYear == current)
      return startYear
    else
      return `${startYear}-${current}`
  }
}
