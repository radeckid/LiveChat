import { Component, OnInit } from '@angular/core';
import { ControlService } from './services/control.service';
import { MessageSignalRService } from './services/message-signal-r.service';
import { NotificationSignalRService } from './services/notification-signal-r.service';
import { User } from './user';
import { ConnectionSignalRService } from './services/connection-signal-r.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css'],
  providers: [MessageSignalRService, ConnectionSignalRService]
})
export class AppComponent implements OnInit {

  isLogged: boolean;
  user: User;

  constructor(private controlService: ControlService) { }

  ngOnInit(): void {
    this.controlService.getLogged().subscribe( isLogged => {
      this.isLogged = isLogged;
      console.log('isLogged received status');
    });

    this.controlService.getUser().subscribe(user => {
      this.user = user;
    });
  }
}
