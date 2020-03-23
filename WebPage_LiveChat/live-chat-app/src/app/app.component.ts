import { Component, OnInit } from '@angular/core';
import { HttpService } from './services/http.service';
import { ControlService } from './services/control.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css'],
  providers: [HttpService, ControlService]
})
export class AppComponent implements OnInit {

  isLogged: boolean;

  constructor(private controlService: ControlService) { }

  ngOnInit(): void {
    this.controlService.getLogged().subscribe( isLogged => {
      this.isLogged = isLogged;
    });
  }
}
