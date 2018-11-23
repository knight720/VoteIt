CREATE DATABASE [VoteItDB];
GO

USE [VoteItDB];
GO

CREATE TABLE [Feed] (
    [Feed_Id] int NOT NULL IDENTITY,
    [Feed_Title] nvarchar(160) NOT NULL,
    [Feed_Like] int NOT NULL,
	[Feed_CreatedDateTime] datetime NOT NULL,
    [Feed_CreatedUser] varchar(50) NOT NULL,
    [Feed_ValidFlag] [bit] NOT NULL,
    CONSTRAINT [PK_Feed] PRIMARY KEY ([Feed_Id])
);
GO
