
----------------------------------------------------
---- Tasks Procedures
----------------------------------------------------

---- Procedure: sp_GetTask
---- Retrieves a Person record by ID.
--ALTER PROCEDURE sp_GetTask
--    @TaskID INT
--AS
--BEGIN
--    SET NOCOUNT ON;
--    SELECT *
--    FROM Tasks
--    WHERE ID = @TaskID AND StatusID <> 4;
--END;
--GO

-- Procedure: sp_GetAllTasks
--ALTER PROCEDURE sp_GetAllTasks
--AS
--BEGIN
--    SET NOCOUNT ON;
--    SELECT *
--    FROM Tasks WHERE StatusID <> 4
--END;
--GO


 --Procedure: sp_GetAllTasksBySpecificStatus
--CREATE PROCEDURE sp_GetAllTasksBySpecificStatus
--    @StatusID INT
--AS
--BEGIN
--    SET NOCOUNT ON;
--    SELECT *
--    FROM Tasks WHERE Tasks.StatusID = @StatusID
--END;
--GO

--CREATE PROCEDURE sp_AddTask
--(
--    @StatusID   INT,
--    @TaskTitle  NVARCHAR(50),
--    @ListedBy   INT,
--    @NewTaskID  INT OUTPUT
--)
--AS
--BEGIN
--    SET NOCOUNT ON;

--     --Validate the StatusID exists in TasksStatus table
--    IF NOT EXISTS (SELECT 1 FROM TasksStatus WHERE ID = @StatusID)
--    BEGIN
--        RAISERROR('Invalid StatusID.', 16, 1);
--        RETURN;
--    END;

--     --Check if the ListedBy employee is active
--    IF NOT EXISTS
--    (
--        SELECT 1
--        FROM Employee
--        WHERE ID = @ListedBy
--          AND GETDATE() BETWEEN StartDate AND EndDate
--    )
--    BEGIN
--        RAISERROR('The employee is not active and cannot list a new task.', 16, 1);
--        RETURN;
--    END;

--     --Insert the new task
--    INSERT INTO Tasks ( TaskTitle, ListedBy)
--    VALUES ( @TaskTitle, @ListedBy);

--     --Output the new task ID using SCOPE_IDENTITY()
--    SET @NewTaskID = SCOPE_IDENTITY();
--END;
--GO





--CREATE PROCEDURE sp_UpdateTask
--(
--    @TaskID       INT,
--    @StatusID     INT,
--    @TaskTitle    NVARCHAR(50),
--    @CompletedBy  INT = NULL,
--    @CompletedDate DATETIME = NULL,
--    @Updated BIT OUTPUT
--)
--AS
--BEGIN
--    SET NOCOUNT ON;
--    SET @Updated = 0; -- Default to not updated

--     Check if the task exists
--    IF NOT EXISTS (SELECT 1 FROM Tasks WHERE ID = @TaskID)
--    BEGIN
--        RAISERROR('Task not found.', 16, 1);
--        RETURN;
--    END;

--     Validate TaskStatus exists
--    IF NOT EXISTS (SELECT 1 FROM TasksStatus WHERE ID = @StatusID)
--    BEGIN
--        RAISERROR('Invalid StatusID.', 16, 1);
--        RETURN;
--    END;

--     If CompletedBy is provided, ensure the employee is active
--    IF @CompletedBy IS NOT NULL AND NOT EXISTS
--    (
--        SELECT 1
--        FROM Employee
--        WHERE ID = @CompletedBy
--          AND GETDATE() BETWEEN StartDate AND EndDate
--    )
--    BEGIN
--        RAISERROR('The employee is not active and cannot complete the task.', 16, 1);
--        RETURN;
--    END;

--     Update the task
--    UPDATE Tasks
--    SET 
--        StatusID = @StatusID,
--        TaskTitle = @TaskTitle,
--        CompletedBy = @CompletedBy,
--        CompletedDate = @CompletedDate
--    WHERE ID = @TaskID;

--     Set output flag to indicate success
--    SET @Updated = 1;
--END;
--GO



--CREATE PROCEDURE sp_ModifyTaskStatus
--(
--    @TaskID          INT,
--	@NewTaskStatus   INT,
--    @CompletedBy     INT
--)
--AS
--BEGIN
--    SET NOCOUNT ON;

--     1) Check if the "CompletedBy" employee is active
--    IF NOT EXISTS
--    (
--        SELECT 1
--        FROM Employee
--        WHERE ID = @CompletedBy
--          AND GETDATE() BETWEEN StartDate AND EndDate
--    )
--    BEGIN
--        RAISERROR('The employee is not active and cannot complete a task.', 16, 1);
--        RETURN;
--    END;

--     2) Make sure the task itself exists (optional but recommended)
--    IF NOT EXISTS
--    (
--        SELECT 1
--        FROM Tasks
--        WHERE ID = @TaskID
--    )
--    BEGIN
--        RAISERROR('Task does not exist.', 16, 1);
--        RETURN;
--    END;

--     3) Update the task to "completed" (or whatever the new status is)
--    UPDATE Tasks
--    SET 
--        StatusID       = 3,
--        CompletedBy    = @CompletedBy,
--        CompletedDate  = GETDATE()
--    WHERE ID = @TaskID;
--END;
--GO




--ALTER PROCEDURE sp_CompleteTask
--(
--    @TaskID          INT,
--    @CompletedBy     INT
--)
--AS
--BEGIN
--    SET NOCOUNT OFF;

--    -- 1) Check if the "CompletedBy" employee is active
--    IF NOT EXISTS
--    (
--        SELECT 1
--        FROM Employee
--        WHERE ID = @CompletedBy
--          AND GETDATE() BETWEEN StartDate AND EndDate
--    )
--    BEGIN
--        RAISERROR('The employee is not active and cannot complete a task.', 16, 1);
--        RETURN;
--    END;

--    -- 2) Make sure the task itself exists (optional but recommended)
--    IF NOT EXISTS
--    (
--        SELECT 1
--        FROM Tasks
--        WHERE ID = @TaskID
--    )
--    BEGIN
--        RAISERROR('Task does not exist.', 16, 1);
--        RETURN;
--    END;

--     --3) Update the task to "completed" (or whatever the new status is)
--    UPDATE Tasks
--    SET 
--        StatusID       = 3,
--        CompletedBy    = @CompletedBy,
--        CompletedDate  = GETDATE()
--    WHERE ID = @TaskID;
--END;
--GO




--CREATE PROCEDURE sp_GetTasksByUser
--(
--    @UserID INT
--)
--AS
--BEGIN
--    SET NOCOUNT ON;

--    ------------------------------------------------------------------------------
--    -- 1) (Optional) Check if this employee is currently active
--    ------------------------------------------------------------------------------
--    IF NOT EXISTS 
--    (
--        SELECT 1 
--        FROM Employee 
--        WHERE ID = @UserID
--          AND (GETDATE() BETWEEN StartDate AND EndDate) OR IsActive = 0 
--    )
--    BEGIN
--        RAISERROR('The employee is not active. No tasks to show.', 16, 1);
--        RETURN;
--    END;

--    ------------------------------------------------------------------------------
--    -- 2) Return tasks either LISTED or COMPLETED by this active employee
--    ------------------------------------------------------------------------------
--    SELECT
--         T.ID
--        ,T.TaskTitle
--        ,T.StatusID
--        ,TS.Status            AS [StatusName]      -- assuming TaskStatus has StatusName
--        ,T.ListedBy
--        ,LB.FullNAME          AS [ListedByName]    -- if Employee table has EmployeeName
--        ,T.CompletedBy
--        ,CB.FullName          AS [CompletedByName]
--        ,T.ListedDate
--        ,T.CompletedDate
--    FROM Tasks T
--    JOIN TaskStatus TS    ON T.StatusID    = TS.ID
--    LEFT JOIN Employee LB ON T.ListedBy    = LB.ID
--    LEFT JOIN Employee CB ON T.CompletedBy = CB.ID
--    WHERE T.ListedBy = @UserID
--       OR T.CompletedBy = @UserID;
--END;
--GO



--CREATE PROCEDURE sp_GetListedTasksByUser
--(
--    @UserID INT
--)
--AS
--BEGIN
--    SET NOCOUNT ON;

--    -- Check if the employee is active
--    IF NOT EXISTS (
--        SELECT 1 
--        FROM Employee 
--        WHERE ID = @UserID
--          AND (GETDATE() BETWEEN StartDate AND EndDate) OR IsActive = 0
--    )
--    BEGIN
--        RAISERROR('The employee is not active and cannot list tasks.', 16, 1);
--        RETURN;
--    END;

--    -- Retrieve tasks listed by the employee
--    SELECT
--         T.ID,
--         T.TaskTitle,
--         T.StatusID,
--         T.ListedBy,
--         T.ListedDate
--    FROM Tasks T
--    WHERE T.ListedBy = @UserID;
--END;
--GO




--CREATE PROCEDURE sp_GetCompletedTasksByUser
--(
--    @UserID INT
--)
--AS
--BEGIN
--    SET NOCOUNT ON;

--    -- Check if the employee is active
--    IF NOT EXISTS (
--        SELECT 1 
--        FROM Employee 
--        WHERE ID = @UserID
--          AND (GETDATE() BETWEEN StartDate AND EndDate) OR IsActive = 0
--    )
--    BEGIN
--        RAISERROR('The employee is not active and cannot complete tasks.', 16, 1);
--        RETURN;
--    END;

--    -- Retrieve tasks completed by the employee
--    SELECT
--         T.ID,
--         T.TaskTitle,
--         T.StatusID,
--         T.CompletedBy,
--         T.CompletedDate
--    FROM Tasks T
--    WHERE T.CompletedBy = @UserID;
--END;
--GO


