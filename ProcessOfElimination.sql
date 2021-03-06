drop table PlayerActions;
drop table ChatMessages;
drop table GameTurns;
drop table GameCards;
drop table Cards;
drop table GamePlayers;
drop table Teams;
drop table Games;



CREATE TABLE Games(
	[ID] int IDENTITY(1,1) NOT NULL,
	[Name] nvarchar(50) not null,
	[Password] varchar(32) null default null,
	[HasStarted] bit not null default 0,
	[HasFinished] bit not null default 0,
	[NumPlayers] int not null,
	[HostedByUserID] nvarchar(128) null default null,
	[CreatedOn] datetime not null default current_timestamp,
 CONSTRAINT [PK_Games] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]


CREATE TABLE Teams(
	[ID] int IDENTITY(1,1) NOT NULL,
	[Name] nvarchar(20) not null,
 CONSTRAINT [PK_Teams] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]


CREATE TABLE GamePlayers(
	[ID] int IDENTITY(1,1) NOT NULL,
	[GameID] int not null,
	[Name] nvarchar(50) not null,
	[UserID] nvarchar(128) not null,
	[Position] int not null,
	[PublicTeamID] int not null,
	[PrivateTeamID] int not null,
	[Notes] nvarchar(MAX) not null default '',
CONSTRAINT [PK_GamePlayers] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]


CREATE TABLE Cards(
	[ID] int IDENTITY(1,1) NOT NULL,
	[Name] nvarchar(50) not null,
	[Value] int not null default 1,
	[TeamID] int not null,
	[IsEvent] bit not null,
	[NumberPerGame] int not null default 1,
	[Description] nvarchar(MAX) not null,
 CONSTRAINT [PK_Cards] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]


CREATE TABLE GameCards(
	[ID] int IDENTITY(1,1) NOT NULL,
	[GameID] int not null,
	[CardID] int not null,
	[GamePlayerID] int null,
	[Position] int not null,
	[Discarded] bit not null default 0,
 CONSTRAINT [PK_GameCards] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]


CREATE TABLE GameTurns(
	[ID] int IDENTITY(1,1) NOT NULL,
	[GameID] int not null,
	[ActivePlayerID] int not null,
	[EventCardID] int not null,
	[Number] int not null,
	[Timestamp] datetime not null default current_timestamp,
	[Message] varchar(MAX),
 CONSTRAINT [PK_GameTurns] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]


CREATE TABLE ChatMessages(
	ID int IDENTITY(1,1) not null,
	GameTurnID int not null,
	GamePlayerID int not null,
	Timestamp datetime not null default current_timestamp,
	Message varchar(MAX) not null,
 CONSTRAINT [PK_ChatMessages] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]


CREATE TABLE PlayerActions(
	ID int IDENTITY(1,1) not null,
	GameTurnID int not null,
	GamePlayerID int not null,
	GameCardID int not null,
	[Order] int not null,
 CONSTRAINT [PK_PlayerActions] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]






CREATE NONCLUSTERED INDEX [IX_Games_HasStarted] ON Games
(
	[HasStarted] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO

CREATE NONCLUSTERED INDEX [IX_Games_HasFinished] ON Games
(
	[HasFinished] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO

CREATE NONCLUSTERED INDEX [IX_GamePlayers_GameID_Name] ON GamePlayers
(
	[GameID] ASC,
	[Name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO

CREATE NONCLUSTERED INDEX [IX_GamePlayers_UserID] ON GamePlayers
(
	[UserID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO

CREATE NONCLUSTERED INDEX [IX_Cards_IsEvent] ON Cards
(
	[IsEvent] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO

CREATE NONCLUSTERED INDEX [IX_GameCards_GameID] ON GameCards
(
	[GameID] ASC,
	[Discarded] ASC,
	[Position] DESC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO

CREATE NONCLUSTERED INDEX [IX_GameCards_CardID] ON GameCards
(
	[CardID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO

CREATE NONCLUSTERED INDEX [IX_GameCards_GamePlayerID] ON GameCards
(
	[GamePlayerID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO

CREATE NONCLUSTERED INDEX [IX_GameTurns_GameID_Number] ON GameTurns
(
	[GameID] ASC,
	[Number]
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO

CREATE NONCLUSTERED INDEX [IX_ChatMessages_GameTurnID_Timestamp] ON ChatMessages
(
	[GameTurnID] ASC,
	[Timestamp]
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO

CREATE NONCLUSTERED INDEX [IX_ChatMessages_GamePlayerID_Timestamp] ON ChatMessages
(
	[GamePlayerID] ASC,
	[Timestamp]
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO

CREATE NONCLUSTERED INDEX [IX_PlayerActions_GameTurnID_Order] ON PlayerActions
(
	[GameTurnID] ASC,
	[Order] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO

CREATE NONCLUSTERED INDEX [IX_PlayerActions_GamePlayerID] ON PlayerActions
(
	[GamePlayerID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO

CREATE NONCLUSTERED INDEX [IX_PlayerActions_GameCardID] ON PlayerActions
(
	[GameCardID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO



ALTER TABLE Games WITH CHECK ADD  CONSTRAINT [FK_Games_AspNetUsers] FOREIGN KEY([HostedByUserID])
REFERENCES [AspNetUsers] ([ID])
GO

ALTER TABLE GamePlayers WITH CHECK ADD  CONSTRAINT [FK_GamePlayers_Games] FOREIGN KEY([GameID])
REFERENCES [Games] ([ID])
GO

ALTER TABLE GamePlayers WITH CHECK ADD  CONSTRAINT [FK_GamePlayers_AspNetUsers] FOREIGN KEY([UserID])
REFERENCES [AspNetUsers] ([ID])
GO

ALTER TABLE GamePlayers WITH CHECK ADD  CONSTRAINT [FK_GamePlayers_Teams_Public] FOREIGN KEY([PublicTeamID])
REFERENCES [Teams] ([ID])
GO

ALTER TABLE GamePlayers WITH CHECK ADD  CONSTRAINT [FK_GamePlayers_Teams_Private] FOREIGN KEY([PrivateTeamID])
REFERENCES [Teams] ([ID])
GO

ALTER TABLE GameCards WITH CHECK ADD  CONSTRAINT [FK_GameCards_Games] FOREIGN KEY([GameID])
REFERENCES [Games] ([ID])
GO

ALTER TABLE GameCards WITH CHECK ADD  CONSTRAINT [FK_GameCards_Cards] FOREIGN KEY([CardID])
REFERENCES [Cards] ([ID])
GO

ALTER TABLE Cards WITH CHECK ADD  CONSTRAINT [FK_Cards_Teams] FOREIGN KEY([TeamID])
REFERENCES [Teams] ([ID])
GO

ALTER TABLE GameCards WITH CHECK ADD  CONSTRAINT [FK_Cards_Players] FOREIGN KEY([GamePlayerID])
REFERENCES [GamePlayers] ([ID])
GO

ALTER TABLE GameTurns WITH CHECK ADD  CONSTRAINT [FK_GameTurns_Games] FOREIGN KEY([GameID])
REFERENCES [Games] ([ID])
GO

ALTER TABLE GameTurns WITH CHECK ADD  CONSTRAINT [FK_GameTurns_GameCards] FOREIGN KEY([EventCardID])
REFERENCES [GameCards] ([ID])
GO

ALTER TABLE GameTurns WITH CHECK ADD  CONSTRAINT [FK_GameTurns_GamePlayers] FOREIGN KEY([ActivePlayerID])
REFERENCES [GamePlayers] ([ID])
GO

ALTER TABLE ChatMessages WITH CHECK ADD  CONSTRAINT [FK_ChatMessages_GameTurns] FOREIGN KEY([GameTurnID])
REFERENCES [GameTurns] ([ID])
GO

ALTER TABLE ChatMessages WITH CHECK ADD  CONSTRAINT [FK_ChatMessages_GamePlayers] FOREIGN KEY([GamePlayerID])
REFERENCES [GamePlayers] ([ID])
GO

ALTER TABLE PlayerActions WITH CHECK ADD  CONSTRAINT [FK_PlayerActions_GameTurns] FOREIGN KEY([GameTurnID])
REFERENCES [GameTurns] ([ID])
GO

ALTER TABLE PlayerActions WITH CHECK ADD  CONSTRAINT [FK_PlayerActions_GamePlayers] FOREIGN KEY([GamePlayerID])
REFERENCES [GamePlayers] ([ID])
GO

ALTER TABLE PlayerActions WITH CHECK ADD  CONSTRAINT [FK_PlayerActions_GameCards] FOREIGN KEY([GameCardID])
REFERENCES [GameCards] ([ID])
GO


insert into Teams select 'Human'
insert into Teams select 'Alien'

insert into Games select 'Test public game', null, 0, 0, 4, 'b5b9cfcb-6562-4d20-b787-810fa3fa2289', current_timestamp
insert into gameplayers select 1, 'Alice', '6d643b42-2d15-4fc3-9a50-0efdaa43785c', 0, 1, 1, ''
insert into gameplayers select 1, 'Bob', 'bf9a9ee6-686d-4696-a835-b951fe7207a3', 0, 1, 1, ''
insert into gameplayers select 1, 'Carly', 'f9319238-c1d1-4439-b5ef-4d760f4f6da3', 0, 1, 1, ''


insert into cards select 'Event #1', 1, 1, 1, 1, 'Only one of this event card'
insert into cards select 'Event #2', 2, 1, 1, 1, 'Only one of this event card'
insert into cards select 'Event #3', 3, 1, 1, 3, 'Three of this event card'
insert into cards select 'Event #4', 4, 1, 1, 1, 'Only one of this event card'
insert into cards select 'Player Card #1', 1, 1, 0, 5, 'A human card'
insert into cards select 'Player Card #2', 1, 1, 0, 5, 'A human card'
insert into cards select 'Player Card #3', 2, 1, 0, 5, 'A human card'
insert into cards select 'Player Card #4', 2, 1, 0, 5, 'A human card'
insert into cards select 'Player Card #5', 3, 1, 0, 1, 'A rare human card'
insert into cards select 'Player Card #6', 1, 2, 0, 4, 'An alien card'
insert into cards select 'Player Card #7', 2, 2, 0, 3, 'An alien card'
insert into cards select 'Player Card #8', 3, 2, 0, 3, 'A rare alien card'