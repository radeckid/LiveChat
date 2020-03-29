import { Component, OnInit } from '@angular/core';
import { HttpService } from '../services/http.service';
import { HttpErrorResponse } from '@angular/common/http';
import { ControlService } from '../services/control.service';
import { UserDTO } from '../userDTO';

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
    const user: UserDTO = {Email: this.email, Password: this.password};
    this.httpService.getUser(user).subscribe(response => {
      this.email = '';
      this.password = '';
      this.httpService.setToken(response.token);
      this.controlService.setUser(response.user);
    }, (error: HttpErrorResponse) => {
      this.msg = error.status.toString() + ' ' + error.message;
    });
  }
}
