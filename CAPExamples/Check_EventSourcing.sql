USE [CAP_EventSourcing]
GO

SELECT SUM(Balance), DateOfBusiness
FROM AccountBalance
GROUP BY DateOfBusiness