
CREATE DATABASE CandidateHubDatabase;

GO

USE CandidateHubDatabase

GO

CREATE LOGIN api_data WITH PASSWORD = 's*XgF6Q&./uzwDHyhNvC95', 
DEFAULT_DATABASE=[CandidateHubDatabase], DEFAULT_LANGUAGE=us_english, CHECK_POLICY= OFF;

GO

CREATE USER api_data FOR LOGIN api_data


GRANT CREATE FUNCTION,CREATE PROCEDURE,CREATE SYNONYM,CREATE TABLE,CREATE TYPE,CREATE VIEW,EXECUTE,DELETE TO api_data
GO

GRANT SELECT,ALTER,CONTROL,CREATE SEQUENCE,DELETE,EXECUTE,INSERT,UPDATE  ON SCHEMA :: [dbo] TO api_data

GO

CREATE TABLE Candidates (
    Id UNIQUEIDENTIFIER PRIMARY KEY,
    FirstName NVARCHAR(50) NOT NULL,
    LastName NVARCHAR(50) NOT NULL,
    PhoneNumber NVARCHAR(20),
    Email NVARCHAR(100),
    CallTimeInterval NVARCHAR(50),
    LinkedinProfile NVARCHAR(255),
    GithubProfile NVARCHAR(255),
    Comment NVARCHAR(1000)
);

