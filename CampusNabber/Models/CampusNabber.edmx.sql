
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 03/09/2016 08:08:59
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


-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[PostItems]', 'U') IS NOT NULL
    DROP TABLE [dbo].[PostItems];
GO
IF OBJECT_ID(N'[dbo].[Schools]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Schools];
GO
IF OBJECT_ID(N'[dbo].[Users]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Users];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'PostItems'
CREATE TABLE [dbo].[PostItems] (
    [object_id] uniqueidentifier  NOT NULL,
    [username] nvarchar(40)  NOT NULL,
    [school_name] nvarchar(100)  NOT NULL,
    [post_date] datetime  NOT NULL,
    [price] smallint  NOT NULL,
    [title] nvarchar(100)  NOT NULL,
    [description] nvarchar(max)  NOT NULL,
    [photo_path] nvarchar(max)  NOT NULL,
    [category] nvarchar(60)  NOT NULL
);
GO

-- Creating table 'Schools'
CREATE TABLE [dbo].[Schools] (
    [object_id] uniqueidentifier  NOT NULL,
    [school_name] nvarchar(100)  NOT NULL,
    [address] nvarchar(80)  NOT NULL,
    [main_hex_color] nvarchar(max)  NOT NULL,
    [secondary_hex_color] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'Users'
CREATE TABLE [dbo].[Users] (
    [object_id] uniqueidentifier  NOT NULL,
    [username] nvarchar(40)  NOT NULL,
    [encrypted_password] varbinary(max)  NOT NULL,
    [student_email] nvarchar(60)  NOT NULL,
    [school_name] nvarchar(100)  NOT NULL
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

-- Creating primary key on [object_id] in table 'Users'
ALTER TABLE [dbo].[Users]
ADD CONSTRAINT [PK_Users]
    PRIMARY KEY CLUSTERED ([object_id] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------