CREATE TABLE [dbo].[Results] (
    [Result ID]    INT           IDENTITY (1, 1) NOT NULL,
    [Year]         INT           CONSTRAINT [DF_Results_Year] DEFAULT ((2004)) NOT NULL,
    [Race ID]      INT           NULL,
    [Racer ID]     INT           NULL,
    [Team ID]      INT           NULL,
    [Place]        INT           NULL,
    [Points Place] INT           NULL,
    [Points]       FLOAT (53)    NULL,
    [Ribbon]       NVARCHAR (30) NULL,
    [Comments]     NVARCHAR (50) NULL,
    CONSTRAINT [PK_Results] PRIMARY KEY CLUSTERED ([Result ID] ASC),
    CONSTRAINT [FK_Results_Race] FOREIGN KEY ([Race ID]) REFERENCES [dbo].[Races] ([Race ID]),
    CONSTRAINT [FK_Results_Racer] FOREIGN KEY ([Racer ID]) REFERENCES [dbo].[Racers] ([Racer ID]),
    CONSTRAINT [FK_Results_Year] FOREIGN KEY ([Year]) REFERENCES [dbo].[Years] ([Year])
);

