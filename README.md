# CSharp-Backend

This repository contains a C# and ASP.NET Core application utilizing Entity Framework for database operations. This project demonstrates a basic implementation of user authentication with JWT, password hashing with BCrypt, and includes setup instructions for a PostgreSQL database.

## Table of Contents

- [Introduction](#introduction)
- [Features](#features)
- [Prerequisites](#prerequisites)
- [Installation](#installation)
- [Configuration](#configuration)
- [Database Migration](#database-migration)
- [Running the Application](#running-the-application)
- [API Endpoints](#api-endpoints)

## Introduction

CSharp-Backend is a project showcasing the development of a backend application using C# and ASP.NET Core. The application includes user authentication, JWT integration, and database operations using Entity Framework with PostgreSQL.

## Features

- User registration and authentication
- JWT-based authentication
- Password hashing with BCrypt
- Database operations with Entity Framework Core
- PostgreSQL database integration
- API documentation with Postman

## Prerequisites

Before you begin, ensure you have the following installed:

- .NET SDK 8.0
- PostgreSQL
- Visual Studio Code (or any other code editor)

## Installation

1. **Clone the repository:**
   ```bash
   git clone https://github.com/NuelUzoma/CSharp-Backend.git
   cd CSharp-Backend
   ```

2. **Install .NET SDK 8.0:**
   Follow the instructions from the [official .NET download page for debian](https://learn.microsoft.com/en-gb/dotnet/core/install/linux-debian).

   For Linux:
   ```bash
   wget https://packages.microsoft.com/config/debian/12/packages-microsoft-prod.deb -O packages-microsoft-prod.deb
   sudo dpkg -i packages-microsoft-prod.deb
   rm packages-microsoft-prod.deb

   sudo apt-get update && \
   sudo apt-get install -y dotnet-sdk-8.0

   sudo apt-get update && \
   sudo apt-get install -y aspnetcore-runtime-8.0

   dotnet --version
   ```

3. **Install PostgreSQL:**
   Follow the instructions from the [official PostgreSQL download page](https://www.postgresql.org/download/).

4. **Restore NuGet packages:**
   ```bash
   dotnet restore
   ```

## Configuration

1. **Set up the database connection string:**
   Update the `appsettings.json` file with your PostgreSQL connection string:
   ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": "Host=localhost;Database=first_backend;Username=your_username;Password=your_password"
     },
     "Jwt": {
       "Key": "your_256_bit_key",
       "Issuer": "http://localhost:5145",
       "Audience": "http://localhost:5145"
     },
     "Logging": {
       "LogLevel": {
         "Default": "Information",
         "Microsoft.AspNetCore": "Warning"
       }
     },
     "AllowedHosts": "*"
   }
   ```

2. **Generate a 256-bit secret key for JWT:**
   ```bash
   openssl rand -base64 32
   ```

## Database Migration

1. **Create and apply migrations:**
   ```bash
   dotnet dotnet-ef migrations add InitialCreate
   dotnet dotnet-ef database update
   ```

## Running the Application

1. **Run the application:**
   ```bash
   dotnet run
   ```

## API Endpoints

### User Registration

- **Endpoint:** `POST /api/user/signup`
- **Description:** Registers a new user.
- **Request Body:**
  ```json
  {
    "name": "JohnDoe",
    "email": "johndoe@example.com",
    "password": "your_password"
  }
  ```
- **Response:**
  ```json
  {
    "id": 1,
    "name": "JohnDoe",
    "email": "johndoe@example.com"
  }
  ```

### User Authentication

- **Endpoint:** `POST /api/user/login`
- **Description:** Authenticates a user and returns a JWT.
- **Request Body:**
  ```json
  {
    "email": "johndoe@example.com",
    "password": "your_password"
  }
  ```
- **Response:**
  ```json
  {
    "token": "your_jwt_token"
  }
  ```

### Get Users

- **Endpoint:** `GET /api/user`
- **Description:** Retrieves users.
- **Response:**
  ```json
    [
        {
            "id": 1,
            "name": "Emmanuel",
            "email": "emc14@gmail.com"
        },
        {
            "id": 2,
            "name": "Nuel",
            "email": "user2@gmail.com"
        },
        {
            "id": 3,
            "name": "Uzo",
            "email": "user3@gmail.com"
        },
        {
            "id": 4,
            "name": "Uzoma",
            "email": "user4@gmail.com"
        },
        {
            "id": 5,
            "name": "Uzomachukwu",
            "email": "user5@gmail.com"
        },
        {
            "id": 6,
            "name": "Uzomachukws Chidera",
            "email": "user6@gmail.com"
        },
        {
            "id": 7,
            "name": "Chidera KSI",
            "email": "user7@gmail.com"
        }
    ]
  ```

### Get User by ID

- **Endpoint:** `GET /api/user/{id}`
- **Description:** Retrieves a user by ID.
- **Response:**
  ```json
    {
        "id": 4,
        "name": "Uzoma",
        "email": "user4@gmail.com"
    }
  ```

### Delete User by ID

- **Endpoint:** `DELETE /api/user/{id}`
- **Description:** Deletes a user by ID.
- **Response:**
  ```json
    {
         "message": "User don comot from the application"
    }
  ```

#### API Full Documentation collection is available on Postman as well, awaiting publishing.

---