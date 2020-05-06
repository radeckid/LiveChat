import { Injectable } from '@angular/core';
import { ControlService } from './control.service';
import * as signalR from '@aspnet/signalr';
import { Subject, Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ConnectionSignalRService {

  private hubConnection: signalR.HubConnection;
  private isSuccessLogged: Subject<boolean> = new Subject<boolean>();

  constructor() { }

  getIsSuccessLoggedStatus(): Observable<boolean> {
    return this.isSuccessLogged.asObservable();
  }

   startConnection(userId: number) {
     this.hubConnection = new signalR.HubConnectionBuilder()

     .withUrl('http://localhost:53064/connection')
     .build();

     this.hubConnection
     .start()
     .then(() => {
       this.hubConnection.invoke<boolean>('GetConnectionId', userId).then(value => {
         if (!value.valueOf()) {
            this.isSuccessLogged.next(false);
         } else {
          this.isSuccessLogged.next(true);
         }
       });
     })
     .catch(err => {
      console.log('Error while starting connection: ' + err);
      this.isSuccessLogged.next(false);
     });
     this.isSuccessLogged.next(false);
   }
}
