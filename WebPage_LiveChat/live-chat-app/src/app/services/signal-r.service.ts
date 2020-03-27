import { Injectable } from '@angular/core';
import * as signalR from "@aspnet/signalr";
import { ChartModel } from '../chart-model';
import { Subject, Observable } from 'rxjs';
import { ControlService } from './control.service';

@Injectable({
  providedIn: 'root'
})
export class SignalRService {

  data: Subject<Array<ChartModel>>;
  private hubConnection: signalR.HubConnection;

  constructor(private controlService: ControlService) {
    this.data = new Subject<Array<ChartModel>>();

   }

   startConnection() {
     this.hubConnection = new signalR.HubConnectionBuilder()

     .withUrl('http://localhost:53064/chart')
     .build();

     this.hubConnection
     .start()
     .then(() => console.log('Connection started'))
     .catch(err => console.log('Error while starting connection: '+ err));
   }

   addTransferChartDataListener() {
     console.log('listen');
     this.hubConnection.on('transferchartdata', (data) => {
       if(this.controlService.user.)
      this.data.next(data);
      console.log(data);
     });
   }

   getDate(): Observable<Array<ChartModel>> {
     return this.data.asObservable();
   }
}
