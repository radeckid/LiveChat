import { Component, OnInit, ErrorHandler } from '@angular/core';
import { User } from '../user';
import { HttpService } from '../services/http.service';
import { HttpErrorResponse } from '@angular/common/http';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {

  email: string;
  password: string;
  msg: string;

  constructor(private httpService: HttpService) { }

  ngOnInit(): void {
  }

  getUser() {
    const user: User = {Email: this.email, Password: this.password};
    this.httpService.getUser(user).subscribe(() => {
      this.msg = 'Login successful';
    }, (error: HttpErrorResponse) => {
      this.msg = error.status.toString() + ' ' + error.message;
    });
  }
}
