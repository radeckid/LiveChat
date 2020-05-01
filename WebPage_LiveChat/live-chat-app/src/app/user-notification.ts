import { NotificationType } from './notification-type.enum';

export class UserNotification {
  senderEmail: string;
  receiverId: number;
  type: NotificationType;
  desc: string;
  extraData: string;
  date: Date;
}
