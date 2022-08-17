USE master;
GO

DROP DATABASE IF EXISTS CustomerDB;
GO

CREATE DATABASE CustomerDB;
GO

USE CustomerDB;
GO

CREATE TABLE Customers
(
    Id INT CONSTRAINT PK_Customers PRIMARY KEY IDENTITY(1,1),
    FirstName NVARCHAR(450) NOT NULL,
    LastName NVARCHAR(450) NOT NULL,
    Email NVARCHAR(450) CONSTRAINT UQ_Customers_Email UNIQUE,
    Gender NVARCHAR(15) NOT NULL,
    Age INT NOT NULL
)
GO


-- Please change the path to the json file. otherwise it won't work
INSERT INTO Customers
SELECT FirstName, LastName, Email, Gender, Age
FROM OPENROWSET (BULK 'C:\Users\megad\RiderProjects\SystemsProgrammingHomework\CustomerDb\Customers.json', 
    Single_CLOB) AS JsonFile
CROSS APPLY OPENJSON(BulkColumn)
WITH(FirstName NVARCHAR(450), LastName NVARCHAR(450), Email NVARCHAR(450), Gender NVARCHAR(15), Age INT) as JsonTable
GO
