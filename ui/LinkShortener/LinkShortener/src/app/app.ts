import { Component, Input, output, Output, signal } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { FormsModule } from "@angular/forms";
import { WelcomePageComponent } from './welcome_page/welcome_page';
import { UserPageComponent } from './user_page/user_page';
import { AboutPageComponent } from './about_page/about_page';
import { parseJwt } from './utils/token_decoder'; // Assuming you have a utility function to parse JWT

@Component({
  selector: 'app-root',
  standalone: true,
  imports: 
  [
    RouterOutlet,
    WelcomePageComponent,
    FormsModule,
    UserPageComponent,
    AboutPageComponent
  ],
  templateUrl: './app.html',
  styleUrls: ['./app.css']
})
export class App {
  protected readonly title = signal('LinkShortener');

  isLoggedIn: boolean = false;
  isAboutPage: boolean = false;
  isAdmin: boolean = false;
  token: string = "";
  user = {
    id: null,
    login: null,
    role: null
  }

  handleAboutPage() {
    this.isAboutPage = !this.isAboutPage;
    console.log("!App!", "Toggling About Page visibility:", this.isAboutPage);
  }

  handleLoginSuccess(token: string) {
    this.isLoggedIn = true;
    this.token = token;
    localStorage.setItem('token', token); // Store token in localStorage
    console.log("!App!", "Login successful, user is now logged in.");
    console.log("!App!", "Token received in App:", this.token);
    this.updateUserInfo();
  }

  updateUserInfo() {
    let role = "http://schemas.microsoft.com/ws/2008/06/identity/claims/role"; // Path of Claims.Roles
    let payload = parseJwt(this.token);
    this.user.id = payload.id;
    this.user.login = payload.login;
    this.user.role = payload[role];
    console.log("!App!", "User information updated:", this.user);

    if (this.user.role === "Admin") {
      this.isAdmin = true;
      console.log("!App!", "User is an Admin.");
    }
  }
}