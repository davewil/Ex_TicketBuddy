ALTER TABLE [Event].[Events]
    ADD [Price] [decimal](18, 2) NULL;
GO

ALTER TABLE [Ticket].[Events]
    ADD [Price] [decimal](18, 2) NULL;
GO