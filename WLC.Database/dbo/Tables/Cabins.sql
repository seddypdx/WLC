CREATE TABLE [dbo].[Cabins] (
    [Cabin ID]    INT           NOT NULL,
    [Cabin Name]  NVARCHAR (30) NULL,
    [Cabin Phone] NVARCHAR (15) NULL,
    CONSTRAINT [PK_Cabins] PRIMARY KEY CLUSTERED ([Cabin ID] ASC)
);

