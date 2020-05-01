import { Injectable } from '@angular/core';
import * as signalR from '@aspnet/signalr';
import { ControlService } from './control.service';

@Injectable({
  providedIn: 'root'
})
export class MessageSignalRService {

  private hubConnection: signalR.HubConnection;

  constructor(private controlService: ControlService) { }

   startConnection() {
     this.hubConnection = new signalR.HubConnectionBuilder()

     .withUrl('http://localhost:53064/messagechart')
     .build();

     this.hubConnection
     .start()
     .then(() => console.log('Connection started'))
     .catch(err => console.log('Error while starting connection: ' + err));
   }

   addTransferChartDataListener() {
     console.log('listen');
     this.hubConnection.on('transferchartdata', (data) => {
       this.controlService.setRefreshStatus(data);
       console.log('message');
     });
   }
}
