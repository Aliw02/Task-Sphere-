use TaskSphere;

-- CREATE DATABASE TaskSphere;

--------------------------------------------------
-- Schema Definitions with Enhancements & Soft Delete Support
--------------------------------------------------

---- Person Table: Stores personal details. Added IsActive column for soft delete.
--CREATE TABLE Person
--(
--    ID INT IDENTITY(1,1) NOT NULL,
--    FullName NVARCHAR(100) NOT NULL,
--    BirthDate DATE NOT NULL CHECK (BirthDate <= GETDATE()),  -- BirthDate cannot be in the future
--    Address NVARCHAR(200) NOT NULL,                           -- extended length for detailed addresses
--    Email NVARCHAR(100) NOT NULL UNIQUE,                      -- enforce unique email addresses
--    Phone NVARCHAR(20) NOT NULL,                              -- adjusted length for phone numbers
--    IsActive BIT NOT NULL DEFAULT 1,                          -- 1 = active, 0 = soft-deleted
--    CreatedDate DATETIME NOT NULL DEFAULT GETDATE(),          -- record creation timestamp
--    ModifiedDate DATETIME NOT NULL DEFAULT GETDATE(),         -- last modification timestamp
--    PRIMARY KEY (ID)
--);

---- EmployeeTitles Table: Stores distinct job titles with optional descriptions.
--CREATE TABLE EmployeeTitles
--(
--    ID INT IDENTITY(1,1) NOT NULL,
--    Title NVARCHAR(50) NOT NULL,
--    Description NVARCHAR(200) NULL,                         -- optional additional context
--    CreatedDate DATETIME NOT NULL DEFAULT GETDATE(),
--    ModifiedDate DATETIME NOT NULL DEFAULT GETDATE(),
--    PRIMARY KEY (ID),
--    UNIQUE (Title)
--);

---- Employee Table: Connects a Person to an EmployeeTitle.  
---- Replaced computed IsActive column with a regular IsActive flag for soft delete.
--CREATE TABLE Employee
--(
--    ID INT IDENTITY(1,1) NOT NULL,
--    PersonID INT NOT NULL,
--    TitleID INT NOT NULL,
--	Salary SMALLMONEY NOT NULL,
--    StartDate DATE NOT NULL,
--    EndDate DATE NOT NULL,                                    -- EndDate is now required
--    IsActive BIT NOT NULL DEFAULT 1,                          -- 1 = active, 0 = soft-deleted
--    CreatedDate DATETIME NOT NULL DEFAULT GETDATE(),
--    ModifiedDate DATETIME NOT NULL DEFAULT GETDATE(),
--    PRIMARY KEY (ID),
--    FOREIGN KEY (PersonID) REFERENCES Person(ID),
--    FOREIGN KEY (TitleID) REFERENCES EmployeeTitles(ID),
--    CHECK (StartDate <= EndDate)                              -- ensures StartDate is not after EndDate
--);



---- Departments Table: Stores Departments Of The Company.
--CREATE TABLE Departments
--(
--    ID INT IDENTITY(1,1) PRIMARY KEY NOT NULL,
--	Department NVARCHAR(50) NOT NULL Unique
--);


---- Task Table: Stores Tasks details. 
--CREATE TABLE Tasks (
--    ID INT IDENTITY(1,1) PRIMARY KEY,       -- Unique Task ID
--    ListedBy INT NOT NULL,                  -- Employee who listed the task
--    CompletedBy INT NULL,                    -- Employee who completed the task (NULL if not completed)
--    StatusID INT NOT NULL,                   -- Task status from TasksStatus table
--    FOREIGN KEY (ListedBy) REFERENCES Employee(ID),  -- Reference to Employee table
--    FOREIGN KEY (CompletedBy) REFERENCES Employee(ID), -- Reference to Employee table
--    FOREIGN KEY (StatusID) REFERENCES TasksStatus(ID) -- Reference to TasksStatus table
--);


---- TaskStatus Table: Stores Tasks details. 
--CREATE TABLE TasksStatus (
--    ID INT IDENTITY(1,1) PRIMARY KEY,  -- Unique status ID
--    Status NVARCHAR(50) NOT NULL   -- Status name (e.g., Pending, In Progress, Completed)
--);


--INSERT INTO TasksStatus (Status) 
--VALUES 
--    ('Pending'),
--    ('In Progress'),
--    ('Completed'),
--    ('Cancelled');


--------------------------------------------------
-- Audit Tables for Soft-Deleted Records
--------------------------------------------------

---- DeletedPersons: Stores details of soft-deleted Person records.
--CREATE TABLE DeletedPersons
--(
--    DeletedID INT IDENTITY(1,1) PRIMARY KEY,
--    PersonID INT NOT NULL,
--    FullName NVARCHAR(100) NOT NULL,
--    BirthDate DATE NOT NULL,
--    Address NVARCHAR(200) NOT NULL,
--    Email NVARCHAR(100) NOT NULL,
--    Phone NVARCHAR(20) NOT NULL,
--    DeletedDate DATETIME NOT NULL DEFAULT GETDATE()
--);

---- DeletedEmployees: Stores details of soft-deleted Employee records.
--CREATE TABLE DeletedEmployees
--(
--    DeletedID INT IDENTITY(1,1) PRIMARY KEY,
--    EmployeeID INT NOT NULL,
--    PersonID INT NOT NULL,
--    TitleID INT NOT NULL,
--    StartDate DATE NOT NULL,
--    EndDate DATE NOT NULL,
--    DeletedDate DATETIME NOT NULL DEFAULT GETDATE()
--);




--------------------------------------------------
-- Add Sample Data To The EmployeeTitles Table
--------------------------------------------------

--INSERT INTO EmployeeTitles (Title, Description)
--VALUES 
--('Software Engineer', 'Responsible for designing, developing, and maintaining software applications.'),
--('Project Manager', 'Oversees project planning, execution, and delivery.'),
--('Business Analyst', 'Analyzes business processes and requirements for improvements.'),
--('Quality Assurance Engineer', 'Ensures software quality through rigorous testing and validation.'),
--('HR Manager', 'Manages recruitment, employee relations, and organizational policies.'),
--('DevOps Engineer', 'Facilitates collaboration between development and operations teams.'),
--('Data Scientist', 'Analyzes and interprets complex data to drive decision-making.'),
--('UX Designer', 'Designs and enhances user interfaces to improve user experience.');
