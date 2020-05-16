export interface Deletion {
  senderId: number;
  chatId: number;
  reason: string;
  memberId?: number;
}
