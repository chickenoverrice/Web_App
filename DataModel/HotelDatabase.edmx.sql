
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 05/02/2017 16:13:57
-- Generated from EDMX file: C:\Users\Lihao\Documents\Project\hotelManagementSystem\DataModel\HotelDatabase.edmx
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

IF OBJECT_ID(N'[dbo].[FK_RoomTypeRoom]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Rooms] DROP CONSTRAINT [FK_RoomTypeRoom];
GO
IF OBJECT_ID(N'[dbo].[FK_RoomReservation]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Reservations] DROP CONSTRAINT [FK_RoomReservation];
GO
IF OBJECT_ID(N'[dbo].[FK_CustomerRoomType]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[People_Customer] DROP CONSTRAINT [FK_CustomerRoomType];
GO
IF OBJECT_ID(N'[dbo].[FK_ReservationPerson]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Reservations] DROP CONSTRAINT [FK_ReservationPerson];
GO
IF OBJECT_ID(N'[dbo].[FK_RoomTypeReservation]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Reservations] DROP CONSTRAINT [FK_RoomTypeReservation];
GO
IF OBJECT_ID(N'[dbo].[FK_PersonReservation]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Reservations] DROP CONSTRAINT [FK_PersonReservation];
GO
IF OBJECT_ID(N'[dbo].[FK_Customer_inherits_Person]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[People_Customer] DROP CONSTRAINT [FK_Customer_inherits_Person];
GO
IF OBJECT_ID(N'[dbo].[FK_Staff_inherits_Person]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[People_Staff] DROP CONSTRAINT [FK_Staff_inherits_Person];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[People]', 'U') IS NOT NULL
    DROP TABLE [dbo].[People];
GO
IF OBJECT_ID(N'[dbo].[Rooms]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Rooms];
GO
IF OBJECT_ID(N'[dbo].[RoomTypes]', 'U') IS NOT NULL
    DROP TABLE [dbo].[RoomTypes];
GO
IF OBJECT_ID(N'[dbo].[Reservations]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Reservations];
GO
IF OBJECT_ID(N'[dbo].[CurrentDateTimes]', 'U') IS NOT NULL
    DROP TABLE [dbo].[CurrentDateTimes];
GO
IF OBJECT_ID(N'[dbo].[People_Customer]', 'U') IS NOT NULL
    DROP TABLE [dbo].[People_Customer];
GO
IF OBJECT_ID(N'[dbo].[People_Staff]', 'U') IS NOT NULL
    DROP TABLE [dbo].[People_Staff];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'People'
CREATE TABLE [dbo].[People] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [firstName] nvarchar(max)  NOT NULL,
    [lastName] nvarchar(max)  NOT NULL,
    [email] nvarchar(max)  NOT NULL,
    [sessionId] nvarchar(max)  NULL,
    [address] nvarchar(max)  NULL,
    [phone] nvarchar(max)  NULL,
    [city] nvarchar(max)  NULL,
    [state] nvarchar(max)  NULL,
    [zip] nvarchar(max)  NULL,
    [sessionExpiration] datetime  NULL
);
GO

-- Creating table 'Rooms'
CREATE TABLE [dbo].[Rooms] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [occupied] bit  NOT NULL,
    [RoomType_Id] int  NOT NULL
);
GO

-- Creating table 'RoomTypes'
CREATE TABLE [dbo].[RoomTypes] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [type] nvarchar(max)  NOT NULL,
    [basePrice] float  NOT NULL,
    [maxGuests] int  NOT NULL,
    [numberOfRooms] int  NOT NULL,
    [description] nvarchar(max)  NOT NULL,
    [amenities] nvarchar(max)  NOT NULL,
    [picture] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'Reservations'
CREATE TABLE [dbo].[Reservations] (
    [checkIn] datetime  NOT NULL,
    [checkOut] datetime  NOT NULL,
    [Id] int IDENTITY(1,1) NOT NULL,
    [bill] float  NOT NULL,
    [guestsInfo] nvarchar(max)  NULL,
    [firstName] nvarchar(max)  NOT NULL,
    [lastName] nvarchar(max)  NOT NULL,
    [email] nvarchar(max)  NOT NULL,
    [address] nvarchar(max)  NULL,
    [phone] nvarchar(max)  NULL,
    [city] nvarchar(max)  NULL,
    [state] nvarchar(max)  NULL,
    [zip] nvarchar(max)  NULL,
    [RoomTypeId] int  NULL,
    [PersonId] int  NULL,
    [Room_Id] int  NULL
);
GO

-- Creating table 'CurrentDateTimes'
CREATE TABLE [dbo].[CurrentDateTimes] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [time] datetime  NOT NULL
);
GO

-- Creating table 'People_Customer'
CREATE TABLE [dbo].[People_Customer] (
    [expirationDate] datetime  NULL,
    [member] bit  NOT NULL,
    [password] nvarchar(max)  NOT NULL,
    [loyaltyNum] int  NULL,
    [stays] int  NOT NULL,
    [lastStay] datetime  NULL,
    [Id] int  NOT NULL,
    [RoomPref_Id] int  NULL
);
GO

-- Creating table 'People_Staff'
CREATE TABLE [dbo].[People_Staff] (
    [password] nvarchar(max)  NOT NULL,
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

-- Creating primary key on [Id] in table 'CurrentDateTimes'
ALTER TABLE [dbo].[CurrentDateTimes]
ADD CONSTRAINT [PK_CurrentDateTimes]
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

-- Creating foreign key on [Room_Id] in table 'Reservations'
ALTER TABLE [dbo].[Reservations]
ADD CONSTRAINT [FK_RoomReservation]
    FOREIGN KEY ([Room_Id])
    REFERENCES [dbo].[Rooms]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_RoomReservation'
CREATE INDEX [IX_FK_RoomReservation]
ON [dbo].[Reservations]
    ([Room_Id]);
GO

-- Creating foreign key on [RoomPref_Id] in table 'People_Customer'
ALTER TABLE [dbo].[People_Customer]
ADD CONSTRAINT [FK_CustomerRoomType]
    FOREIGN KEY ([RoomPref_Id])
    REFERENCES [dbo].[RoomTypes]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_CustomerRoomType'
CREATE INDEX [IX_FK_CustomerRoomType]
ON [dbo].[People_Customer]
    ([RoomPref_Id]);
GO

-- Creating foreign key on [RoomTypeId] in table 'Reservations'
ALTER TABLE [dbo].[Reservations]
ADD CONSTRAINT [FK_RoomTypeReservation]
    FOREIGN KEY ([RoomTypeId])
    REFERENCES [dbo].[RoomTypes]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_RoomTypeReservation'
CREATE INDEX [IX_FK_RoomTypeReservation]
ON [dbo].[Reservations]
    ([RoomTypeId]);
GO

-- Creating foreign key on [PersonId] in table 'Reservations'
ALTER TABLE [dbo].[Reservations]
ADD CONSTRAINT [FK_PersonReservation]
    FOREIGN KEY ([PersonId])
    REFERENCES [dbo].[People]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_PersonReservation'
CREATE INDEX [IX_FK_PersonReservation]
ON [dbo].[Reservations]
    ([PersonId]);
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