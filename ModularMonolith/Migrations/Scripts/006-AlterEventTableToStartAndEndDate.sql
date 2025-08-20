EXEC sp_rename 'Event.Events.Date', 'StartDate', 'COLUMN';

ALTER TABLE [Event].[Events]
    ADD [EndDate] [datetimeoffset](7) NULL;
GO

UPDATE [Event].[Events]
SET [EndDate] = DATEADD(DAY, 1, [StartDate])
WHERE [EndDate] IS NULL;
GO

ALTER TABLE [Event].[Events]
    ALTER COLUMN [EndDate] [datetimeoffset](7) NOT NULL;
GO