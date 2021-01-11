CREATE TABLE [dbo].[MemberStatuses] (
    [MemberStatusId] INT           IDENTITY (1, 1) NOT NULL,
    [MemberStatus]   NVARCHAR (20) NOT NULL,
    CONSTRAINT [PK_MemberStatuses] PRIMARY KEY CLUSTERED ([MemberStatusId] ASC)
);

