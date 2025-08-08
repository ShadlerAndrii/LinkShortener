import { Injectable } from "@angular/core";
import { HttpClient, HttpHeaders } from "@angular/common/http";
import { LinkDTO, Link } from "../utils/interface_links"; // Adjust the import path as necessary

@Injectable({ providedIn: 'root' })
export class ApiLinkService {
    private readonly API_URL = "https://localhost:7211/LinkData";
    
    constructor(private http: HttpClient) {}

    GetGuesLinks() {
        const guestRequest = this.API_URL.substring(0, this.API_URL.lastIndexOf('/'));
        return this.http.get<LinkDTO[]>(guestRequest);
    }

    GetUserLinks() {
        const token = localStorage.getItem('token');
        const headers = new HttpHeaders().set("Authorization", `Bearer ${token}`);

        return this.http.get<Link[]>(this.API_URL, { headers });
    }

    CreateLink(longLink: string) {
        const token = localStorage.getItem('token');
        const headers = new HttpHeaders().set("Authorization", `Bearer ${token}`)

        const formData = new FormData();
        formData.append("longLink", longLink);

        return this.http.post(this.API_URL, formData, { headers, responseType: 'text' });
    }

    DeleteLink(linkId: number) {
        const token = localStorage.getItem('token');
        const headers = new HttpHeaders().set("Authorization", `Bearer ${token}`);

        return this.http.delete(`${this.API_URL}/${linkId}`, { headers });
    }
}