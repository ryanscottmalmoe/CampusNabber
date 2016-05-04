
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 05/04/2016 01:56:36
-- Generated from EDMX file: C:\Users\rmalmoe\Desktop\CampusNabber\CampusNabber\Models\CampusNabber.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [ewucampusnabber];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK_PostItemSchool]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[PostItems] DROP CONSTRAINT [FK_PostItemSchool];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[PostItems]', 'U') IS NOT NULL
    DROP TABLE [dbo].[PostItems];
GO
IF OBJECT_ID(N'[dbo].[Schools]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Schools];
GO
IF OBJECT_ID(N'[dbo].[C__MigrationHistory]', 'U') IS NOT NULL
    DROP TABLE [dbo].[C__MigrationHistory];
GO
IF OBJECT_ID(N'[dbo].[PostItemPhotos]', 'U') IS NOT NULL
    DROP TABLE [dbo].[PostItemPhotos];
GO
IF OBJECT_ID(N'[dbo].[FlagPosts]', 'U') IS NOT NULL
    DROP TABLE [dbo].[FlagPosts];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'PostItems'
CREATE TABLE [dbo].[PostItems] (
    [object_id] uniqueidentifier  NOT NULL,
    [username] nvarchar(40)  NOT NULL,
    [post_date] datetime  NOT NULL,
    [price] smallint  NOT NULL,
    [title] nvarchar(100)  NOT NULL,
    [description] nvarchar(max)  NOT NULL,
    [category] nvarchar(60)  NOT NULL,
    [school_id] uniqueidentifier  NULL,
    [photo_path_id] uniqueidentifier  NOT NULL
);
GO

-- Creating table 'Schools'
CREATE TABLE [dbo].[Schools] (
    [object_id] uniqueidentifier  NOT NULL,
    [school_name] nvarchar(100)  NOT NULL,
    [address] nvarchar(80)  NOT NULL,
    [main_hex_color] nvarchar(max)  NOT NULL,
    [secondary_hex_color] nvarchar(max)  NOT NULL,
    [school_tag] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'C__MigrationHistory'
CREATE TABLE [dbo].[C__MigrationHistory] (
    [MigrationId] nvarchar(150)  NOT NULL,
    [ContextKey] nvarchar(300)  NOT NULL,
    [Model] varbinary(max)  NOT NULL,
    [ProductVersion] nvarchar(32)  NOT NULL
);
GO

-- Creating table 'PostItemPhotos'
CREATE TABLE [dbo].[PostItemPhotos] (
    [object_id] uniqueidentifier  NOT NULL,
    [num_photos] smallint  NOT NULL
);
GO

-- Creating table 'FlagPosts'
CREATE TABLE [dbo].[FlagPosts] (
    [object_id] uniqueidentifier  NOT NULL,
    [flagged_postitem_id] uniqueidentifier  NOT NULL,
    [username_of_post] nvarchar(max)  NOT NULL,
    [flag_reason] nvarchar(max)  NOT NULL,
    [username_of_flagger] nvarchar(max)  NOT NULL,
    [flag_date] datetime  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [object_id] in table 'PostItems'
ALTER TABLE [dbo].[PostItems]
ADD CONSTRAINT [PK_PostItems]
    PRIMARY KEY CLUSTERED ([object_id] ASC);
GO

-- Creating primary key on [object_id] in table 'Schools'
ALTER TABLE [dbo].[Schools]
ADD CONSTRAINT [PK_Schools]
    PRIMARY KEY CLUSTERED ([object_id] ASC);
GO

-- Creating primary key on [MigrationId], [ContextKey] in table 'C__MigrationHistory'
ALTER TABLE [dbo].[C__MigrationHistory]
ADD CONSTRAINT [PK_C__MigrationHistory]
    PRIMARY KEY CLUSTERED ([MigrationId], [ContextKey] ASC);
GO

-- Creating primary key on [object_id] in table 'PostItemPhotos'
ALTER TABLE [dbo].[PostItemPhotos]
ADD CONSTRAINT [PK_PostItemPhotos]
    PRIMARY KEY CLUSTERED ([object_id] ASC);
GO

-- Creating primary key on [object_id] in table 'FlagPosts'
ALTER TABLE [dbo].[FlagPosts]
ADD CONSTRAINT [PK_FlagPosts]
    PRIMARY KEY CLUSTERED ([object_id] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [school_id] in table 'PostItems'
ALTER TABLE [dbo].[PostItems]
ADD CONSTRAINT [FK_PostItemSchool]
    FOREIGN KEY ([school_id])
    REFERENCES [dbo].[Schools]
        ([object_id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_PostItemSchool'
CREATE INDEX [IX_FK_PostItemSchool]
ON [dbo].[PostItems]
    ([school_id]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------