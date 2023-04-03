# **APIMe** Capstone Project

Welcome to the APIMe Capstone Project! This project helps students to understand how APIs work in a better way and provide to test all types of requests available in PostMan.


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
## Deployment

The instructor will deploy the application on an IIS server on the Conestoga College network. Students can then access the application using an API client such as Postman to test the APIMe solution.


## **Getting Started Guide**

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
    
    
## Available routes for testing in Postman
- (GET)    https://localhost:44433/routeApi/records/{tableName}
- (GET)    https://localhost:44433/routeApi/records/{tableName}/{id}
- (POST)   https://localhost:44433/routeApi/records/{tableName}
- (PUT)    https://localhost:44433/routeApi/records/{tableName}
- (PATCH)  https://localhost:44433/routeApi/records/{tableName}/{id}
- (DELETE) https://localhost:44433/routeApi/records/{tableName}/{id}

### Tables to use in Postman routes
- Products
- Customers
- Suppliers
- Payments
- Employees
- Inventories
- Categories
- Orders

### Example JSON header of Content-Type: application/json
```
{
    "id": 2,
    "orderNumber": "ORD0063",
    "customerName": "Sid Thompson",
    "totalAmount": 131.50,
    "orderDate": "2023-03-20 16:10:00.000"
} 
```


## License

This project is licensed under the **MIT License**. See the LICENSE file for more information.


