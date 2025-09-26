import { useEffect, useState, useCallback } from "react";
import { Typography, Paper, Alert } from "@mui/material";
import { JobApplication } from "../models/JobApplication";
import {
    getJobApplications,
    createJobApplication,
    updateJobApplication,
    deleteJobApplication,
    PaginatedResponse,
    isAxiosError,
} from "../api/jobApplications";
import { JobApplicationForm } from "../components/JobApplicationForm";
import { JobApplicationsTable } from "../components/JobApplicationsTable";

export const Home: React.FC = () => {
    const [data, setData] = useState<JobApplication[]>([]);

    const [pageNumber, setPageNumber] = useState(1);
    const [pageSize, setPageSize] = useState(10);
    const [totalCount, setTotalCount] = useState(0);
    const [totalPages, setTotalPages] = useState(1);
    const [loading, setLoading] = useState(true);

    const [editing, setEditing] = useState<JobApplication | null>(null);
    const [error, setError] = useState<string | null>(null);

    // Centralized error message generation utility
    const getErrorMessage = (err: unknown, operation: 'fetch' | 'save' | 'delete'): string => {
        let baseMessage = `Failed to ${operation} job application(s).`;

        if (isAxiosError(err)) {
            if (err.code === "ERR_NETWORK") {
                return `Could not connect to the server to ${operation} data. Please check your network or API status.`;
            } 
            
            if (err.response) {
                const status = err.response.status;
                const responseData = err.response.data;

                // Handle 400 validation specifically for the 'save' operation
                if (status === 400 && operation === 'save' && typeof responseData === 'string' && responseData.includes("DateApplied cannot be in the future")) {
                    return "Validation failed: Date Applied cannot be in the future (server check).";
                }
                
                // Handle 404 specifically for 'delete' operation
                if (status === 404 && operation === 'delete') {
                    return "Failed to delete: Application not found on server.";
                }
                
                // Handle 404 for fetch/save operations if the status is available
                if (status === 404) {
                    return `Resource not found: Server returned status ${status}.`;
                }

                // General API errors
                return `${baseMessage}: Server returned status ${status}.`;
            }
        }
        
        // Fallback for non-Axios or unhandled errors
        return baseMessage; 
    };

    const fetchData = useCallback(async () => {
        setError(null);
        setLoading(true);
        try {
            const response: PaginatedResponse<JobApplication> = await getJobApplications({
                pageNumber,
                pageSize,
            });

            setData(response.data);
            setTotalCount(response.totalCount);
            setTotalPages(response.totalPages);

        } catch (err: unknown) {
            console.error("Fetch Error:", err);
            setError(getErrorMessage(err, 'fetch'));
        } finally {
            setLoading(false);
        }
    }, [pageNumber, pageSize]);

    useEffect(() => {
        fetchData();
    }, [fetchData]);

    const handleSubmit = async (application: Omit<JobApplication, "id">) => {
        setError(null);
        try {
            if (editing) {
                await updateJobApplication(editing.id, { ...application, id: editing.id });
                setEditing(null);
            } else {
                await createJobApplication(application);
            }

            if (pageNumber !== 1) {
                setPageNumber(1);
            } else {
                await fetchData();
            }

        } catch (err: unknown) {
            console.error("Submit Error:", err);
            setError(getErrorMessage(err, 'save'));
        }
    };

    const handleDelete = async (id: number) => {
        setError(null);
        try {
            await deleteJobApplication(id);
            
            if (data.length === 1 && pageNumber > 1) {
                setPageNumber(pageNumber - 1);
            } else {
                fetchData();
            }
        } catch (err: unknown) {
            console.error("Delete Error:", err);
            setError(getErrorMessage(err, 'delete'));
        }
    };

    return (
        <div style={{ padding: 20 }}>
            <Typography variant="h4" gutterBottom>
                Job Applications Tracker
            </Typography>

            {error && <Alert severity="error" sx={{ mb: 2 }}>{error}</Alert>}

            <Paper elevation={3} sx={{ p: 2, mb: 3, borderRadius: 2 }}>
                <JobApplicationForm onSubmit={handleSubmit} initialData={editing ?? undefined} />
            </Paper>

            <JobApplicationsTable
                data={data}
                onEdit={(row) => setEditing(row)}
                onDelete={handleDelete}
                loading={loading}
                pageNumber={pageNumber}
                pageSize={pageSize}
                totalCount={totalCount}
                totalPages={totalPages}
                onPageChange={(newPage: number) => setPageNumber(newPage)}
                onPageSizeChange={(newSize: number) => setPageSize(newSize)}
            />
        </div>
    );
};
