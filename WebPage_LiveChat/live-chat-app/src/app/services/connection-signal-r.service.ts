import { Injectable } from '@angular/core';
import * as signalR from '@aspnet/signalr';
import { Subject, Observable } from 'rxjs';
import { IHttpConnectionOptions } from '@aspnet/signalr';


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

   startConnection(userId: number, token: string) {
    const options: IHttpConnectionOptions = {
      accessTokenFactory: () => {
        return token;
      }
    };

    this.hubConnection = new signalR.HubConnectionBuilder()

     .withUrl('http://localhost:53064/connection', options)
     .build();

    this.hubConnection
     .start()
     .then(() => this.hubConnection.invoke<boolean>('GetConnectionId', userId).then(value => {
      if (!value.valueOf()) {
         this.isSuccessLogged.next(false);
      } else {
       this.isSuccessLogged.next(true);
      }
    })
     .catch(err => {
      console.log('Error while starting connection: ' + err);
      this.isSuccessLogged.next(false);
     }));
     this.isSuccessLogged.next(false);
   }
}
