CREATE TABLE [dbo].[NoticeQueueItems] (
    [NoticeQueueItemId]    INT           IDENTITY (1, 1) NOT NULL,
    [NoticeId]             INT           NOT NULL,
    [MessageId]            INT           NOT NULL,
    [NoticeStatusId]       INT           NOT NULL,
    [DateLastChanged]      SMALLDATETIME NOT NULL,
    [NotificationLocation] NVARCHAR (50) NULL,
    CONSTRAINT [PK_NoticeQueueItems] PRIMARY KEY CLUSTERED ([NoticeQueueItemId] ASC),
    CONSTRAINT [FK_NoticeQueueItems_Notices] FOREIGN KEY ([NoticeId]) REFERENCES [dbo].[Notices] ([NoticeId]),
    CONSTRAINT [FK_NoticeQueueItems_NoticeStatuses] FOREIGN KEY ([NoticeStatusId]) REFERENCES [dbo].[NoticeStatuses] ([NoticeStatusId])
);

