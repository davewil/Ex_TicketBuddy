CREATE TABLE [dbo].[InboxState](
    [Id] [bigint] IDENTITY(1,1) NOT NULL,
    [MessageId] [uniqueidentifier] NOT NULL,
    [ConsumerId] [uniqueidentifier] NOT NULL,
    [LockId] [uniqueidentifier] NOT NULL,
    [RowVersion] [timestamp] NULL,
    [Received] [datetime2](7) NOT NULL,
    [ReceiveCount] [int] NOT NULL,
    [ExpirationTime] [datetime2](7) NULL,
    [Consumed] [datetime2](7) NULL,
    [Delivered] [datetime2](7) NULL,
    [LastSequenceNumber] [bigint] NULL,
    CONSTRAINT [PK_InboxState] PRIMARY KEY CLUSTERED
(
[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
    CONSTRAINT [AK_InboxState_MessageId_ConsumerId] UNIQUE NONCLUSTERED
(
    [MessageId] ASC,
[ConsumerId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
    ) ON [PRIMARY]
GO


CREATE TABLE [dbo].[OutboxMessage](
    [SequenceNumber] [bigint] IDENTITY(1,1) NOT NULL,
    [EnqueueTime] [datetime2](7) NULL,
    [SentTime] [datetime2](7) NOT NULL,
    [Headers] [nvarchar](max) NULL,
    [Properties] [nvarchar](max) NULL,
    [InboxMessageId] [uniqueidentifier] NULL,
    [InboxConsumerId] [uniqueidentifier] NULL,
    [OutboxId] [uniqueidentifier] NULL,
    [MessageId] [uniqueidentifier] NOT NULL,
    [ContentType] [nvarchar](256) NOT NULL,
    [MessageType] [nvarchar](max) NOT NULL,
    [Body] [nvarchar](max) NOT NULL,
    [ConversationId] [uniqueidentifier] NULL,
    [CorrelationId] [uniqueidentifier] NULL,
    [InitiatorId] [uniqueidentifier] NULL,
    [RequestId] [uniqueidentifier] NULL,
    [SourceAddress] [nvarchar](256) NULL,
    [DestinationAddress] [nvarchar](256) NULL,
    [ResponseAddress] [nvarchar](256) NULL,
    [FaultAddress] [nvarchar](256) NULL,
    [ExpirationTime] [datetime2](7) NULL,
    CONSTRAINT [PK_OutboxMessage] PRIMARY KEY CLUSTERED
(
[SequenceNumber] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
    ) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
    GO

CREATE TABLE [dbo].[OutboxState](
    [OutboxId] [uniqueidentifier] NOT NULL,
    [LockId] [uniqueidentifier] NOT NULL,
    [RowVersion] [timestamp] NULL,
    [Created] [datetime2](7) NOT NULL,
    [Delivered] [datetime2](7) NULL,
    [LastSequenceNumber] [bigint] NULL,
    CONSTRAINT [PK_OutboxState] PRIMARY KEY CLUSTERED
(
[OutboxId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
    ) ON [PRIMARY]
GO