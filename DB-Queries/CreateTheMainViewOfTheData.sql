
----------------------------------------
--------- Creat The Main View ---------
----------------------------------------

CREATE VIEW vw_MainData AS
SELECT 
    E.ID AS EmployeeID,
    P.ID AS PersonID,
    P.FullName,
    P.BirthDate,
    P.Address,
    P.Email,
    P.Phone,
    E.StartDate,
    E.EndDate,
    ET.Title,
    ET.Description,
    E.IsActive AS EmployeeIsActive,
    P.IsActive AS PersonIsActive,
    E.CreatedDate AS EmployeeCreatedDate,
    E.ModifiedDate AS EmployeeModifiedDate,
    P.CreatedDate AS PersonCreatedDate,
    P.ModifiedDate AS PersonModifiedDate

FROM Employee E
INNER JOIN Person P 
    ON E.PersonID = P.ID
INNER JOIN EmployeeTitles ET 
    ON E.TitleID = ET.ID
WHERE   (E.IsActive = 1) AND (P.IsActive = 1);

GO
