import { ChatType } from './chat-type.enum';

export interface Chat {
  id: number;
  ownerId: number;
  name: string;
  type: ChatType;
}
