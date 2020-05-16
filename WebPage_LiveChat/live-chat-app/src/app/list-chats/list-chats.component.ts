import { Component, OnInit } from '@angular/core';
import { ControlService } from '../services/control.service';
import { User } from '../user';
import { Chat } from '../chat';
import { ChatType } from '../chat-type.enum';
import { Invitation } from '../invitation';

@Component({
  selector: 'app-list-chats',
  templateUrl: './list-chats.component.html',
  styleUrls: ['./list-chats.component.css']
})
export class ListChatsComponent implements OnInit {

  chatType: typeof ChatType = ChatType;
  isLogged: boolean;
  chats: Array<Chat>;
  user: User;
  isCreatingGroupChat: boolean;
  chatName: string;
  expression: string;
  isRequstToInviteToGroup: boolean;
  isRequestToDeleteMember: boolean;
  friends: Array<User>;
  memberCurrentChat: Array<User>;
  actualChatGroupSelected: number;

  constructor(private controlService: ControlService) {
    this.controlService.getUser().subscribe(user => {
      this.user = user;
    });
    this.controlService.getLogged().subscribe( isLogged => {
      if (isLogged) {
        this.controlService.getChats().subscribe(chats => {
          this.chats = chats;
        });
      }
    });

    this.controlService.getFriends().subscribe(friends => {
      this.friends = friends;
    });
   }

  ngOnInit(): void {

  }

  setChat(chat: Chat) {
    this.controlService.setChat(chat);
  }

  deleteChat(chat: Chat) {
    this.controlService.deleteMembership(chat.id, 'reason');
  }

  getChatName() {
    if (this.isCreatingGroupChat) {
      this.isCreatingGroupChat = false;
    } else {
      this.isCreatingGroupChat = true;
    }
  }

  createChat() {
    if (this.chatName == null || this.chatName.length === 0) {
      this.expression = 'Chat name cannot be empty';
    } else {
      this.controlService.createChat(this.chatName);
    }
  }

  showFriends(chat: Chat, isInviteRequest: boolean) {
    if (isInviteRequest) {
      if (this.isRequstToInviteToGroup) {
        this.isRequstToInviteToGroup = false;
      } else {
        this.controlService.getChatMembers(chat.id).subscribe(members => {
          const membersCurrentChat: Array<User> = new Array<User>();
          this.friends.forEach(friend => {
            if (!members.some(x => x.id === friend.id)) {
              membersCurrentChat.push(friend);
            }
          });
          this.memberCurrentChat = membersCurrentChat;
          this.isRequstToInviteToGroup = true;
      });
        this.actualChatGroupSelected = chat.id;
      }
    } else {
      if (this.isRequestToDeleteMember) {
        this.isRequestToDeleteMember = false;
      } else {
        this.controlService.getChatMembers(chat.id).subscribe(members => {
          const membersCurrentChat: Array<User> = new Array<User>();
          members.forEach(member => {
            if (member.id !== this.controlService.user.value.id) {
              membersCurrentChat.push(member);
            }
          });
          this.memberCurrentChat = membersCurrentChat;
          this.isRequestToDeleteMember = true;
      });
        this.actualChatGroupSelected = chat.id;
      }
    }
  }

  invite(friendId: number, chatId: number) {
    const invitation: Invitation = {userId: this.controlService.user.value.id, otherId: friendId, chatId};
    this.controlService.invite(invitation);
  }

  deleteMember(chatId: number, memberId: number) {
    this.controlService.deleteMembership(chatId, 'reason', memberId);
  }
}
