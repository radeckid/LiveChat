import { Injectable } from '@angular/core';
import { ControlService } from './services/control.service';
import * as signalR from '@aspnet/signalr';

@Injectable({
  providedIn: 'root'
})
export class ConnectionSignalRService {

  private hubConnection: signalR.HubConnection;

  constructor() { }

   startConnection(userId: number) {
     this.hubConnection = new signalR.HubConnectionBuilder()

     .withUrl('http://localhost:53064/connection')
     .build();

     this.hubConnection
     .start()
     .then(() => {
       this.hubConnection.invoke('GetConnectionId', userId);
     })
     .catch(err => console.log('Error while starting connection: ' + err));
   }
}
