import { User } from './user';

export interface Message {
  Id: number;
  Sender: User;
  Receiver: User;
  Date: Date;
  Content: string;
}
