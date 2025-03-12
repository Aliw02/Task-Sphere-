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
---- Departments Procedures
----------------------------------------------------
--ALTER PROCEDURE sp_DepartmentsData
--AS
--BEGIN
--    SET NOCOUNT ON;

--    -- Select all records from the Departments table
--    SELECT * FROM Departments ORDER BY ID;
--END;
