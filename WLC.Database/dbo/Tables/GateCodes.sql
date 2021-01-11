CREATE TABLE [dbo].[GateCodes] (
    [GatecodeId] INT           IDENTITY (1, 1) NOT NULL,
    [LastName]   VARCHAR (50)  NOT NULL,
    [FirstName]  VARCHAR (50)  NOT NULL,
    [Code]       NVARCHAR (20) NOT NULL,
    [CabinId]    INT           NULL,
    CONSTRAINT [PK_GateCodes] PRIMARY KEY CLUSTERED ([GatecodeId] ASC)
);

