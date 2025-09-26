# Job Application Tracker

## Overview
This is a **full-stack job application tracker** built using **ASP.NET Core Web API** for the backend and **React (Create React App) with TypeScript + Material UI** for the frontend.

## Features
- **List all job applications** in a paginated table
- **Add a new job application**
- **Update job application details**
- **Change application status**:
  - Applied
  - Interviewing
  - Offered
  - Rejected
- **Delete applications**
- **Client-side and server-side validation**
- **Swagger UI** for API documentation
- **Unit tests** for core business logic

## Tech Stack
### Backend
- **ASP.NET Core Web API (.NET 8)**
- **Entity Framework Core (Code First with In-Memory or SQLite)**
- **Repository & Service Layer Architecture**
- **Dependency Injection**
- **FluentValidation for validation**
- **Swagger UI for API documentation**
- **xUnit for unit testing**

### Frontend
- **React (Create React App) with TypeScript**
- **Material UI for UI Components**
- **Axios for API Calls**
- **Pagination & client-side validation**
- **Status management matching backend enum (Applied, Interviewing, Offered, Rejected)**

## Prerequisites
- **.NET 8 SDK** (for the backend)
- **Node.js** (for the frontend)
- **Git** (for version control)

## Setup Instructions

### 1 Clone the Repository
```sh
git clone https://github.com/paulchippy/job-application-tracker-project.git
cd job-application-tracker-project
2 Backend Setup (.NET 8 API)
Install dependencies:
sh
Copy code
cd JobApplicationTracker.API
dotnet restore
Run the API:
sh
Copy code
dotnet run
The API will be available at http://localhost:5000
Swagger UI will be available at http://localhost:5000/swagger/index.html

3 Frontend Setup (React CRA)
Install dependencies:
sh
Copy code
cd ../job-app-tracker-frontend
npm install
Run the frontend:
sh
Copy code
npm start
The frontend will be available at http://localhost:3000

Author
Chippy Paul