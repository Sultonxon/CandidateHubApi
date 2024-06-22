This project is created for Technical task provided by Sigma Software.
Before running this project, you need to perform simple configurations:
Database:
  In this project SqlServer is used as data source, Database migration script is located in project: https://github.com/Sultonxon/CandidateHubApi/blob/master/CandidateHub.Api/Data/MSSQL/Sql/Database-Boot.sql
Just copy entire sql script and run in your sql server.
After that you will have CandidateHubDatabase database. Then you need to configure server host and ip address in connection string in appsettings.json(https://github.com/Sultonxon/CandidateHubApi/blob/master/CandidateHub.Api/appsettings.json).
To be able run tests, you also need to perform this configuration in CandidateHub.Api.Tests/GlobalTestConstants file (https://github.com/Sultonxon/CandidateHubApi/blob/master/CandidateHub.Api.Tests/GlobalTestConstants.cs).
