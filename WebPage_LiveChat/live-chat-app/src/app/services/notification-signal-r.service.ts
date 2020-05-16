import { Injectable } from '@angular/core';
import { ControlService } from './control.service';
import * as signalR from '@aspnet/signalr';
import { Subject, Observable } from 'rxjs';
import { IHttpConnectionOptions } from '@aspnet/signalr';

@Injectable({
  providedIn: 'root'
})
export class NotificationSignalRService {

  private hubConnection: signalR.HubConnection;

  notificaionData: Subject<any> = new Subject<any>();

  constructor() { }

   startConnection(token: string) {
    const options: IHttpConnectionOptions = {
      accessTokenFactory: () => {
        return token;
      }
    };

    this.hubConnection = new signalR.HubConnectionBuilder()

     .withUrl('http://localhost:53064/notificationchart', options)
     .build();

    this.hubConnection
     .start()
     .then(() => console.log('Connection started notification'))
     .catch(err => console.log('Error while starting connection: ' + err));
   }

   getNotificationData(): Observable<any> {
    return this.notificaionData.asObservable();
  }

   addTransferChartDataListener() {
     console.log('listen');
     this.hubConnection.on('transfernotifications', (data) => {
       this.notificaionData.next(data[0]);
     });
   }
}
