import { Component, Input, Output, EventEmitter } from "@angular/core";
import { FormsModule } from "@angular/forms"; // Uncomment if you need forms functionality
import { CommonModule } from "@angular/common";
import { ApiUserService } from "../api_controllers/api_users"; // Adjust the import path as necessary
import { ApiLinkService } from "../api_controllers/api_links"; // Adjust the import path as necessary
import { LinkDTO } from "../utils/interface_links"; // Adjust the import path as necessary

@Component({
  selector: "app-welcome_page",
  standalone: true,
  imports: [FormsModule, CommonModule], // Uncomment if you need forms functionality
  templateUrl: "welcome_page.html",
  styleUrls: ["welcome_page.css"]
})

export class WelcomePageComponent {
    @Output() loginSuccess = new EventEmitter<string>();
    
    constructor(private apiUserService: ApiUserService, private apiLinkService: ApiLinkService) {
        /* Initialization logic can go here if needed */
        console.log("!Welcome_Page!", "WelcomePageComponent initialized.");
        this.apiLinkService.GetGuesLinks().subscribe({
            next: (response: LinkDTO[]) => {
                this.links = response; // Assuming the response is an array of Link objects
                console.log("!Welcome_Page!", "Guest links fetched successfully:", this.links);
            },
            error: (error: any) => {
                console.error("!Welcome_Page!", "Failed to fetch guest links:", error);
            }
        });
    }

    links: LinkDTO[] = [];

    loginState = false;
    registerState = false;

    login = "";
    password = "";
    
    Login() {
        this.apiUserService.Login(this.login, this.password).subscribe({
            next: (response: any) => {
                const token = response.token; // Assuming the response contains a token
                console.log("!Welcome_Page!", "Login successful:", "token:", token);
                this.loginSuccess.emit(token); // Emit the token to the parent component
                this.login = this.password = ""; // Clear the input fields
                this.loginState = false; // Hide login form after successful login
            },
            error: (error: any) => {
                console.error("!Welcome_Page!", "Login failed:", error);
            }
        });
    }
    Register() {
        this.apiUserService.Register(this.login, this.password);
        this.registerState = false; // Hide register form after registration
        this.loginState = true; // Optionally show login form after registration
    }  
}