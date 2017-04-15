
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 04/14/2017 20:08:23
-- Generated from EDMX file: C:\Users\Hassan Zaidi\documents\visual studio 2015\Projects\BizLogic\DataModel\HotelDatabase.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [HotelDatabase];
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

-- Creating table 'People'
CREATE TABLE [dbo].[People] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [firstName] nvarchar(max)  NOT NULL,
    [lastName] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'Rooms'
CREATE TABLE [dbo].[Rooms] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [RoomType_Id] int  NOT NULL,
    [Reservations_Id] int  NOT NULL
);
GO

-- Creating table 'RoomTypes'
CREATE TABLE [dbo].[RoomTypes] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [type] nvarchar(max)  NOT NULL,
    [basePrice] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'Reservations'
CREATE TABLE [dbo].[Reservations] (
    [checkIn] nvarchar(max)  NULL,
    [checkOut] nvarchar(max)  NULL,
    [Id] int IDENTITY(1,1) NOT NULL,
    [Customers_Id] int  NOT NULL
);
GO

-- Creating table 'People_Customer'
CREATE TABLE [dbo].[People_Customer] (
    [email] nvarchar(max)  NOT NULL,
    [expirationDate] nvarchar(max)  NOT NULL,
    [member] bit  NOT NULL,
    [lastExpirationDate] nvarchar(max)  NOT NULL,
    [Id] int  NOT NULL
);
GO

-- Creating table 'People_Staff'
CREATE TABLE [dbo].[People_Staff] (
    [Id] int  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [Id] in table 'People'
ALTER TABLE [dbo].[People]
ADD CONSTRAINT [PK_People]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Rooms'
ALTER TABLE [dbo].[Rooms]
ADD CONSTRAINT [PK_Rooms]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'RoomTypes'
ALTER TABLE [dbo].[RoomTypes]
ADD CONSTRAINT [PK_RoomTypes]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Reservations'
ALTER TABLE [dbo].[Reservations]
ADD CONSTRAINT [PK_Reservations]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'People_Customer'
ALTER TABLE [dbo].[People_Customer]
ADD CONSTRAINT [PK_People_Customer]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'People_Staff'
ALTER TABLE [dbo].[People_Staff]
ADD CONSTRAINT [PK_People_Staff]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [RoomType_Id] in table 'Rooms'
ALTER TABLE [dbo].[Rooms]
ADD CONSTRAINT [FK_RoomTypeRoom]
    FOREIGN KEY ([RoomType_Id])
    REFERENCES [dbo].[RoomTypes]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_RoomTypeRoom'
CREATE INDEX [IX_FK_RoomTypeRoom]
ON [dbo].[Rooms]
    ([RoomType_Id]);
GO

-- Creating foreign key on [Reservations_Id] in table 'Rooms'
ALTER TABLE [dbo].[Rooms]
ADD CONSTRAINT [FK_RoomReservation]
    FOREIGN KEY ([Reservations_Id])
    REFERENCES [dbo].[Reservations]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_RoomReservation'
CREATE INDEX [IX_FK_RoomReservation]
ON [dbo].[Rooms]
    ([Reservations_Id]);
GO

-- Creating foreign key on [Customers_Id] in table 'Reservations'
ALTER TABLE [dbo].[Reservations]
ADD CONSTRAINT [FK_ReservationCustomer]
    FOREIGN KEY ([Customers_Id])
    REFERENCES [dbo].[People_Customer]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ReservationCustomer'
CREATE INDEX [IX_FK_ReservationCustomer]
ON [dbo].[Reservations]
    ([Customers_Id]);
GO

-- Creating foreign key on [Id] in table 'People_Customer'
ALTER TABLE [dbo].[People_Customer]
ADD CONSTRAINT [FK_Customer_inherits_Person]
    FOREIGN KEY ([Id])
    REFERENCES [dbo].[People]
        ([Id])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- Creating foreign key on [Id] in table 'People_Staff'
ALTER TABLE [dbo].[People_Staff]
ADD CONSTRAINT [FK_Staff_inherits_Person]
    FOREIGN KEY ([Id])
    REFERENCES [dbo].[People]
        ([Id])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------