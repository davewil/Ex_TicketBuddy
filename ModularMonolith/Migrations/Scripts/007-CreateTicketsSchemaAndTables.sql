IF NOT EXISTS ( SELECT * FROM sys.schemas WHERE name = N'Ticket' )
   EXEC('CREATE SCHEMA [Ticket]');
GO

CREATE TABLE [Ticket].[EventVenues](
    [Id] [int] NOT NULL,
    [Name] [nvarchar](max) NOT NULL,
    [Capacity] [int] NOT NULL,
    CONSTRAINT [PK_EventVenues] PRIMARY KEY CLUSTERED
(
[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
    ) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

INSERT INTO [Ticket].[EventVenues] ([Id], [Name], [Capacity])
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

CREATE TABLE [Ticket].[Events](
    [Id] [uniqueidentifier] NOT NULL,
    [EventName] [nvarchar](max) NOT NULL,
    [StartDate] [datetimeoffset](7) NULL,
    [EndDate] [datetimeoffset](7) NULL,
    [Venue] [int] NOT NULL
    CONSTRAINT [PK_Events] PRIMARY KEY CLUSTERED
(
[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
    ) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO