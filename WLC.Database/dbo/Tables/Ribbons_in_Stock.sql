CREATE TABLE [dbo].[Ribbons_in_Stock] (
    [RibonId]         INT           NOT NULL,
    [Ribbon]          NVARCHAR (50) NULL,
    [Number_in_stock] INT           NULL,
    CONSTRAINT [PK_Ribbons_in_Stock] PRIMARY KEY CLUSTERED ([RibonId] ASC)
);

