CREATE TABLE [dbo].[NoticeStatuses] (
    [NoticeStatusId] INT           NOT NULL,
    [Description]    NVARCHAR (20) NOT NULL,
    CONSTRAINT [PK_NoticeStatuses] PRIMARY KEY CLUSTERED ([NoticeStatusId] ASC)
);

