    export enum ApplicationStatus {
        Applied = "Applied",
        Interviewing = "Interviewing",
        Offered = "Offered",
        Rejected = "Rejected"
    }

    export interface JobApplication {
        id: number;
        companyName: string;
        position: string;
        status: ApplicationStatus;
        dateApplied: string; // ISO date string
    }
