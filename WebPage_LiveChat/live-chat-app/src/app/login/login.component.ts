import { Component, OnInit, ErrorHandler } from '@angular/core';
import { User } from '../user';
import { HttpService } from '../services/http.service';
import { HttpErrorResponse } from '@angular/common/http';
import { ControlService } from '../services/control.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {

  email: string;
  password: string;
  msg: string;

  constructor(private httpService: HttpService, private controlService: ControlService) { }

  ngOnInit(): void {
  }

  getUser() {
    const user: User = {Email: this.email, Password: this.password};
    this.httpService.getUser(user).subscribe(response => {
      this.controlService.setToken(response);
      this.controlService.setUser(user);
      console.log(response);
    }, (error: HttpErrorResponse) => {
      this.msg = error.status.toString() + ' ' + error.message;
    });
  }
}
