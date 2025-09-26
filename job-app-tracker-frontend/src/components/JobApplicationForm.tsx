import { useState, useEffect } from "react";
import { TextField, Button, Paper, Stack, Typography, MenuItem, Box } from "@mui/material";
import { JobApplication, ApplicationStatus } from "../models/JobApplication";

interface Props {
    onSubmit: (application: Omit<JobApplication, "id">) => void;
    initialData?: JobApplication;
}

const formatLocal = (date: Date): string => {
    const year = date.getFullYear();
    const month = (date.getMonth() + 1).toString().padStart(2, '0');
    const day = date.getDate().toString().padStart(2, '0');
    return `${year}-${month}-${day}`;
};

export const JobApplicationForm = ({ onSubmit, initialData }: Props) => {
    const today = formatLocal(new Date());

    const [companyName, setCompanyName] = useState("");
    const [position, setPosition] = useState("");
    const [status, setStatus] = useState<ApplicationStatus>(ApplicationStatus.Applied);
    const [dateApplied, setDateApplied] = useState(today);

    // Validation States
    const [companyNameError, setCompanyNameError] = useState("");
    const [positionError, setPositionError] = useState("");

    useEffect(() => {
        if (initialData) {
            setCompanyName(initialData.companyName);
            setPosition(initialData.position);
            setStatus(initialData.status);

            const date = initialData.dateApplied
                ? formatLocal(new Date(initialData.dateApplied))
                : today;

            setDateApplied(date);
        } else {
            setCompanyName("");
            setPosition("");
            setStatus(ApplicationStatus.Applied);
            setDateApplied(today);
        }
        // Clear errors when switching between edit/add mode
        setCompanyNameError("");
        setPositionError("");
    }, [initialData, today]);

    const validate = (): boolean => {
        let isValid = true;
        setCompanyNameError("");
        setPositionError("");

        // Company Name Validation (Required, Min Length 2)
        if (companyName.trim().length === 0) {
            setCompanyNameError("Company Name is required.");
            isValid = false;
        } else if (companyName.length < 2) {
            setCompanyNameError("Must be at least 2 characters.");
            isValid = false;
        }

        // Position Validation (Required, Min Length 2)
        if (position.trim().length === 0) {
            setPositionError("Position is required.");
            isValid = false;
        } else if (position.length < 2) {
            setPositionError("Must be at least 2 characters.");
            isValid = false;
        }

        return isValid;
    };

    const handleSubmit = (e: React.FormEvent) => {
        e.preventDefault();

        if (!validate()) {
            return;
        }

        onSubmit({ companyName, position, status, dateApplied });

        if (!initialData) {
            setCompanyName("");
            setPosition("");
            setStatus(ApplicationStatus.Applied);
            setDateApplied(today);
        }
    };

    return (
        <Paper
            elevation={3}
            sx={{
                p: 3,
                mb: 3,
                borderRadius: 2,
                border: initialData ? "2px solid #1976d2" : "1px solid #ccc",
                backgroundColor: initialData ? "#e3f2fd" : "white",
            }}
        >
            {initialData && (
                <Typography variant="subtitle1" color="primary" sx={{ mb: 2 }}>
                    Editing selected job application
                </Typography>
            )}
            <form onSubmit={handleSubmit}>
                <Stack spacing={2} direction="row" flexWrap="wrap" alignItems="center">
                    <TextField
                        label="Company Name"
                        value={companyName}
                        onChange={(e) => setCompanyName(e.target.value)}
                        required
                        size="small"
                        error={!!companyNameError}
                        helperText={companyNameError || `${companyName.length}/100`}
                        inputProps={{ maxLength: 100 }}
                        onBlur={() => validate()}
                    />
                    <TextField
                        label="Position"
                        value={position}
                        onChange={(e) => setPosition(e.target.value)}
                        required
                        size="small"
                        error={!!positionError}
                        helperText={positionError || `${position.length}/100`}
                        inputProps={{ maxLength: 100 }}
                        onBlur={() => validate()}
                    />
                    <TextField
                        label="Status"
                        select
                        value={status}
                        onChange={(e) => setStatus(e.target.value as ApplicationStatus)}
                        size="small"
                        helperText=" "
                    >
                        {Object.values(ApplicationStatus).map((s) => (
                            <MenuItem key={s} value={s}>
                                {s}
                            </MenuItem>
                        ))}
                    </TextField>
                    <TextField
                        label="Date Applied"
                        type="date"
                        value={dateApplied}
                        onChange={(e) => setDateApplied(e.target.value)}
                        required
                        inputProps={{ max: today }}
                        InputLabelProps={{
                            shrink: true,
                        }}
                        size="small"
                        helperText=" "
                    />
                    <Box sx={{ alignSelf: "normal" }}>
                        <Button type="submit" variant="contained" color="primary">
                            {initialData ? "Update" : "Add"}
                        </Button>
                    </Box>
                </Stack>
            </form>
        </Paper>
    );
};