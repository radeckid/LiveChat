import { Injectable } from '@angular/core';
import * as signalR from '@aspnet/signalr';
import { ControlService } from './control.service';
import { Subject, Observable, BehaviorSubject } from 'rxjs';
import { IHttpConnectionOptions } from '@aspnet/signalr';

@Injectable({
  providedIn: 'root'
})
export class MessageSignalRService {

  private hubConnection: signalR.HubConnection;

  messageData: Subject<any> = new Subject<any>();

  constructor() { }

   startConnection(token: string) {
    const options: IHttpConnectionOptions = {
      accessTokenFactory: () => {
        return token;
      }
    };
    this.hubConnection = new signalR.HubConnectionBuilder()

     .withUrl('http://localhost:53064/messagechart', options)
     .build();

    this.hubConnection
     .start()
     .then(() => console.log('Connection started'))
     .catch(err => console.log('Error while starting connection: ' + err));
   }

   getMessageData(): Observable<any> {
     return this.messageData.asObservable();
   }

   addTransferChartDataListener() {
     console.log('listen');
     this.hubConnection.on('transferchartdata', (data) => {
       console.log('Just Received message');
       this.messageData.next(data[0]);
     });
   }
}
