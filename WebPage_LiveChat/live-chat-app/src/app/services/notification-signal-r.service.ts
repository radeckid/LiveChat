import { Injectable } from '@angular/core';
import { ControlService } from './control.service';
import * as signalR from '@aspnet/signalr';

@Injectable({
  providedIn: 'root'
})
export class NotificationSignalRService {

  private hubConnection: signalR.HubConnection;

  constructor(private controlService: ControlService) { }

   startConnection() {
     this.hubConnection = new signalR.HubConnectionBuilder()

     .withUrl('http://localhost:53064/notificationchart')
     .build();

     this.hubConnection
     .start()
     .then(() => console.log('Connection started notification'))
     .catch(err => console.log('Error while starting connection: ' + err));
   }

   addTransferChartDataListener() {
     console.log('listen');
     this.hubConnection.on('transfernotifications', (data) => {
       this.controlService.addNotification(data[0]);
     });
   }
}
