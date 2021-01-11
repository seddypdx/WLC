﻿CREATE TABLE [dbo].[Members] (
    [MemberID]                   INT            NOT NULL,
    [FirstName]                  NVARCHAR (20)  NULL,
    [LastName]                   NVARCHAR (20)  NULL,
    [Middle]                     NVARCHAR (10)  NULL,
    [Name Tag]                   NVARCHAR (255) NULL,
    [NickName]                   NVARCHAR (10)  NULL,
    [Suffix]                     NVARCHAR (10)  NULL,
    [Title]                      NVARCHAR (10)  NULL,
    [MaleFemale]                 NVARCHAR (10)  NULL,
    [DateJoined]                 INT            NULL,
    [MemberTypeID]               INT            NULL,
    [PrimaryMemberID]            INT            NULL,
    [SecondaryMemberID]          INT            NULL,
    [Sign-InBadge]               BIT            NOT NULL,
    [ExProprietaryMember]        BIT            NOT NULL,
    [edit]                       NVARCHAR (50)  NULL,
    [Married]                    BIT            NOT NULL,
    [Birthdate]                  DATETIME       NULL,
    [Deceased]                   BIT            NOT NULL,
    [Date]                       DATETIME       NULL,
    [HomeAddress]                NVARCHAR (100) NULL,
    [HomeCity]                   NVARCHAR (20)  NULL,
    [HomeStateOrProvince]        NVARCHAR (20)  NULL,
    [HomePostalCode]             NVARCHAR (20)  NULL,
    [HomeCountry]                NVARCHAR (20)  NULL,
    [HomePhone]                  NVARCHAR (30)  NULL,
    [HomeFax]                    NVARCHAR (30)  NULL,
    [HomeCell]                   NVARCHAR (30)  NULL,
    [EmailName]                  NVARCHAR (50)  NULL,
    [Occupation]                 NVARCHAR (50)  NULL,
    [WorkCompany]                NVARCHAR (30)  NULL,
    [WorkAddress]                NVARCHAR (100) NULL,
    [WorkCity]                   NVARCHAR (20)  NULL,
    [WorkStateOrProvince]        NVARCHAR (20)  NULL,
    [WorkPostalCode]             NVARCHAR (20)  NULL,
    [WorkCountry]                NVARCHAR (20)  NULL,
    [WorkPhone]                  NVARCHAR (30)  NULL,
    [Extension]                  NVARCHAR (10)  NULL,
    [WorkFax]                    NVARCHAR (30)  NULL,
    [WorkCell]                   NVARCHAR (30)  NULL,
    [WorkEmail]                  NVARCHAR (50)  NULL,
    [Father]                     INT            NULL,
    [Mother]                     INT            NULL,
    [Spouse]                     INT            NULL,
    [HomeAddress2]               NVARCHAR (100) NULL,
    [HomeCity2]                  NVARCHAR (20)  NULL,
    [HomeStateOrProvince2]       NVARCHAR (20)  NULL,
    [HomePostalCode2]            NVARCHAR (20)  NULL,
    [HomeCountry2]               NVARCHAR (20)  NULL,
    [MailToAddress1]             BIT            NOT NULL,
    [MailToAddress2]             BIT            NOT NULL,
    [Cabin]                      INT            NULL,
    [QualifiedAssociateThisYear] BIT            NOT NULL,
    [Active]                     BIT            NOT NULL,
    [EmailReports]               BIT            NOT NULL,
    [AspNetUserId]               NVARCHAR (450) NULL,
    [NotifyOnEmergency]          BIT            CONSTRAINT [DF_Members_NotifyOnEmergency] DEFAULT ((0)) NOT NULL,
    [NotifyOnInformation]        BIT            CONSTRAINT [DF_Members_NotifyOnInformation] DEFAULT ((0)) NOT NULL,
    [NotifyOnSocial]             BIT            CONSTRAINT [DF_Members_NotifyOnSocial] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_Members] PRIMARY KEY CLUSTERED ([MemberID] ASC),
    CONSTRAINT [FK_Members_AspNetUsers] FOREIGN KEY ([AspNetUserId]) REFERENCES [dbo].[AspNetUsers] ([Id])
);
