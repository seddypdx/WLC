CREATE TABLE [dbo].[Races] (
    [Race ID]          INT            NOT NULL,
    [Race Name]        NVARCHAR (255) NULL,
    [Year]             INT            NULL,
    [Race Points]      NVARCHAR (50)  NULL,
    [Participants]     INT            NULL,
    [Race Boy or Girl] NVARCHAR (255) NULL,
    [Minimum Age]      INT            NULL,
    [Maximum Age]      INT            NULL,
    [sort order]       INT            NULL,
    [race order]       INT            NULL,
    [isBoating]        BIT            NOT NULL,
    [Active]           BIT            NOT NULL,
    CONSTRAINT [PK_Races] PRIMARY KEY CLUSTERED ([Race ID] ASC)
);

