import { Chat } from './chat';

export interface Message {
  id: number;
  chatId: number;
  senderName: string;
  date: Date;
  content: string;
}
