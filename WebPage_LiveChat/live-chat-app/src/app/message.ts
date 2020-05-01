import { Chat } from './chat';

export interface Message {
  id: number;
  senderName: string;
  date: Date;
  content: string;
}
