-- SQLite Database Script for MyLibrary Application

-- Create Users table for authentication
CREATE TABLE Users (
    UserID INTEGER PRIMARY KEY AUTOINCREMENT,
    Username TEXT NOT NULL UNIQUE,
    Password TEXT NOT NULL 
);

-- Create Books table
CREATE TABLE Books (
    BookID INTEGER PRIMARY KEY AUTOINCREMENT,
    Title TEXT NOT NULL,
    Author TEXT NOT NULL,
    PublicationYear INTEGER NOT NULL,
    AvailableCopies INTEGER NOT NULL DEFAULT 0,
    TotalCopies INTEGER NOT NULL DEFAULT 0
);

-- Create Borrowers table
CREATE TABLE Borrowers (
    BorrowerID INTEGER PRIMARY KEY AUTOINCREMENT,
    Name TEXT NOT NULL,
    Email TEXT,
    Phone TEXT
);

-- Create IssuedBooks table to track borrowings
CREATE TABLE IssuedBooks (
    IssueID INTEGER PRIMARY KEY AUTOINCREMENT,
    BookID INTEGER NOT NULL,
    BorrowerID INTEGER NOT NULL,
    IssueDate TEXT NOT NULL, 
    DueDate TEXT NOT NULL,   
    ReturnDate TEXT,         
    FOREIGN KEY (BookID) REFERENCES Books(BookID),
    FOREIGN KEY (BorrowerID) REFERENCES Borrowers(BorrowerID)
);

-- Seed initial data for Users
INSERT INTO Users (Username, Password) VALUES ('admin', 'password123');
INSERT INTO Users (Username, Password) VALUES ('user', 'userpass');

-- Seed initial data for Books
INSERT INTO Books (Title, Author, PublicationYear, AvailableCopies, TotalCopies) VALUES
('The Hitchhiker''s Guide to the Galaxy', 'Douglas Adams', 1979, 3, 5),
('1984', 'George Orwell', 1949, 2, 4),
('Pride and Prejudice', 'Jane Austen', 1813, 1, 3),
('To Kill a Mockingbird', 'Harper Lee', 1960, 4, 4),
('The Great Gatsby', 'F. Scott Fitzgerald', 1925, 0, 2); 

-- Seed initial data for Borrowers
INSERT INTO Borrowers (Name, Email, Phone) VALUES
('Alice Smith', 'alice.s@example.com', '111-222-3333'),
('Bob Johnson', 'bob.j@example.com', '444-555-6666'),
('Charlie Brown', 'charlie.b@example.com', '777-888-9999');

-- Seed initial data for IssuedBooks (example: some books are currently issued)
INSERT INTO IssuedBooks (BookID, BorrowerID, IssueDate, DueDate, ReturnDate) VALUES
(1, 1, '2025-05-20', '2025-06-03', NULL), 
(2, 2, '2025-05-15', '2025-05-29', NULL), 
(5, 3, '2025-05-25', '2025-06-08', NULL); 
