CREATE TABLE [dbo].[Racers] (
    [Racer ID]       INT            IDENTITY (1, 1) NOT NULL,
    [First Name]     NVARCHAR (50)  NULL,
    [Last Name]      NVARCHAR (50)  NULL,
    [Boy or Girl]    NVARCHAR (255) NULL,
    [Age]            INT            NOT NULL,
    [Birthdate]      DATETIME       NULL,
    [MemberStatusId] INT            NOT NULL,
    [Cabin ID]       INT            NOT NULL,
    CONSTRAINT [PK_Racers] PRIMARY KEY CLUSTERED ([Racer ID] ASC),
    CONSTRAINT [FK_Racers_cabins] FOREIGN KEY ([Cabin ID]) REFERENCES [dbo].[Cabins] ([Cabin ID]),
    CONSTRAINT [FK_Racers_MemberStatus] FOREIGN KEY ([MemberStatusId]) REFERENCES [dbo].[MemberStatuses] ([MemberStatusId])
);

