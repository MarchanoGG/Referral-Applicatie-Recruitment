import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-tutorial',
  templateUrl: './tutorial.component.html',
  styleUrls: ['./tutorial.component.less']
})
export class TutorialComponent implements OnInit {
  title = 'Referral Applicatie Recruitment';

  constructor() { }

  ngOnInit(): void {
  }

}
