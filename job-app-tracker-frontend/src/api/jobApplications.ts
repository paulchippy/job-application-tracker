import axios, { AxiosError } from "axios";
import { JobApplication } from "../models/JobApplication";

const API_URL = "https://localhost:5227/api/JobApplications";


export interface PaginatedResponse<T> {
    data: T[];
    pageNumber: number;
    pageSize: number;
    totalPages: number;
    totalCount: number;
}

interface PaginationParams {
    pageNumber: number;
    pageSize: number;
}

export interface ApiError {
    message: string;
    status: number;
    details?: any; // Optional: for holding validation errors or complex details
}

// Type guard to check if an unknown error is an AxiosError
export const isAxiosError = (error: any): error is AxiosError => {
    return (error as AxiosError).isAxiosError !== undefined;
};

export const getJobApplications = async (
    params: PaginationParams
): Promise<PaginatedResponse<JobApplication>> => {
    try {
        // Axios automatically converts 'params' to the query string: ?pageNumber=X&pageSize=Y
        const response = await axios.get<PaginatedResponse<JobApplication>>(API_URL, {
            params: {
                pageNumber: params.pageNumber,
                pageSize: params.pageSize,
            },
        });
        
        return response.data;
    } catch (error) {
        console.error("Failed to fetch job applications:", error);
        throw error;
    }
};

export const createJobApplication = async (
    application: Omit<JobApplication, "id">
): Promise<JobApplication> => {
    try {
        const response = await axios.post<JobApplication>(API_URL, application);
        return response.data;
    } catch (error) {
        console.error("Failed to create job application:", error);
        throw error;
    }
};

export const updateJobApplication = async (
    id: number,
    application: JobApplication
): Promise<JobApplication> => {
    try {
        const response = await axios.put<JobApplication>(`${API_URL}/${id}`, application);
        return response.data;
    } catch (error) {
        console.error(`Failed to update job application with id ${id}:`, error);
        throw error;
    }
};

export const deleteJobApplication = async (id: number): Promise<void> => {
    try {
        await axios.delete(`${API_URL}/${id}`);
    } catch (error) {
        console.error(`Failed to delete job application with id ${id}:`, error);
        throw error;
    }
};