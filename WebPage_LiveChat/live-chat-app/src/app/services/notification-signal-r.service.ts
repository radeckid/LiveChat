import { Injectable } from '@angular/core';
import { NotificationChartModel } from '../notification-chart-model';
import { Subject, Observable } from 'rxjs';
import { ControlService } from './control.service';
import * as signalR from '@aspnet/signalr';
import { User } from '../user';

@Injectable({
  providedIn: 'root'
})
export class NotificationSignalRService {

  data: Subject<Array<NotificationChartModel>>;
  private hubConnection: signalR.HubConnection;
  user: User;

  constructor(private controlService: ControlService) {
    this.data = new Subject<Array<NotificationChartModel>>();
    this.controlService.getUser().subscribe(user => {
      this.user = user;
    });
   }

   startConnection() {
     this.hubConnection = new signalR.HubConnectionBuilder()

     .withUrl('http://localhost:53064/notificationchart')
     .build();

     this.hubConnection
     .start()
     .then(() => console.log('Connection started'))
     .catch(err => console.log('Error while starting connection: ' + err));
   }

   addTransferChartDataListener() {
     console.log('listen');
     this.hubConnection.on('transfernotifications', (data) => {
       if (this.user.id === data.SenderId) {
        this.data.next(data);
        console.log(data);
       }
     });
   }

   getDate(): Observable<Array<NotificationChartModel>> {
     return this.data.asObservable();
   }
}
