import { Component, OnInit } from '@angular/core';
import { User } from '../user';
import { ControlService } from '../services/control.service';

@Component({
  selector: 'app-searching-user-combo-box',
  templateUrl: './searching-user-combo-box.component.html',
  styleUrls: ['./searching-user-combo-box.component.css']
})
export class SearchingUserComboBoxComponent implements OnInit {

  name: string;
  isExpanded: boolean;
  users: Array<User>;

  constructor(private controlService: ControlService) { }

  ngOnInit(): void {
    this.controlService.getUsers().subscribe(users => {
      const temp: Array<User> = new Array<User>();
      users.forEach(user => {
        if (user.id !== this.controlService.user.value.id) {
            temp.push(user);
        }
      });
      this.users = temp;
    });
  }

  expand() {
    if (this.isExpanded) {
      this.isExpanded = false;
    } else {
      this.isExpanded = true;
    }
  }

  invite(otherId: number) {
    console.log('seatchig');
    this.controlService.invite({userId: this.controlService.user.value.id, otherId});
  }
}
