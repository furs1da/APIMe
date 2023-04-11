# **APIMe** Capstone Project

Welcome to the APIMe Capstone Project! This project is a web-based application that enables users to test and interact with APIMe platform.


## Project Overview

APIMe can be deployed on an IIS server and accessed by multiple users at the same time. Using an API client such as Postman, users can make requests and examine responses with their codes for different routes. Users can also download and deploy the software on their own laptops, giving them more flexibility for testing and experimenting. The project was built with Angular on front-end and .NET 6 on back-end.



## Purpose

The purpose of the APIMe Capstone Project is to provide a user-friendly application that enables users to learn and test APIs in a simple and efficient manner.


## Installation

To install the application, follow these steps:

    1. Clone the repository to your local machine.
    2. Install the necessary dependencies using 'npm install' inside ClientApp folder.
    3. Run 'ng build' command to generate needed packages for application to work.
    4. Run application from Visual Studio
    5. Browser window will pop up, prompting you to login.
## Features

- Easy to install and deploy on IIS server
- Application is self-contained within itself and consists of a front-end UI, business logic and database.
- [ASP.NET Core Identity Authentication](https://learn.microsoft.com/en-us/aspnet/core/security/authentication/identity?view=aspnetcore-7.0&tabs=visual-studio)
- Supports the following route codes:
    -  200 (OK)
    -  201 (Created)
    -  400 (Bad Request)
    -  401 (Unauthorized)
    -  403 (Forbidden)
    -  404 (Not Found)
    -  500 (Internal Server Error)
- Developed with [Angular](https://angular.io/docs), [.NET 6](https://learn.microsoft.com/en-us/dotnet/fundamentals/), and [SQL Server](https://learn.microsoft.com/en-us/sql/ssms/download-sql-server-management-studio-ssms?view=sql-server-ver16)
- Designed according to [AODA](https://aoda.ca/) standards
- The database features 8 different tables with more than 1,000 records in total
- Built with Object-Oriented Programming standards in mind
- Built in user interface version of Postman
- Route Logs of API requests sent to the system
## Deployment

The instructor will deploy the application on an IIS server on the Conestoga College network. Students can then access the application using an API client such as Postman to test the APIMe solution.


## **Getting Started Guide**

### Prerequisites
    1. .NET 6 SDK
    2. SQL Server
    3. IIS web server
    4. Angular 15
    5. Postman

### Authorization
    1. Register your student account
    2. Confirm your email
    3. Login to APIMe

### Test the routes using APIMe
    1. Go to the routes page
    2. Observe the various routes offered by APIMe
    3. Click 'Test' on any of the routes
    4. Dialog window will pop up with the response details retrieved from the server

### Test the routes using Postman
    1. Go to the routes page
    2. Observe the various routes offered by APIMe
    3. Retrieve the route details from the description of the route
    4. If the route is POST/PUT/PATCH, the header of Content-Type: application/json will be required
    5. Fill out the JSON header based on the routes requirements that can be found in the route's dialog window

### Test the routes using APIMe's Postman
    1. Go to the "Postman" page
    2. Choose the request type
    3. Type in endpoint or use magic stick to automatically complete the endpoint
    4. Add headers to the request
    5. Fill out the "Request Body" if needed
    6. Send the request and observe the response

### Accessing Routes Log
    1. Go to the "Routes Log" page
    2. Filter by name
    3. Specify the HTTP method
    4. Filter by table name
    5. Specify the date
    6. Press "Export to Excel"

### Accessing Sections and Students
    1. Go to "Sections"
    2. Press "Add Section"
    3. Press "Edit" or "Delete" to modify a section
    4. Go to "Students
    5. Specify the name of the student
    6. Filter by Section

### Managing profile data
    1. Navigate to "Profile" section in the top-right corner
    2. Edit email
    3. Edit First Name
    4. Edit Last Name
    5. Click "Save"


## License

This project is licensed under the **MIT License**. See the LICENSE file for more information.


