CREATE TABLE [Event].[EventVenues](
    [Id] [int] NOT NULL,
    [Name] [nvarchar](max) NOT NULL,
    [Capacity] [int] NOT NULL,
    CONSTRAINT [PK_EventVenues] PRIMARY KEY CLUSTERED
    (
        [Id] ASC
    )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [Event].[Events]
    ADD [Venue] [int] NOT NULL
GO

INSERT INTO [Event].[EventVenues] ([Id], [Name], [Capacity])
VALUES 
    (0, 'The O2 Arena, London', 10),
    (1, 'Wembley Stadium, London', 11),
    (2, 'Manchester Arena', 12),
    (3, 'Utilita Arena, Birmingham', 13),
    (4, 'The SSE Hydro, Glasgow', 14),
    (5, 'First Direct Arena, Leeds', 15),
    (6, 'Principality Stadium, Cardiff', 16),
    (7, 'Emirates Old Trafford, Manchester', 17),
    (8, 'Royal Albert Hall, London', 18),
    (9, 'Roundhouse, London', 20)
GO
