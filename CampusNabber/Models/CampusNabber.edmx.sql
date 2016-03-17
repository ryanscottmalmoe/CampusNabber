
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 03/17/2016 05:40:34
-- Generated from EDMX file: C:\Users\rmalmoe\Desktop\CampusNabber\CampusNabber\Models\CampusNabber.edmx
-- --------------------------------------------------



-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[PostItems]', 'U') IS NOT NULL
    DROP TABLE [dbo].[PostItems];
GO

-- Creating table 'PostItems'
CREATE TABLE [dbo].[PostItems] (
    [object_id] uniqueidentifier  NOT NULL,
    [username] nvarchar(40)  NOT NULL,
    [school_name] nvarchar(100)  NOT NULL,
    [post_date] datetime  NOT NULL,
    [price] smallint  NOT NULL,
    [title] nvarchar(100)  NOT NULL,
    [description] nvarchar(max)  NOT NULL,
    [photo_path_id] uniqueidentifier  NULL,
    [category] nvarchar(60)  NOT NULL,
    [tags] nvarchar(max)  NOT NULL
);
GO


-- Creating primary key on [object_id] in table 'PostItems'
ALTER TABLE [dbo].[PostItems]
ADD CONSTRAINT [PK_PostItems]
    PRIMARY KEY CLUSTERED ([object_id] ASC);
GO


-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------