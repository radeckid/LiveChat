import { Component, OnInit } from '@angular/core';
import { UserDTO } from '../userDTO';
import { HttpService } from '../services/http.service';
import { HttpErrorResponse } from '@angular/common/http';

@Component({
  selector: 'app-Register',
  templateUrl: './Register.component.html',
  styleUrls: ['./Register.component.css'],

})
export class RegisterComponent implements OnInit {

  email: string;
  password: string;
  msg: string;

  constructor(private httpSerice: HttpService) { }

  ngOnInit() {
  }

  addUser() {
    const user: UserDTO = {Email: this.email, Password: this.password};
    this.httpSerice.addUser(user).subscribe(() => {
      this.email = '';
      this.password = '';
      this.msg = 'User created';
    }, (error: HttpErrorResponse) => {
      this.msg = error.message;
      console.log('err');
    });
  }
}
