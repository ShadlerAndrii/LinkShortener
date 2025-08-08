import { Injectable, Output, EventEmitter } from "@angular/core";
import { HttpClient } from "@angular/common/http";

@Injectable({ providedIn: 'root' })
export class ApiUserService {
    private readonly API_URL = "https://localhost:7211/UserData";
    
    token: string | null = null;
    @Output() tokenReceived = new EventEmitter<string>();

    constructor(private http: HttpClient) {}

    Login(login: string, password: string) {
        const formData = new FormData();
        formData.append("login", login);
        formData.append("password", password);

        return this.http.post(this.API_URL + "/authenticate", formData);
            
    }

    Register(login: string, password: string) {
        let role = "User"; // Default role, can be changed as needed
        const formData = new FormData();
        formData.append("login", login);
        formData.append("password", password);
        formData.append("role", role);

        this.http.post(this.API_URL, formData).subscribe({
            next: (response: any) => {
                console.log("!Api_Users", "Registration successful:", response);
            },
            error: (error: any) => {
                console.error("!Api_Users", "Registration failed:", error);
            }
        });
    }
}