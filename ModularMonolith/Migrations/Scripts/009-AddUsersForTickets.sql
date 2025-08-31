CREATE TABLE [Ticket].[Users](
    [Id] [uniqueidentifier] NOT NULL,
    [FullName] [nvarchar](max) NOT NULL,
    [Email] [nvarchar](max) NOT NULL,
    CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED
(
[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
    ) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [Ticket].[Tickets]
    ADD [UserId] [uniqueidentifier] NULL;
GO

ALTER TABLE [Ticket].[Tickets]
    ADD [PurchasedAt] [datetimeoffset](7) NULL;
GO