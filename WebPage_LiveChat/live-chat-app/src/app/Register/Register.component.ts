import { Component, OnInit, ErrorHandler } from '@angular/core';
import { User } from '../user';
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
    const user: User = {Email: this.email, Password: this.password};
    this.httpSerice.addUser(user).subscribe(() => {
      this.msg = 'User created';
    }, (error: HttpErrorResponse) => {
      console.log('err');
    });
  }
}
