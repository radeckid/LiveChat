import { Injectable } from '@angular/core';
import * as signalR from '@aspnet/signalr';
import { Subject, Observable } from 'rxjs';
import { ControlService } from './control.service';
import { MessageChartModel } from '../message-chart-model';

@Injectable({
  providedIn: 'root'
})
export class MessageSignalRService {

  data: Subject<Array<MessageChartModel>>;
  private hubConnection: signalR.HubConnection;

  constructor(private controlService: ControlService) {
    this.data = new Subject<Array<MessageChartModel>>();

   }

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
     this.hubConnection.on('transfermesasges', (data) => {
       if (this.controlService.user.Id === data.ReceiverId) {
        this.data.next(data);
        console.log(data);
       }
     });
   }

   getDate(): Observable<Array<MessageChartModel>> {
     return this.data.asObservable();
   }
}
