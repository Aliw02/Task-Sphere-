--CREATE TRIGGER trg_SoftDeletePerson
--ON Person
--INSTEAD OF DELETE
--AS
--BEGIN
--    SET NOCOUNT ON;
    
--    -- Log the record in the audit table
--    INSERT INTO DeletedPersons (PersonID, FullName, BirthDate, Address, Email, Phone, DeletedDate)
--    SELECT ID, FullName, BirthDate, Address, Email, Phone, GETDATE()
--    FROM deleted;
    
--    -- Update the original record to mark it as inactive.
--    UPDATE P
--    SET IsActive = 0,
--        ModifiedDate = GETDATE()
--    FROM Person P
--    INNER JOIN deleted D ON P.ID = D.ID;
--END;
--GO

-- Trigger for Employee: Instead of physically deleting, mark as inactive and log deletion.
--CREATE TRIGGER trg_SoftDeleteEmployee
--ON Employee
--INSTEAD OF DELETE
--AS
--BEGIN
--    SET NOCOUNT ON;
    
--    -- Log the record in the audit table
--    INSERT INTO DeletedEmployees (EmployeeID, PersonID, TitleID, StartDate, EndDate, DeletedDate)
--    SELECT ID, PersonID, TitleID, StartDate, EndDate, GETDATE()
--    FROM deleted;
    
--    -- Update the original record to mark it as inactive.
--    UPDATE E
--    SET IsActive = 0,
--        ModifiedDate = GETDATE()
--    FROM Employee E
--    INNER JOIN deleted D ON E.ID = D.ID;
--END;
--GO

----------------------------------------------------
---- CRUD Stored Procedures
----------------------------------------------------

---- Procedure: sp_AddPerson
---- Inserts a new Person record and returns the new PersonID.
--CREATE PROCEDURE sp_AddPerson
--    @FullName NVARCHAR(100),
--    @BirthDate DATE,
--    @Address NVARCHAR(200),
--    @Email NVARCHAR(100),
--    @Phone NVARCHAR(20),
--    @PersonID INT OUTPUT
--AS
--BEGIN
--    SET NOCOUNT ON;
--    BEGIN TRY
--        INSERT INTO Person 
--            (FullName, BirthDate, Address, Email, Phone, CreatedDate, ModifiedDate)
--        VALUES 
--            (@FullName, @BirthDate, @Address, @Email, @Phone, GETDATE(), GETDATE());

--        SET @PersonID = SCOPE_IDENTITY();
--    END TRY
--    BEGIN CATCH
--        DECLARE @ErrorMessage NVARCHAR(4000), @ErrorSeverity INT, @ErrorState INT;
--        SELECT @ErrorMessage = ERROR_MESSAGE(), 
--               @ErrorSeverity = ERROR_SEVERITY(), 
--               @ErrorState = ERROR_STATE();
--        RAISERROR (@ErrorMessage, @ErrorSeverity, @ErrorState);
--    END CATCH
--END;
--GO

---- Procedure: sp_UpdatePerson
---- Updates an existing Person record.
--CREATE PROCEDURE sp_UpdatePerson
--    @PersonID INT,
--    @FullName NVARCHAR(100),
--    @BirthDate DATE,
--    @Address NVARCHAR(200),
--    @Email NVARCHAR(100),
--    @Phone NVARCHAR(20)
--AS
--BEGIN
--    SET NOCOUNT OFF;
--    BEGIN TRY
--        UPDATE Person
--        SET FullName = @FullName,
--            BirthDate = @BirthDate,
--            Address = @Address,
--            Email = @Email,
--            Phone = @Phone,
--            ModifiedDate = GETDATE()
--        WHERE ID = @PersonID;

--        IF @@ROWCOUNT = 0
--            RAISERROR('Person not found.', 16, 1);
--    END TRY
--    BEGIN CATCH
--        DECLARE @ErrorMessage NVARCHAR(4000), @ErrorSeverity INT, @ErrorState INT;
--        SELECT @ErrorMessage = ERROR_MESSAGE(), 
--               @ErrorSeverity = ERROR_SEVERITY(), 
--               @ErrorState = ERROR_STATE();
--        RAISERROR (@ErrorMessage, @ErrorSeverity, @ErrorState);
--    END CATCH
--END;
--GO

---- Procedure: sp_DeletePerson
---- Deletes (soft deletes) a Person record.
--CREATE PROCEDURE sp_DeletePerson
--    @PersonID INT
--AS
--BEGIN
--    SET NOCOUNT OFF;
--    BEGIN TRY
--        DELETE FROM Person
--        WHERE ID = @PersonID;

--        IF @@ROWCOUNT = 0
--            RAISERROR('Person not found or already inactive.', 16, 1);
--    END TRY
--    BEGIN CATCH
--        DECLARE @ErrorMessage NVARCHAR(4000), @ErrorSeverity INT, @ErrorState INT;
--        SELECT @ErrorMessage = ERROR_MESSAGE(), 
--               @ErrorSeverity = ERROR_SEVERITY(), 
--               @ErrorState = ERROR_STATE();
--        RAISERROR (@ErrorMessage, @ErrorSeverity, @ErrorState);
--    END CATCH
--END;
--GO

---- Procedure: sp_GetPerson
---- Retrieves a Person record by ID.
--CREATE PROCEDURE sp_GetPerson
--    @PersonID INT
--AS
--BEGIN
--    SET NOCOUNT ON;
--    SELECT *
--    FROM Person
--    WHERE ID = @PersonID;
--END;
--GO

-- Procedure: sp_GetAllPeople
--CREATE PROCEDURE sp_GetAllPeople
--AS
--BEGIN
--    SET NOCOUNT ON;
--    SELECT *
--    FROM Person WHERE IsActive = 1
--END;
--GO

---- Procedure: sp_PersonIsExits
---- Retrieves a Person record by ID.
--CREATE PROCEDURE sp_PersonIsExist
--    @PersonID INT,
--    @Exists BIT OUTPUT
--AS
--BEGIN
--    SET NOCOUNT ON;

--    SET @Exists = CASE 
--        WHEN EXISTS (SELECT 1 FROM Person WHERE ID = @PersonID) THEN 1
--        ELSE 0
--    END;
--END;
--GO


----------------------------------------------------
---- Employee Procedures
----------------------------------------------------

---- Procedure: sp_AddEmployee
---- Inserts a new Employee record and returns the new EmployeeID.
--CREATE PROCEDURE sp_AddEmployee
--    @PersonID INT,
--    @TitleID INT,
--    @StartDate DATE,
--    @EndDate DATE,
--    @EmployeeID INT OUTPUT
--AS
--BEGIN
--    SET NOCOUNT ON;
--    BEGIN TRY
--        IF @StartDate > @EndDate
--        BEGIN
--            RAISERROR('StartDate cannot be later than EndDate.', 16, 1);
--            RETURN;
--        END

--        INSERT INTO Employee
--            (PersonID, TitleID, StartDate, EndDate, CreatedDate, ModifiedDate)
--        VALUES 
--            (@PersonID, @TitleID, @StartDate, @EndDate, GETDATE(), GETDATE());

--        SET @EmployeeID = SCOPE_IDENTITY();
--    END TRY
--    BEGIN CATCH
--        DECLARE @ErrorMessage NVARCHAR(4000), @ErrorSeverity INT, @ErrorState INT;
--        SELECT @ErrorMessage = ERROR_MESSAGE(), 
--               @ErrorSeverity = ERROR_SEVERITY(), 
--               @ErrorState = ERROR_STATE();
--        RAISERROR (@ErrorMessage, @ErrorSeverity, @ErrorState);
--    END CATCH
--END;
--GO

---- Procedure: sp_UpdateEmployee
---- Updates an existing Employee record.
--CREATE PROCEDURE sp_UpdateEmployee
--    @EmployeeID INT,
--    @PersonID INT,
--    @TitleID INT,
--    @StartDate DATE,
--    @EndDate DATE
--AS
--BEGIN
--    SET NOCOUNT ON;
--    BEGIN TRY
--        IF @StartDate > @EndDate
--        BEGIN
--            RAISERROR('StartDate cannot be later than EndDate.', 16, 1);
--            RETURN;
--        END

--        UPDATE Employee
--        SET PersonID = @PersonID,
--            TitleID = @TitleID,
--            StartDate = @StartDate,
--            EndDate = @EndDate,
--            ModifiedDate = GETDATE()
--        WHERE ID = @EmployeeID;

--        IF @@ROWCOUNT = 0
--            RAISERROR('Employee not found.', 16, 1);
--    END TRY
--    BEGIN CATCH
--        DECLARE @ErrorMessage NVARCHAR(4000), @ErrorSeverity INT, @ErrorState INT;
--        SELECT @ErrorMessage = ERROR_MESSAGE(), 
--               @ErrorSeverity = ERROR_SEVERITY(), 
--               @ErrorState = ERROR_STATE();
--        RAISERROR (@ErrorMessage, @ErrorSeverity, @ErrorState);
--    END CATCH
--END;
--GO

---- Procedure: sp_DeleteEmployee
---- Deletes (soft deletes) an Employee record.
--CREATE PROCEDURE sp_DeleteEmployee
--    @EmployeeID INT
--AS
--BEGIN
--    SET NOCOUNT ON;
--    BEGIN TRY
--        DELETE FROM Employee
--        WHERE ID = @EmployeeID;

--        IF @@ROWCOUNT = 0
--            RAISERROR('Employee not found or already inactive.', 16, 1);
--    END TRY
--    BEGIN CATCH
--        DECLARE @ErrorMessage NVARCHAR(4000), @ErrorSeverity INT, @ErrorState INT;
--        SELECT @ErrorMessage = ERROR_MESSAGE(), 
--               @ErrorSeverity = ERROR_SEVERITY(), 
--               @ErrorState = ERROR_STATE();
--        RAISERROR (@ErrorMessage, @ErrorSeverity, @ErrorState);
--    END CATCH
--END;
--GO

---- Procedure: sp_GetEmployee
---- Retrieves an Employee record by ID along with related Person and Title details.
--CREATE PROCEDURE sp_GetEmployee
--    @EmployeeID INT
--AS
--BEGIN
--    SET NOCOUNT ON;
--    SELECT E.*, P.FullName, P.Email, ET.Title
--    FROM Employee E
--    INNER JOIN Person P ON E.PersonID = P.ID
--    INNER JOIN EmployeeTitles ET ON E.TitleID = ET.ID
--    WHERE E.ID = @EmployeeID;
--END;
--GO

---- Procedure: sp_AddPersonAndEmployee
---- Inserts a Person record and an associated Employee record in one transaction.
--CREATE PROCEDURE sp_AddPersonAndEmployee
--    @FullName NVARCHAR(100),
--    @BirthDate DATE,
--    @Address NVARCHAR(200),
--    @Email NVARCHAR(100),
--    @Phone NVARCHAR(20),
--    @TitleID INT,
--    @StartDate DATE,
--    @EndDate DATE,
--    @PersonID INT OUTPUT,
--    @EmployeeID INT OUTPUT
--AS
--BEGIN
--    SET NOCOUNT ON;
--    BEGIN TRY
--        BEGIN TRANSACTION;

--        INSERT INTO Person 
--            (FullName, BirthDate, Address, Email, Phone, CreatedDate, ModifiedDate)
--        VALUES 
--            (@FullName, @BirthDate, @Address, @Email, @Phone, GETDATE(), GETDATE());

--        SET @PersonID = SCOPE_IDENTITY();

--        IF @StartDate > @EndDate
--        BEGIN
--            RAISERROR('StartDate cannot be later than EndDate.', 16, 1);
--            ROLLBACK TRANSACTION;
--            RETURN;
--        END

--        INSERT INTO Employee
--            (PersonID, TitleID, StartDate, EndDate, CreatedDate, ModifiedDate)
--        VALUES 
--            (@PersonID, @TitleID, @StartDate, @EndDate, GETDATE(), GETDATE());

--        SET @EmployeeID = SCOPE_IDENTITY();

--        COMMIT TRANSACTION;
--    END TRY
--    BEGIN CATCH
--        IF @@TRANCOUNT > 0
--            ROLLBACK TRANSACTION;
--        DECLARE @ErrorMessage NVARCHAR(4000), @ErrorSeverity INT, @ErrorState INT;
--        SELECT @ErrorMessage = ERROR_MESSAGE(), 
--               @ErrorSeverity = ERROR_SEVERITY(), 
--               @ErrorState = ERROR_STATE();
--        RAISERROR (@ErrorMessage, @ErrorSeverity, @ErrorState);
--    END CATCH
--END;
--GO

