ALTER TABLE [Event].[Events]
    ADD [Date] [datetimeoffset](7) NOT NULL
    CONSTRAINT DF_Events_Date DEFAULT (SYSDATETIMEOFFSET());
GO