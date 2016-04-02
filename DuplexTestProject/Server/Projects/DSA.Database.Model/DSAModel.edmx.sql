
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, and Azure
-- --------------------------------------------------
-- Date Created: 01/26/2015 23:04:02
-- Generated from EDMX file: D:\c#\DTS\finalProject\DuplexTestProject\Server\Projects\DSA.Database.Model\DSAModel.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [DSA];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK_PatientIntervention]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Interventions] DROP CONSTRAINT [FK_PatientIntervention];
GO
IF OBJECT_ID(N'[dbo].[FK_InterventionLocation]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Interventions] DROP CONSTRAINT [FK_InterventionLocation];
GO
IF OBJECT_ID(N'[dbo].[FK_MaterialIntervention]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Interventions] DROP CONSTRAINT [FK_MaterialIntervention];
GO
IF OBJECT_ID(N'[dbo].[FK_InterventionDateHourDetails]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Interventions] DROP CONSTRAINT [FK_InterventionDateHourDetails];
GO
IF OBJECT_ID(N'[dbo].[FK_AreaIntervention]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Interventions] DROP CONSTRAINT [FK_AreaIntervention];
GO
IF OBJECT_ID(N'[dbo].[FK_InterventionRevenue]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Interventions] DROP CONSTRAINT [FK_InterventionRevenue];
GO
IF OBJECT_ID(N'[dbo].[FK_InterventionWork]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Interventions] DROP CONSTRAINT [FK_InterventionWork];
GO
IF OBJECT_ID(N'[dbo].[FK_YearIntervention]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Interventions] DROP CONSTRAINT [FK_YearIntervention];
GO
IF OBJECT_ID(N'[dbo].[FK_MonthIntervention]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Interventions] DROP CONSTRAINT [FK_MonthIntervention];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[Patients]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Patients];
GO
IF OBJECT_ID(N'[dbo].[Interventions]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Interventions];
GO
IF OBJECT_ID(N'[dbo].[Materials]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Materials];
GO
IF OBJECT_ID(N'[dbo].[Locations]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Locations];
GO
IF OBJECT_ID(N'[dbo].[Areas]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Areas];
GO
IF OBJECT_ID(N'[dbo].[DateHourDetails]', 'U') IS NOT NULL
    DROP TABLE [dbo].[DateHourDetails];
GO
IF OBJECT_ID(N'[dbo].[Revenues]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Revenues];
GO
IF OBJECT_ID(N'[dbo].[Works]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Works];
GO
IF OBJECT_ID(N'[dbo].[Years]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Years];
GO
IF OBJECT_ID(N'[dbo].[Months]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Months];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'Patients'
CREATE TABLE [dbo].[Patients] (
    [id] int IDENTITY(1,1) NOT NULL,
    [name] nvarchar(100)  NULL,
    [surname] nvarchar(100)  NULL,
    [Address] nvarchar(100)  NULL,
    [City] nvarchar(50)  NULL,
    [Phone] nvarchar(50)  NULL,
    [Email] nvarchar(50)  NULL
);
GO

-- Creating table 'Interventions'
CREATE TABLE [dbo].[Interventions] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Observation] nvarchar(max)  NOT NULL,
    [Number] int  NOT NULL,
    [Patient_id] int  NOT NULL,
    [Location_Id] int  NULL,
    [Material_Id] int  NULL,
    [DateHourDetail_Id] int  NOT NULL,
    [Area_Id] int  NULL,
    [Revenue_Id] int  NULL,
    [Work_Id] int  NULL,
    [Year_Id] int  NOT NULL,
    [Month_Id] int  NOT NULL
);
GO

-- Creating table 'Materials'
CREATE TABLE [dbo].[Materials] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'Locations'
CREATE TABLE [dbo].[Locations] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'Areas'
CREATE TABLE [dbo].[Areas] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'DateHourDetails'
CREATE TABLE [dbo].[DateHourDetails] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [StartHour] datetime  NOT NULL,
    [EndingHour] datetime  NOT NULL,
    [Duration] time  NOT NULL,
    [Date] datetime  NOT NULL
);
GO

-- Creating table 'Revenues'
CREATE TABLE [dbo].[Revenues] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Amount] decimal(18,0)  NOT NULL
);
GO

-- Creating table 'Works'
CREATE TABLE [dbo].[Works] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'Years'
CREATE TABLE [dbo].[Years] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [YearNb] int  NOT NULL
);
GO

-- Creating table 'Months'
CREATE TABLE [dbo].[Months] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [MOnthNumber] int  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [id] in table 'Patients'
ALTER TABLE [dbo].[Patients]
ADD CONSTRAINT [PK_Patients]
    PRIMARY KEY CLUSTERED ([id] ASC);
GO

-- Creating primary key on [Id] in table 'Interventions'
ALTER TABLE [dbo].[Interventions]
ADD CONSTRAINT [PK_Interventions]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Materials'
ALTER TABLE [dbo].[Materials]
ADD CONSTRAINT [PK_Materials]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Locations'
ALTER TABLE [dbo].[Locations]
ADD CONSTRAINT [PK_Locations]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Areas'
ALTER TABLE [dbo].[Areas]
ADD CONSTRAINT [PK_Areas]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'DateHourDetails'
ALTER TABLE [dbo].[DateHourDetails]
ADD CONSTRAINT [PK_DateHourDetails]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Revenues'
ALTER TABLE [dbo].[Revenues]
ADD CONSTRAINT [PK_Revenues]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Works'
ALTER TABLE [dbo].[Works]
ADD CONSTRAINT [PK_Works]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Years'
ALTER TABLE [dbo].[Years]
ADD CONSTRAINT [PK_Years]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Months'
ALTER TABLE [dbo].[Months]
ADD CONSTRAINT [PK_Months]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [Patient_id] in table 'Interventions'
ALTER TABLE [dbo].[Interventions]
ADD CONSTRAINT [FK_PatientIntervention]
    FOREIGN KEY ([Patient_id])
    REFERENCES [dbo].[Patients]
        ([id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_PatientIntervention'
CREATE INDEX [IX_FK_PatientIntervention]
ON [dbo].[Interventions]
    ([Patient_id]);
GO

-- Creating foreign key on [Location_Id] in table 'Interventions'
ALTER TABLE [dbo].[Interventions]
ADD CONSTRAINT [FK_InterventionLocation]
    FOREIGN KEY ([Location_Id])
    REFERENCES [dbo].[Locations]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_InterventionLocation'
CREATE INDEX [IX_FK_InterventionLocation]
ON [dbo].[Interventions]
    ([Location_Id]);
GO

-- Creating foreign key on [Material_Id] in table 'Interventions'
ALTER TABLE [dbo].[Interventions]
ADD CONSTRAINT [FK_MaterialIntervention]
    FOREIGN KEY ([Material_Id])
    REFERENCES [dbo].[Materials]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_MaterialIntervention'
CREATE INDEX [IX_FK_MaterialIntervention]
ON [dbo].[Interventions]
    ([Material_Id]);
GO

-- Creating foreign key on [DateHourDetail_Id] in table 'Interventions'
ALTER TABLE [dbo].[Interventions]
ADD CONSTRAINT [FK_InterventionDateHourDetails]
    FOREIGN KEY ([DateHourDetail_Id])
    REFERENCES [dbo].[DateHourDetails]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_InterventionDateHourDetails'
CREATE INDEX [IX_FK_InterventionDateHourDetails]
ON [dbo].[Interventions]
    ([DateHourDetail_Id]);
GO

-- Creating foreign key on [Area_Id] in table 'Interventions'
ALTER TABLE [dbo].[Interventions]
ADD CONSTRAINT [FK_AreaIntervention]
    FOREIGN KEY ([Area_Id])
    REFERENCES [dbo].[Areas]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_AreaIntervention'
CREATE INDEX [IX_FK_AreaIntervention]
ON [dbo].[Interventions]
    ([Area_Id]);
GO

-- Creating foreign key on [Revenue_Id] in table 'Interventions'
ALTER TABLE [dbo].[Interventions]
ADD CONSTRAINT [FK_InterventionRevenue]
    FOREIGN KEY ([Revenue_Id])
    REFERENCES [dbo].[Revenues]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_InterventionRevenue'
CREATE INDEX [IX_FK_InterventionRevenue]
ON [dbo].[Interventions]
    ([Revenue_Id]);
GO

-- Creating foreign key on [Work_Id] in table 'Interventions'
ALTER TABLE [dbo].[Interventions]
ADD CONSTRAINT [FK_InterventionWork]
    FOREIGN KEY ([Work_Id])
    REFERENCES [dbo].[Works]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_InterventionWork'
CREATE INDEX [IX_FK_InterventionWork]
ON [dbo].[Interventions]
    ([Work_Id]);
GO

-- Creating foreign key on [Year_Id] in table 'Interventions'
ALTER TABLE [dbo].[Interventions]
ADD CONSTRAINT [FK_YearIntervention]
    FOREIGN KEY ([Year_Id])
    REFERENCES [dbo].[Years]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_YearIntervention'
CREATE INDEX [IX_FK_YearIntervention]
ON [dbo].[Interventions]
    ([Year_Id]);
GO

-- Creating foreign key on [Month_Id] in table 'Interventions'
ALTER TABLE [dbo].[Interventions]
ADD CONSTRAINT [FK_MonthIntervention]
    FOREIGN KEY ([Month_Id])
    REFERENCES [dbo].[Months]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_MonthIntervention'
CREATE INDEX [IX_FK_MonthIntervention]
ON [dbo].[Interventions]
    ([Month_Id]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------