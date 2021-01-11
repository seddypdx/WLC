CREATE TABLE [dbo].[Years] (
    [Year]         INT            NOT NULL,
    [LaborDayDate] DATETIME       NULL,
    [Notes]        NVARCHAR (MAX) NULL,
    CONSTRAINT [PK_Years] PRIMARY KEY CLUSTERED ([Year] ASC)
);

