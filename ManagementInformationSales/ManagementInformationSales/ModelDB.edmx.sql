
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, and Azure
-- --------------------------------------------------
-- Date Created: 11/26/2014 13:44:36
-- Generated from EDMX file: C:\Users\Vlad\Documents\GitHub\ManagerFolderWatcher\ManagementInformationSales\ManagementInformationSales\ModelDB.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [InformationSalesDB];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------


-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------


-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'ClientSet'
CREATE TABLE [dbo].[ClientSet] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [FullName] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'ManagerSet'
CREATE TABLE [dbo].[ManagerSet] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [SecondName] nvarchar(max)  NOT NULL,
    [FirstName] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'OrderSet'
CREATE TABLE [dbo].[OrderSet] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [OrderDate] datetime  NOT NULL,
    [ManagerId] int  NOT NULL,
    [ClientId] int  NOT NULL,
    [ProductName] nvarchar(max)  NOT NULL,
    [Cost] float  NOT NULL,
    [Manager_Id] int  NOT NULL,
    [Client_Id] int  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [Id] in table 'ClientSet'
ALTER TABLE [dbo].[ClientSet]
ADD CONSTRAINT [PK_ClientSet]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'ManagerSet'
ALTER TABLE [dbo].[ManagerSet]
ADD CONSTRAINT [PK_ManagerSet]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'OrderSet'
ALTER TABLE [dbo].[OrderSet]
ADD CONSTRAINT [PK_OrderSet]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [Manager_Id] in table 'OrderSet'
ALTER TABLE [dbo].[OrderSet]
ADD CONSTRAINT [FK_ManagerOrder]
    FOREIGN KEY ([Manager_Id])
    REFERENCES [dbo].[ManagerSet]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_ManagerOrder'
CREATE INDEX [IX_FK_ManagerOrder]
ON [dbo].[OrderSet]
    ([Manager_Id]);
GO

-- Creating foreign key on [Client_Id] in table 'OrderSet'
ALTER TABLE [dbo].[OrderSet]
ADD CONSTRAINT [FK_ClientOrder]
    FOREIGN KEY ([Client_Id])
    REFERENCES [dbo].[ClientSet]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_ClientOrder'
CREATE INDEX [IX_FK_ClientOrder]
ON [dbo].[OrderSet]
    ([Client_Id]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------