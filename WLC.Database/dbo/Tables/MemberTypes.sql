CREATE TABLE [dbo].[MemberTypes] (
    [MemberTypeID]          INT           NOT NULL,
    [MemberType]            NVARCHAR (50) NULL,
    [MemberDues]            MONEY         NULL,
    [Active]                BIT           NOT NULL,
    [Category]              NVARCHAR (25) NULL,
    [MembershipType]        BIT           NOT NULL,
    [Family]                BIT           NOT NULL,
    [RosterSortOrder]       SMALLINT      NULL,
    [RosterFamilySortOrder] SMALLINT      NULL,
    CONSTRAINT [PK_MemberTypes] PRIMARY KEY CLUSTERED ([MemberTypeID] ASC)
);

