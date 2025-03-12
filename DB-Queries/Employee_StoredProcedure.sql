----------------------------------------------------
---- Employee Procedures
----------------------------------------------------

---- Procedure: sp_AddEmployee
---- Inserts a new Employee record and returns the new EmployeeID.
--ALTER PROCEDURE sp_AddEmployee
   -- @PersonID INT,
   -- @TitleID INT,
	  --@Salary DECIMAL,
	  --@DepartmentID INT,
   -- @StartDate DATE,
   -- @EndDate DATE,
   -- @EmployeeID INT OUTPUT
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
--            (PersonID, TitleID, Salary, DepartmentID, StartDate, EndDate, CreatedDate, ModifiedDate)
--        VALUES 
--            (@PersonID, @TitleID, @Salary, @DepartmentID, @StartDate, @EndDate, GETDATE(), GETDATE());

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
--    SELECT E.ID, E.PersonID, E.TitleID, E.Salary, E.DepartmentID, E.StartDate, E.EndDate
--    FROM Employee E
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




--CREATE PROCEDURE sp_AllEmployeeData
--AS
--BEGIN
--    SET NOCOUNT ON;

--     Select specific main details of employees
--    SELECT 
--        ID,              -- Employee ID
--        PersonID,            -- Employee Name
--		TitleID,
--		Salary,
--        DepartmentID,     -- Department ID (assuming you have this in the Employee table)
--		StartDate,
--		EndDate
--    FROM Employee WHERE GetDate() BETWEEN Startdate AND EndDate;
--END;
