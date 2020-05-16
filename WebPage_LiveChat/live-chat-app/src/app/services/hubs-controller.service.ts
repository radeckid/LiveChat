import { Injectable } from '@angular/core';
import { ConnectionSignalRService } from './connection-signal-r.service';
import { MessageSignalRService } from './message-signal-r.service';
import { NotificationSignalRService } from './notification-signal-r.service';
import { Subject, Observable } from 'rxjs';
import { UserNotification } from '../user-notification';
import { Message } from '../message';

@Injectable({
  providedIn: 'root'
})
export class HubsControllerService {

  messageData: Subject<Message> = new Subject<Message>();
  notificationData: Subject<UserNotification> = new Subject<UserNotification>();

  private isSuccessLogged: Subject<boolean> = new Subject<boolean>();
  private token: string;

  // tslint:disable-next-line: max-line-length
  constructor(private messageSignalRService: MessageSignalRService, private notificationSignalRService: NotificationSignalRService, private connectionSignalRSerivce: ConnectionSignalRService) {
    this.messageSignalRService.getMessageData().subscribe(message => {
      this.messageData.next(message);
    }, error => {
      console.log('cannot parse message data');
    });

    this.notificationSignalRService.getNotificationData().subscribe(notification => {
      this.notificationData.next(notification);
    }, error => {
      console.log('cannot parse notification data');
    });

    this.connectionSignalRSerivce.getIsSuccessLoggedStatus().subscribe(x => {
      this.isSuccessLogged.next(x);

      if (x) {
        this.messageSignalRService.startConnection(this.token);
        this.messageSignalRService.addTransferChartDataListener();

        this.notificationSignalRService.startConnection(this.token);
        this.notificationSignalRService.addTransferChartDataListener();
      }
    });
  }

  getMessage(): Observable<Message> {
    return this.messageData.asObservable();
  }

  getNotification(): Observable<UserNotification> {
    return this.notificationData.asObservable();
  }

  getIsSuccessLoggedStatus(): Observable<boolean> {
    return this.isSuccessLogged.asObservable();
  }

  startListen(userId: number, token: string) {
    this.token = token;
    this.connectionSignalRSerivce.startConnection(userId, this.token);
  }
}
