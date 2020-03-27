import { Component, OnInit } from '@angular/core';
import { HttpService } from './services/http.service';
import { ControlService } from './services/control.service';
import { SignalRService } from './services/signal-r.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css'],
  providers: [HttpService, ControlService, SignalRService]
})
export class AppComponent implements OnInit {

  isLogged: boolean;

  constructor(private controlService: ControlService, private signalRService: SignalRService) { }

  ngOnInit(): void {
    this.controlService.getLogged().subscribe( isLogged => {
      this.isLogged = isLogged;
      console.log('isLogged received status');
    });

    this.signalRService.startConnection();
    this.signalRService.addTransferChartDataListener();
  }

}
