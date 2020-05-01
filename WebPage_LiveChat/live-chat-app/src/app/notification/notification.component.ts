import { Component, OnInit } from '@angular/core';
import { ControlService } from '../services/control.service';
import { UserNotification } from '../user-notification';
import { NotificationType } from '../notification-type.enum';
import { strict } from 'assert';
import { stringify } from 'querystring';

@Component({
  selector: 'app-notification',
  templateUrl: './notification.component.html',
  styleUrls: ['./notification.component.css']
})
export class NotificationComponent implements OnInit {

  notificationType: typeof NotificationType = NotificationType;
  notifications: Array<UserNotification>;

  constructor(private controlService: ControlService) { }

  ngOnInit(): void {
    this.controlService.getNotifications().subscribe(notifications => {
      this.notifications = notifications;
    });

  }

  action(type: NotificationType, isAccepted: boolean, id: number) {
    this.controlService.notificationProcess(type, {id, action: isAccepted, userId: this.controlService.user.value.id});
  }
}
