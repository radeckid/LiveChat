import { NotificationType } from './notification-type.enum';

export interface NotificationChartModel {
  ReceiverId: number;
  SenderId: number;
  Type: NotificationType;
  Desc: string;
  ExtraData: string;
  Date: Date;
}
