import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { HttpClientModule } from '@angular/common/http';
import { AppComponent } from './app.component';
import { RegisterComponent } from './Register/Register.component';
import { LoginComponent } from './login/login.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { ChatComponent } from './chat/chat.component';
import { ListChatsComponent } from './list-chats/list-chats.component';
import { ChartsModule } from 'ng2-charts';
import { HttpService } from './services/http.service';
import { ControlService } from './services/control.service';
import { MessageSignalRService } from './services/message-signal-r.service';

@NgModule({
   declarations: [
      AppComponent,
      RegisterComponent,
      LoginComponent,
      ChatComponent,
      ListChatsComponent
   ],
   imports: [
      BrowserModule,
      HttpClientModule,
      FormsModule,
      ChartsModule
   ],
   providers: [HttpService, ControlService, MessageSignalRService],
   bootstrap: [
      AppComponent
   ]
})
export class AppModule { }
