import { Component, OnInit } from '@angular/core';
import { ControlService } from './services/control.service';
import { MessageSignalRService } from './services/message-signal-r.service';
import { NotificationSignalRService } from './services/notification-signal-r.service';
import { User } from './user';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css'],
  providers: []
})
export class AppComponent implements OnInit {

  isLogged: boolean;
  user: User;

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
