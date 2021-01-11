CREATE TABLE [dbo].[NoticeTypes] (
    [NoticeTypeId] INT           NOT NULL,
    [Description]  NVARCHAR (20) NOT NULL,
    CONSTRAINT [PK_NoticeTypes] PRIMARY KEY CLUSTERED ([NoticeTypeId] ASC)
);

