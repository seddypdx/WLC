CREATE TABLE [dbo].[Notices] (
    [NoticeId]       INT            IDENTITY (1, 1) NOT NULL,
    [NoticeTypeId]   INT            NOT NULL,
    [Message]        NVARCHAR (MAX) NULL,
    [DateCreated]    SMALLDATETIME  NOT NULL,
    [DateToSend]     SMALLDATETIME  NOT NULL,
    [NoticeStatusId] INT            NOT NULL,
    CONSTRAINT [PK_Notices] PRIMARY KEY CLUSTERED ([NoticeId] ASC),
    CONSTRAINT [FK_Notices_NoticeStatuses] FOREIGN KEY ([NoticeStatusId]) REFERENCES [dbo].[NoticeStatuses] ([NoticeStatusId]),
    CONSTRAINT [FK_Notices_NoticeTypes] FOREIGN KEY ([NoticeTypeId]) REFERENCES [dbo].[NoticeTypes] ([NoticeTypeId])
);

