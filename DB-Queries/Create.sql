USE [TaskSphere]
GO
/****** Object:  StoredProcedure [dbo].[sp_GetAllEmployeesTitles]    Script Date: 25/03/07 7:02:43 PM ******/
--SET ANSI_NULLS ON
--GO
--SET QUOTED_IDENTIFIER ON
--GO

---- Procedure: sp_GetAllEmployeesTitles
---- Retrieves an All Employees Titles
--ALTER PROCEDURE sp_GetAllEmployeesTitles
--AS
--BEGIN
--    SET NOCOUNT ON;
--    SELECT ID, Title
--    FROM EmployeeTitles
--	  ORDER BY ID
--END;

EXEC sp_GetAllEmployeesTitles;

