import { Component } from "@angular/core";
import { FormsModule } from "@angular/forms"; // Uncomment if you need forms functionality
import { CommonModule } from "@angular/common";
import { ApiLinkService } from "../api_controllers/api_links"; // Adjust the import path as necessary
import { Link } from "../utils/interface_links"; // Adjust the import path as necessary

@Component({
  selector: "app-user_page",
  standalone: true,
  imports: [FormsModule, CommonModule], // Uncomment if you need forms functionality
  templateUrl: "user_page.html",
  styleUrls: ["user_page.css"]
})

export class UserPageComponent {
    constructor(private apiLinkService: ApiLinkService) {
        // Initialization logic can go here if needed
        this.init();
    }

    links: Link[] = [];

    isCreateLinkState = false;

    newLongLink = "";
    newShortLink = "";

    init() {
        console.log("!User_Page!", "UserPageComponent initialized.");
        this.apiLinkService.GetUserLinks().subscribe({
            next: (response: Link[]) => {
                this.links = response; // Assuming the response is an array of Link objects
                console.log("!User_Page!", "User links fetched successfully:", this.links);
            },
            error: (error: any) => {
                console.error("!User_Page!", "Failed to fetch user links:", error);
            }
        });
    }

    CreateNewLink(longLink: string) {
        console.log("!User_Page!", "Creating a new shortened link...");
        // Logic to create a new shortened link goes here
        this.apiLinkService.CreateLink(longLink).subscribe({
            next: (response: any) => {
                console.log("!User_Page!", "New link created successfully:", response);
                this.newShortLink = response; // Assuming the response contains the new short link
                this.init(); // Refresh the list of links after creation
            },
            error: (error: any) => {
                console.error("!User_Page!", "Failed to create new link:", error);
            }
        });
    }

    DeleteLink(linkId: number) {
        console.log("!User_Page!", `Deleting link...`);
        this.apiLinkService.DeleteLink(linkId).subscribe({
            next: () => {
                console.log("!User_Page!", `Link with ID: ${linkId} deleted successfully.`);
                this.init(); // Refresh the list of links after deletion
            },
            error: (error: any) => {
                console.error("!User_Page!", `Failed to delete link with ID: ${linkId}`, error);
            }
        });
    }

    onShortLinkCreatedClick(){
        navigator.clipboard.writeText(this.newShortLink)
        .then(() => {
            alert("Short link copied to clipboard");
        });

    }
}