import { Component, OnInit } from '@angular/core';
import { HttpService } from './services/http.service';
import { ControlService } from './services/control.service';
import { MessageSignalRService } from './services/signal-r.service';
import { NotificationSignalRService } from './services/notification-signal-r.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css'],
  providers: [HttpService, ControlService, MessageSignalRService]
})
export class AppComponent implements OnInit {

  isLogged: boolean;

  constructor(private controlService: ControlService, private messageSignalRService: MessageSignalRService,
              private notificationSignalRService: NotificationSignalRService) { }

  ngOnInit(): void {
    this.controlService.getLogged().subscribe( isLogged => {
      this.isLogged = isLogged;
      console.log('isLogged received status');
    });

    this.messageSignalRService.startConnection();
    this.messageSignalRService.addTransferChartDataListener();

    this.notificationSignalRService.startConnection();
    this.notificationSignalRService.addTransferChartDataListener();
  }

}
