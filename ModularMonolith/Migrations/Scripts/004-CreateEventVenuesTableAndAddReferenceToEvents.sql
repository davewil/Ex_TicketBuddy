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
    ADD [VenueId] [uniqueidentifier] NULL
GO

INSERT INTO [Event].[EventVenues] ([Id], [Name], [Capacity])
VALUES 
    (0, 'The O2 Arena, London', 20000),
    (1, 'Wembley Stadium, London', 90000),
    (2, 'Manchester Arena', 21000),
    (3, 'Utilita Arena, Birmingham', 15800),
    (4, 'The SSE Hydro, Glasgow', 13000),
    (5, 'First Direct Arena, Leeds', 13500),
    (6, 'Principality Stadium, Cardiff', 74500),
    (7, 'Emirates Old Trafford, Manchester', 26000),
    (8, 'Royal Albert Hall, London', 5272),
    (9, 'Roundhouse, London', 3300)
GO
