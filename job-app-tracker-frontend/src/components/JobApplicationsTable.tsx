import {
  Table,
  TableBody,
  TableCell,
  TableContainer,
  TableHead,
  TableRow,
  Paper,
  Button,
  Stack,
  TablePagination,
  CircularProgress,
  Box,
} from "@mui/material";
import { JobApplication } from "../models/JobApplication";

interface Props {
  data: JobApplication[];
  onEdit: (row: JobApplication) => void;
  onDelete: (id: number) => Promise<void>;

  loading: boolean;
  pageNumber: number;
  pageSize: number;
  totalCount: number;
  totalPages: number;
  onPageChange: (newPage: number) => void;
  onPageSizeChange: (newSize: number) => void;
}

export const JobApplicationsTable = ({
  data,
  onEdit,
  onDelete,
  loading,
  pageNumber,
  pageSize,
  totalCount,
  onPageChange,
  onPageSizeChange,
}: Props) => {

  const handleChangePage = (_: unknown, newPage: number) => {
    onPageChange(newPage + 1);
  };

  const handleChangeRowsPerPage = (event: React.ChangeEvent<HTMLInputElement>) => {
    const newSize = parseInt(event.target.value, 10);
    onPageSizeChange(newSize);
    onPageChange(1);
  };

  return (
    <TableContainer component={Paper}>
      <Table>
        <TableHead>
          <TableRow sx={{ backgroundColor: "#1976d2", "& .MuiTableCell-root": { color: "#fff" } }}>
            <TableCell>Company Name</TableCell>
            <TableCell>Position</TableCell>
            <TableCell>Status</TableCell>
            <TableCell>Date Applied</TableCell>
            <TableCell>Actions</TableCell>
          </TableRow>
        </TableHead>
        <TableBody>
          {loading ? (
            <TableRow>
              <TableCell colSpan={5} align="center">
                <Box sx={{ display: 'flex', justifyContent: 'center', alignItems: 'center', py: 2 }}>
                  <CircularProgress size={24} />
                  <span style={{ marginLeft: 8 }}>Loading applications...</span>
                </Box>
              </TableCell>
            </TableRow>
          ) : data.length === 0 ? (
            <TableRow>
              <TableCell colSpan={5} align="center">
                No job applications found.
              </TableCell>
            </TableRow>
          ) : (
            data.map((app) => (
              <TableRow key={app.id}>
                <TableCell>{app.companyName}</TableCell>
                <TableCell>{app.position}</TableCell>
                <TableCell>{app.status}</TableCell>
                <TableCell>{new Date(app.dateApplied).toLocaleDateString()}</TableCell>
                <TableCell>
                  <Stack direction="row" spacing={1}>
                    <Button variant="outlined" size="small" onClick={() => onEdit(app)}>
                      Edit
                    </Button>
                    <Button
                      variant="outlined"
                      color="error"
                      size="small"
                      onClick={() => onDelete(app.id)}
                    >
                      Delete
                    </Button>
                  </Stack>
                </TableCell>
              </TableRow>
            ))
          )}
        </TableBody>
      </Table>

      <TablePagination
        component="div"
        count={totalCount}
        page={pageNumber - 1}
        onPageChange={handleChangePage}
        rowsPerPage={pageSize}
        onRowsPerPageChange={handleChangeRowsPerPage}
        rowsPerPageOptions={[5, 10, 20]}
      />
    </TableContainer>
  );
};