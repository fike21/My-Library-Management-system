NAME: FIKIRE TIBEBU    ID: DBUR/0737/13
  
  📚 My Library Management System

✨ Project Overview
The My Library Management System is a robust software solution designed to streamline and automate the essential operations of a library. It focuses on efficiently managing library resources, handling user interactions, and overseeing the borrowing and return processes, ultimately enhancing organization, security, and accessibility.

🚀 Features
Comprehensive Book Cataloging: Easily add, update, search, and manage your entire book collection. Store details like title, author, ISBN, publication year, total copies available, and currently available copies.
User Account Management: Register and maintain detailed user accounts, ensuring secure storage of personal information.
Secure Authentication: Utilizes strong password hashing and salting techniques to protect user credentials, safeguarding against common security threats.
Efficient Borrowing & Returns: Track every book issued to a user and its subsequent return. This includes recording issue dates, due dates, and actual return dates.
Real-time Availability: Instantly see which books are on the shelves and which are currently borrowed.
Intuitive Search & Filtering: Quickly locate any book or user with powerful search and filtering options.
🛠️ Technology Stack
Database: SQL Server
Database Management: SQL Server Management Studio (SSMS)
(ACTION REQUIRED: Replace this with your actual application's technology. E.g.,)
Backend: C# / .NET 8 (ASP.NET Core)
Frontend: HTML, CSS, JavaScript (React.js / Angular / Vue.js)
🏁 Getting Started
These instructions will help you get a copy of the My Library Management System up and running on your local machine for development and testing.

Prerequisites
Before you start, make sure you have the following installed:

SQL Server: Any edition (e.g., SQL Server Express for local development, Developer Edition, Standard Edition).
SQL Server Management Studio (SSMS): To manage and interact with your SQL Server database.
(ACTION REQUIRED: List your application's specific prerequisites here. E.g.,)
.NET 8 SDK (if using .NET)
Python 3.10+ and pip (if using Python)
Java JDK 17+ and Maven/Gradle (if using Java)
Node.js LTS and npm/yarn (if using Node.js)
Database Setup
Create the Database:
Open SSMS and connect to your SQL Server instance. Execute the following SQL command to create your database:

SQL

CREATE DATABASE MyLibraryDB;
GO
USE MyLibraryDB;
GO
Create Tables and Apply Schema:
Execute the following SQL script to create all the necessary tables and apply the correct schema, including the password hashing setup, TotalCopies in Books, and ReturnDate as DATE in IssueBooks.

SQL

-- Users Table: For managing library users with secure password storage
CREATE TABLE Users (
    UserID INT PRIMARY KEY IDENTITY(1,1),
    Username NVARCHAR(50) NOT NULL UNIQUE,
    PasswordHash VARCHAR(255) NOT NULL, -- Stores the securely hashed password (e.g., bcrypt output)
    Salt VARCHAR(255) NOT NULL,           -- Stores the unique salt used for hashing
    FullName NVARCHAR(100) NOT NULL,
    Email NVARCHAR(100) UNIQUE,
    PhoneNumber NVARCHAR(20),
    RegistrationDate DATETIME DEFAULT GETDATE(),
    -- Add other user-specific columns as needed
);

-- Books Table: For managing library book details
CREATE TABLE Books (
    BookID INT PRIMARY KEY IDENTITY(1,1),
    Title NVARCHAR(255) NOT NULL,
    Author NVARCHAR(100) NOT NULL,
    ISBN NVARCHAR(20) UNIQUE,
    PublicationYear INT,
    Publisher NVARCHAR(100),
    Genre NVARCHAR(50),
    TotalCopies INT NOT NULL DEFAULT 0,    -- Total number of physical copies owned by the library
    AvailableCopies INT NOT NULL DEFAULT 0, -- Copies currently not issued (should be managed by app logic)
    -- Add other book-specific columns as needed
);

-- IssueBooks Table: For tracking borrowing and return records
CREATE TABLE IssueBooks (
    IssueID INT PRIMARY KEY IDENTITY(1,1),
    BookID INT NOT NULL FOREIGN KEY REFERENCES Books(BookID),
    UserID INT NOT NULL FOREIGN KEY REFERENCES Users(UserID),
    IssueDate DATE NOT NULL DEFAULT GETDATE(), -- Date the book was issued
    DueDate DATE NOT NULL,                     -- Date the book is expected to be returned
    ReturnDate DATE,                           -- Actual date the book was returned (NULL if not yet returned)
    -- Add any other relevant fields for tracking issues (e.g., FineAmount)
);

-- Optional: Add indexes for performance on frequently queried columns
CREATE INDEX IX_Books_Title ON Books (Title);
CREATE INDEX IX_Users_Username ON Users (Username);
CREATE INDEX IX_IssueBooks_IssueDate ON IssueBooks (IssueDate);

-- Optional: Add CHECK constraints for data integrity
ALTER TABLE Books
ADD CONSTRAINT CK_Books_TotalCopies CHECK (TotalCopies >= 0);

ALTER TABLE Books
ADD CONSTRAINT CK_Books_AvailableCopies CHECK (AvailableCopies >= 0 AND AvailableCopies <= TotalCopies);
Application Setup
(ACTION REQUIRED: This section is highly dependent on your actual application's code. Provide detailed steps specific to your project.)

Clone the Repository:
Start by cloning the project to your local machine:

Bash

git clone https://github.com/your-username/my-library-management-system.git
cd my-library-management-system
Configure Database Connection:
Locate your application's configuration file (e.g., appsettings.json for .NET, .env for Laravel/Node.js, settings.py for Django, application.properties for Spring Boot). Update the database connection string to point to your newly created MyLibraryDB instance.

Example (appsettings.json for a .NET application):

JSON

{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=MyLibraryDB;Integrated Security=True;TrustServerCertificate=True;"
  },
  "Logging": { /* ... */ }
}
(Adjust Server, Database, and authentication method (e.g., User ID=sa;Password=yourStrongPassword;) as per your SQL Server setup.)

Install Dependencies:
Navigate to your project's root directory in the terminal and install the required packages/libraries:

Bash

# For .NET applications:
dotnet restore

# For Python applications (if using pip):
pip install -r requirements.txt

# For Node.js applications:
npm install
Run the Application:
Once dependencies are installed, you can launch the application:

Bash

# For .NET applications:
dotnet run

# For Python applications (e.g., Django):
python manage.py runserver

# For Node.js applications (e.g., Express app):
npm start
Your application should now be running! Typically, it will be accessible in your web browser at http://localhost:[port_number] (e.g., http://localhost:5000 or http://localhost:8000).
