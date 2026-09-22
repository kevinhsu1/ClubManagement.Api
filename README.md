📌 Overview
ClubManagement.Api is an ASP.NET Core Web API that manages club member data, including active membership status, sorting, and pagination.
The project demonstrates clean architecture, repository patterns, ADO.NET data access, input validation, and unit testing with NUnit + Moq.

This project is designed for environments where ADO.NET, SQL Server, and stored procedures are preferred over Entity Framework.

📁 Project Structure
Code
ClubManagement.Api/
│
├── Controllers/
│   └── MembersController.cs
│
├── Models/
│   └── ActiveMemberDto.cs
│
├── Repositories/
│   ├── IMemberRepository.cs
│   └── MemberRepository.cs   (ADO.NET implementation)
│
├── appsettings.json
└── Program.cs
Test Project
Code
ClubManagement.Api.Tests/
│
└── MembersControllerTests.cs
🧩 Technologies Used
ASP.NET Core Web API

ADO.NET (SqlConnection, SqlCommand, SqlDataReader)

SQL Server

Dependency Injection

NUnit (unit testing)

Moq (mocking repository dependencies)

🎯 Features
✔ Get Active Members
Supports sorting by multiple columns

Supports optional pagination

Uses ADO.NET for SQL queries

Returns DTOs (no EF entities)

Includes input validation

✔ Clean Repository Pattern
Controller does not contain SQL

Repository handles all data access

Interface allows mocking for tests

✔ Unit Tests
NUnit test project

Moq for mocking IMemberRepository

Tests for:

Successful responses

Validation failures

Pagination behavior

🔍 API Endpoint
GET /api/members/active
Query Parameters
Parameter	Type	Default	Description
sortColumn	string	LastName	Column to sort by
pageNumber	int?	null	Page number
pageSize	int?	null	Page size


Valid Sort Columns
FirstName

LastName

Email

MembershipType

HomeClub

JoinDate

🛡 Input Validation
The controller validates:

Invalid sort columns

Negative or zero page numbers

Negative or zero page sizes

Missing pageSize when pageNumber is provided

Missing pageNumber when pageSize is provided

Invalid input returns:

Code
400 Bad Request
🗄 Data Access (ADO.NET)
The repository uses:

SqlConnection

SqlCommand

Parameterized SQL (prevents SQL injection)

Manual mapping to DTOs

This approach is ideal for environments requiring:

High SQL control

Stored procedure compatibility

Legacy system integration

🧪 Unit Testing
The test project uses:

NUnit for test framework

Moq for mocking repository dependencies

Test cases include:

Valid requests returning 200 OK

Invalid sort column → 400 Bad Request

Invalid pagination → 400 Bad Request

Pagination parameters passed correctly

🚀 How to Run
Clone the repository

Update appsettings.json with your SQL Server connection string

Run the API using Visual Studio or dotnet run

Use Postman, Swagger, or browser to call:

Code
GET /api/members/active
