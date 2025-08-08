import { Injectable } from "@angular/core";
import { HttpClient, HttpHeaders } from "@angular/common/http";

@Injectable({ providedIn: 'root' })
export class ApiAboutsService {
    private readonly API_URL = "https://localhost:7211/AboutData";

    constructor(private http: HttpClient) {}

    GetAbout(){
        return this.http.get(this.API_URL, { responseType: 'text' });
    }

    ChangeAbout(aboutText: string) {
        const token = localStorage.getItem('token');
        const headers = new HttpHeaders().set("Authorization", `Bearer ${token}`);

        const formData = new FormData();
        formData.append("aboutText", aboutText);

        return this.http.put(this.API_URL, formData, { headers });
    }
}