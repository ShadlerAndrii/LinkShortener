import { Component, Input } from "@angular/core";
import { FormsModule } from "@angular/forms";
import { CommonModule } from "@angular/common";
import { ApiAboutsService } from "../api_controllers/api_abouts"; // Adjust the import path as necessary

@Component({
  selector: "app-about_page",
  standalone: true,
  imports: [FormsModule, CommonModule],
  templateUrl: "about_page.html",
  styleUrls: ["about_page.css"]
})

export class AboutPageComponent {
    constructor(private apiAboutsService: ApiAboutsService) {
        // Initialization logic can go here if needed
        console.log("!About_Page!", "AboutPageComponent initialized.");
        this.getAboutInfo();
    }

    AboutText = "";
    @Input() isAdmin: boolean = false; // Input to check if the user is an admin

    // You can add methods to fetch or display information about the application
    getAboutInfo() {
        console.log("!About_Page!", "Fetching application information...");
        // Example: Fetching some data from the API
        this.apiAboutsService.GetAbout().subscribe({
            next: (response) => {
                console.log("!About_Page!", "Application information fetched successfully:", response);
                this.AboutText = response; // Assuming the response contains the information you want to display
            },
            error: (error) => {
                console.error("!About_Page!", "Failed to fetch application information:", error);
            }
        });
    }

    changeAboutText(newText: string) {
        this.apiAboutsService.ChangeAbout(newText).subscribe({
            next: (response) => {
                console.log("!About_Page!", "About text changed successfully:", response);
                this.getAboutInfo(); // Refresh the about text after changing it
            },
            error: (error) => {
                console.error("!About_Page!", "Failed to change about text:", error);
            }
        });        
    }
}