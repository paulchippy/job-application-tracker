# Job Application Tracker

## Overview
This project is a complete job application tracking system built as a full-stack monorepo. The backend is a RESTful API developed with ASP.NET Core and Entity framework core, and the frontend is a modern client application built with React and TypeScript (CRA), styled using Material UI.

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
- **Entity Framework Core Code First with In-Memory Database**
- **Repository & Dependency Injection Architecture**
- **Dependency Injection**
- **FluentValidation for validation**
- **Swagger UI for API documentation**
- **xUnit + moq for unit testing**

### Frontend
- **React (Create React App) with TypeScript**
- **Material UI for UI Components**
- **Axios for API Calls**
- **Pagination & client-side validation**

## Prerequisites
- **.NET 8 SDK** (for the backend)
- **Node.js** (for the frontend)
- **Git** (for version control)

## Setup Instructions

### 1 Clone the Repository
```sh

git clone https://github.com/paulchippy/job-application-tracker.git
cd job-application-tracker
```

### 2 Backend Setup (.NET 8 API)
#### Install dependencies:
```sh
cd JobApplicationTracker.API
```
```sh
dotnet restore
dotnet build
```

#### Run the API:
```sh
dotnet run
```
The API will be available at **http://localhost:5227** & Swagger UI will be accessible  at **http://localhost:5227/swagger/index.html**.


### 3 Frontend Setup (React with Typescript)
#### Install dependencies:
```sh
cd ../job-app-tracker-frontend
```
```sh
npm install
```
#### Run the frontend:
```sh
npm run start
```
The frontend will be available at **http://localhost:3000**.

---

Author
Chippy Paul