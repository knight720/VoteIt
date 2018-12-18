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

CREATE TABLE [dbo].[FeedLike] (
    [FeedLike_Id]              BIGINT       IDENTITY (1, 1) NOT NULL,
    [FeedLike_FeedId]          INT          NOT NULL,
    [FeedLike_CreatedDateTime] DATETIME     NOT NULL,
    [FeedLike_CreatedUser]     VARCHAR (50) NOT NULL,
    [FeedLike_ValidFlag]       BIT          NOT NULL
);


GO
CREATE NONCLUSTERED INDEX [IDX_FeedId_CreatedUser]
    ON [dbo].[FeedLike]([FeedLike_FeedId] ASC, [FeedLike_CreatedUser] ASC);


GO
ALTER TABLE [dbo].[FeedLike]
    ADD CONSTRAINT [PK_FeedLike] PRIMARY KEY CLUSTERED ([FeedLike_Id] ASC);


GO
ALTER TABLE [dbo].[FeedLike]
    ADD CONSTRAINT [CK_FeedLike_FeedId_CreatedUser] UNIQUE NONCLUSTERED ([FeedLike_FeedId] ASC, [FeedLike_CreatedUser] ASC);

