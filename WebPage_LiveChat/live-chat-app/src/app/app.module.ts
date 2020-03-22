import { BrowserModule } from '@angular/platform-browser';
import { NgModule, Output } from '@angular/core';
import { HttpClientModule } from '@angular/common/http';
import { AppComponent } from './app.component';
import { RegisterComponent } from './Register/Register.component';
import { LoginComponent } from './login/login.component';
import { FormsModule } from '@angular/forms';
import { ChatComponent } from './chat/chat.component';
import { ListChatsComponent } from './list-chats/list-chats.component';

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
      FormsModule
   ],
   providers: [],
   bootstrap: [
      AppComponent
   ]
})
export class AppModule { }
