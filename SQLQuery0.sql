CREATE PROCEDURE sp_AuthorInsert 
@id AS INT, 
@firstname AS NVARCHAR, 
@lastname AS NVARCHAR 
AS 
BEGIN 
  INSERT INTO Authors(Id,FirstName,LastName) 
  VALUES(@id,@firstname,@lastname) 
END 

ALTER PROCEDURE sp_AuthorsDelete 
@id AS INT 
AS 
BEGIN 
 --IF(EXISTS(SELECT * FROM Books AS B WHERE B.Id_Author=@id)) 
 --BEGIN 
 -- DECLARE @IP AS INT 
 -- SELECT @IP=B.Id 
 -- FROM Books AS B 
 -- WHERE B.Id_Author=@id 
  
 -- UPDATE Books 
 -- SET Id_Author=NULL 
 -- WHERE Id_Author=@IP 
 --END 

  DELETE FROM Authors 
  WHERE Id=@id 
END
