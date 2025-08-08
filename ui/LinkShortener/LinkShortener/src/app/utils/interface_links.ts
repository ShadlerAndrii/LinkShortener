export interface Link {
    LinkId: number;
    LongLink: string;
    ShortLink: string;
    CreationDate: Date;
    UserId: number;
    UserLogin: string;
}

export interface LinkDTO {
    LongLink: string;
    ShortLink: string;
}