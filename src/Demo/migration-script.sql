IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;

GO

CREATE TABLE [Players] (
    [Id] int NOT NULL IDENTITY,
    [Name] nvarchar(max) NULL,
    [NickName] nvarchar(max) NULL,
    [Hp] int NOT NULL,
    [Mp] int NOT NULL,
    [Attack] int NOT NULL,
    [Defense] int NOT NULL,
    [MagicAttack] int NOT NULL,
    [MagicDefense] int NOT NULL,
    [Level] int NOT NULL,
    [CorrelationId] int NOT NULL,
    CONSTRAINT [PK_Players] PRIMARY KEY ([Id])
);

GO

CREATE TABLE [Skills] (
    [Id] int NOT NULL IDENTITY,
    [PlayerId] int NOT NULL,
    [Name] nvarchar(max) NULL,
    [Cost] int NOT NULL,
    [Cooldown] decimal(18,2) NOT NULL,
    [Level] int NOT NULL,
    [IsLearned] bit NOT NULL,
    [CorrelationId] int NOT NULL,
    CONSTRAINT [PK_Skills] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Skills_Players_PlayerId] FOREIGN KEY ([PlayerId]) REFERENCES [Players] ([Id]) ON DELETE CASCADE
);

GO

CREATE INDEX [IX_Skills_PlayerId] ON [Skills] ([PlayerId]);

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20200526064642_InitialCreate', N'3.1.4');

GO

