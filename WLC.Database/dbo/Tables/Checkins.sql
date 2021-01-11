CREATE TABLE [dbo].[Checkins] (
    [CheckinId]    INT            IDENTITY (1, 1) NOT NULL,
    [Member ID]    INT            NOT NULL,
    [Note]         VARCHAR (8000) NULL,
    [CheckInTime]  DATETIME2 (7)  NOT NULL,
    [CheckOutTime] DATETIME2 (7)  NULL,
    CONSTRAINT [PK__Checkins__F3C85D719D16BEC9] PRIMARY KEY CLUSTERED ([CheckinId] ASC),
    CONSTRAINT [FK__Checkins__Member__42E1EEFE] FOREIGN KEY ([Member ID]) REFERENCES [dbo].[Members] ([MemberID])
);

