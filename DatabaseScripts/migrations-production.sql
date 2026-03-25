IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251219082344_InitialCreate'
)
BEGIN
    CREATE TABLE [AspNetRoles] (
        [Id] nvarchar(450) NOT NULL,
        [Name] nvarchar(256) NULL,
        [NormalizedName] nvarchar(256) NULL,
        [ConcurrencyStamp] nvarchar(max) NULL,
        CONSTRAINT [PK_AspNetRoles] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251219082344_InitialCreate'
)
BEGIN
    CREATE TABLE [AspNetUsers] (
        [Id] nvarchar(450) NOT NULL,
        [FullName] nvarchar(max) NULL,
        [IsApproved] bit NOT NULL,
        [MustChangePassword] bit NOT NULL,
        [UserName] nvarchar(256) NULL,
        [NormalizedUserName] nvarchar(256) NULL,
        [Email] nvarchar(256) NULL,
        [NormalizedEmail] nvarchar(256) NULL,
        [EmailConfirmed] bit NOT NULL,
        [PasswordHash] nvarchar(max) NULL,
        [SecurityStamp] nvarchar(max) NULL,
        [ConcurrencyStamp] nvarchar(max) NULL,
        [PhoneNumber] nvarchar(max) NULL,
        [PhoneNumberConfirmed] bit NOT NULL,
        [TwoFactorEnabled] bit NOT NULL,
        [LockoutEnd] datetimeoffset NULL,
        [LockoutEnabled] bit NOT NULL,
        [AccessFailedCount] int NOT NULL,
        CONSTRAINT [PK_AspNetUsers] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251219082344_InitialCreate'
)
BEGIN
    CREATE TABLE [Courses] (
        [Id] int NOT NULL IDENTITY,
        [Code] nvarchar(max) NULL,
        [Title] nvarchar(max) NULL,
        [CreditHours] int NOT NULL,
        CONSTRAINT [PK_Courses] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251219082344_InitialCreate'
)
BEGIN
    CREATE TABLE [Sections] (
        [Id] int NOT NULL IDENTITY,
        [Name] nvarchar(max) NULL,
        CONSTRAINT [PK_Sections] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251219082344_InitialCreate'
)
BEGIN
    CREATE TABLE [Sessions] (
        [Id] int NOT NULL IDENTITY,
        [Name] nvarchar(max) NULL,
        [StartDate] datetime2 NOT NULL,
        [EndDate] datetime2 NOT NULL,
        CONSTRAINT [PK_Sessions] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251219082344_InitialCreate'
)
BEGIN
    CREATE TABLE [AspNetRoleClaims] (
        [Id] int NOT NULL IDENTITY,
        [RoleId] nvarchar(450) NOT NULL,
        [ClaimType] nvarchar(max) NULL,
        [ClaimValue] nvarchar(max) NULL,
        CONSTRAINT [PK_AspNetRoleClaims] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_AspNetRoleClaims_AspNetRoles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [AspNetRoles] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251219082344_InitialCreate'
)
BEGIN
    CREATE TABLE [AspNetUserClaims] (
        [Id] int NOT NULL IDENTITY,
        [UserId] nvarchar(450) NOT NULL,
        [ClaimType] nvarchar(max) NULL,
        [ClaimValue] nvarchar(max) NULL,
        CONSTRAINT [PK_AspNetUserClaims] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_AspNetUserClaims_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251219082344_InitialCreate'
)
BEGIN
    CREATE TABLE [AspNetUserLogins] (
        [LoginProvider] nvarchar(450) NOT NULL,
        [ProviderKey] nvarchar(450) NOT NULL,
        [ProviderDisplayName] nvarchar(max) NULL,
        [UserId] nvarchar(450) NOT NULL,
        CONSTRAINT [PK_AspNetUserLogins] PRIMARY KEY ([LoginProvider], [ProviderKey]),
        CONSTRAINT [FK_AspNetUserLogins_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251219082344_InitialCreate'
)
BEGIN
    CREATE TABLE [AspNetUserRoles] (
        [UserId] nvarchar(450) NOT NULL,
        [RoleId] nvarchar(450) NOT NULL,
        CONSTRAINT [PK_AspNetUserRoles] PRIMARY KEY ([UserId], [RoleId]),
        CONSTRAINT [FK_AspNetUserRoles_AspNetRoles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [AspNetRoles] ([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_AspNetUserRoles_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251219082344_InitialCreate'
)
BEGIN
    CREATE TABLE [AspNetUserTokens] (
        [UserId] nvarchar(450) NOT NULL,
        [LoginProvider] nvarchar(450) NOT NULL,
        [Name] nvarchar(450) NOT NULL,
        [Value] nvarchar(max) NULL,
        CONSTRAINT [PK_AspNetUserTokens] PRIMARY KEY ([UserId], [LoginProvider], [Name]),
        CONSTRAINT [FK_AspNetUserTokens_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251219082344_InitialCreate'
)
BEGIN
    CREATE TABLE [TeacherProfiles] (
        [Id] int NOT NULL IDENTITY,
        [EmployeeId] nvarchar(max) NULL,
        [ApplicationUserId] nvarchar(450) NULL,
        CONSTRAINT [PK_TeacherProfiles] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_TeacherProfiles_AspNetUsers_ApplicationUserId] FOREIGN KEY ([ApplicationUserId]) REFERENCES [AspNetUsers] ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251219082344_InitialCreate'
)
BEGIN
    CREATE TABLE [StudentProfiles] (
        [Id] int NOT NULL IDENTITY,
        [ApplicationUserId] nvarchar(450) NULL,
        [RollNumber] int NOT NULL,
        [FullName] nvarchar(max) NULL,
        [SectionId] int NOT NULL,
        CONSTRAINT [PK_StudentProfiles] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_StudentProfiles_AspNetUsers_ApplicationUserId] FOREIGN KEY ([ApplicationUserId]) REFERENCES [AspNetUsers] ([Id]),
        CONSTRAINT [FK_StudentProfiles_Sections_SectionId] FOREIGN KEY ([SectionId]) REFERENCES [Sections] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251219082344_InitialCreate'
)
BEGIN
    CREATE TABLE [CourseAssignments] (
        [Id] int NOT NULL IDENTITY,
        [TeacherProfileId] int NOT NULL,
        [CourseId] int NOT NULL,
        [SessionId] int NOT NULL,
        [SectionId] int NOT NULL,
        CONSTRAINT [PK_CourseAssignments] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_CourseAssignments_Courses_CourseId] FOREIGN KEY ([CourseId]) REFERENCES [Courses] ([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_CourseAssignments_TeacherProfiles_TeacherProfileId] FOREIGN KEY ([TeacherProfileId]) REFERENCES [TeacherProfiles] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251219082344_InitialCreate'
)
BEGIN
    CREATE TABLE [CourseSchedules] (
        [Id] int NOT NULL IDENTITY,
        [CourseId] int NOT NULL,
        [TeacherProfileId] int NOT NULL,
        [SectionId] int NOT NULL,
        [DayOfWeek] int NOT NULL,
        [StartTime] time NOT NULL,
        [EndTime] time NOT NULL,
        [SessionId] int NOT NULL,
        CONSTRAINT [PK_CourseSchedules] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_CourseSchedules_Courses_CourseId] FOREIGN KEY ([CourseId]) REFERENCES [Courses] ([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_CourseSchedules_Sections_SectionId] FOREIGN KEY ([SectionId]) REFERENCES [Sections] ([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_CourseSchedules_TeacherProfiles_TeacherProfileId] FOREIGN KEY ([TeacherProfileId]) REFERENCES [TeacherProfiles] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251219082344_InitialCreate'
)
BEGIN
    CREATE TABLE [AttendanceRecords] (
        [Id] int NOT NULL IDENTITY,
        [Date] datetime2 NOT NULL,
        [CourseId] int NOT NULL,
        [StudentProfileId] int NOT NULL,
        [SectionId] int NOT NULL,
        [Status] int NOT NULL,
        [MarkedByTeacherId] int NULL,
        [CreatedAt] datetime2 NOT NULL,
        CONSTRAINT [PK_AttendanceRecords] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_AttendanceRecords_Courses_CourseId] FOREIGN KEY ([CourseId]) REFERENCES [Courses] ([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_AttendanceRecords_Sections_SectionId] FOREIGN KEY ([SectionId]) REFERENCES [Sections] ([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_AttendanceRecords_StudentProfiles_StudentProfileId] FOREIGN KEY ([StudentProfileId]) REFERENCES [StudentProfiles] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251219082344_InitialCreate'
)
BEGIN
    CREATE TABLE [Enrollments] (
        [Id] int NOT NULL IDENTITY,
        [StudentProfileId] int NOT NULL,
        [CourseId] int NOT NULL,
        [SessionId] int NOT NULL,
        [SectionId] int NOT NULL,
        CONSTRAINT [PK_Enrollments] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_Enrollments_Courses_CourseId] FOREIGN KEY ([CourseId]) REFERENCES [Courses] ([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_Enrollments_Sections_SectionId] FOREIGN KEY ([SectionId]) REFERENCES [Sections] ([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_Enrollments_Sessions_SessionId] FOREIGN KEY ([SessionId]) REFERENCES [Sessions] ([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_Enrollments_StudentProfiles_StudentProfileId] FOREIGN KEY ([StudentProfileId]) REFERENCES [StudentProfiles] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251219082344_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_AspNetRoleClaims_RoleId] ON [AspNetRoleClaims] ([RoleId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251219082344_InitialCreate'
)
BEGIN
    EXEC(N'CREATE UNIQUE INDEX [RoleNameIndex] ON [AspNetRoles] ([NormalizedName]) WHERE [NormalizedName] IS NOT NULL');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251219082344_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_AspNetUserClaims_UserId] ON [AspNetUserClaims] ([UserId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251219082344_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_AspNetUserLogins_UserId] ON [AspNetUserLogins] ([UserId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251219082344_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_AspNetUserRoles_RoleId] ON [AspNetUserRoles] ([RoleId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251219082344_InitialCreate'
)
BEGIN
    CREATE INDEX [EmailIndex] ON [AspNetUsers] ([NormalizedEmail]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251219082344_InitialCreate'
)
BEGIN
    EXEC(N'CREATE UNIQUE INDEX [UserNameIndex] ON [AspNetUsers] ([NormalizedUserName]) WHERE [NormalizedUserName] IS NOT NULL');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251219082344_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_AttendanceRecords_CourseId_Date] ON [AttendanceRecords] ([CourseId], [Date]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251219082344_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_AttendanceRecords_SectionId] ON [AttendanceRecords] ([SectionId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251219082344_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_AttendanceRecords_StudentProfileId_Date] ON [AttendanceRecords] ([StudentProfileId], [Date]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251219082344_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_CourseAssignments_CourseId] ON [CourseAssignments] ([CourseId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251219082344_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_CourseAssignments_TeacherProfileId] ON [CourseAssignments] ([TeacherProfileId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251219082344_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_CourseSchedules_CourseId] ON [CourseSchedules] ([CourseId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251219082344_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_CourseSchedules_SectionId] ON [CourseSchedules] ([SectionId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251219082344_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_CourseSchedules_TeacherProfileId] ON [CourseSchedules] ([TeacherProfileId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251219082344_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_Enrollments_CourseId] ON [Enrollments] ([CourseId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251219082344_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_Enrollments_SectionId] ON [Enrollments] ([SectionId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251219082344_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_Enrollments_SessionId] ON [Enrollments] ([SessionId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251219082344_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_Enrollments_StudentProfileId] ON [Enrollments] ([StudentProfileId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251219082344_InitialCreate'
)
BEGIN
    EXEC(N'CREATE UNIQUE INDEX [IX_StudentProfiles_ApplicationUserId] ON [StudentProfiles] ([ApplicationUserId]) WHERE [ApplicationUserId] IS NOT NULL');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251219082344_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_StudentProfiles_SectionId] ON [StudentProfiles] ([SectionId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251219082344_InitialCreate'
)
BEGIN
    EXEC(N'CREATE UNIQUE INDEX [IX_TeacherProfiles_ApplicationUserId] ON [TeacherProfiles] ([ApplicationUserId]) WHERE [ApplicationUserId] IS NOT NULL');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251219082344_InitialCreate'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20251219082344_InitialCreate', N'8.0.8');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251220094301_InitialIdentity'
)
BEGIN
    ALTER TABLE [AttendanceRecords] DROP CONSTRAINT [FK_AttendanceRecords_StudentProfiles_StudentProfileId];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251220094301_InitialIdentity'
)
BEGIN
    ALTER TABLE [AttendanceRecords] ADD CONSTRAINT [FK_AttendanceRecords_StudentProfiles_StudentProfileId] FOREIGN KEY ([StudentProfileId]) REFERENCES [StudentProfiles] ([Id]) ON DELETE NO ACTION;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251220094301_InitialIdentity'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20251220094301_InitialIdentity', N'8.0.8');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251220094712_FixAttendanceCascade'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20251220094712_FixAttendanceCascade', N'8.0.8');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251220095102_FixMultipleCascade'
)
BEGIN
    ALTER TABLE [AttendanceRecords] DROP CONSTRAINT [FK_AttendanceRecords_Sections_SectionId];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251220095102_FixMultipleCascade'
)
BEGIN
    ALTER TABLE [AttendanceRecords] ADD CONSTRAINT [FK_AttendanceRecords_Sections_SectionId] FOREIGN KEY ([SectionId]) REFERENCES [Sections] ([Id]) ON DELETE NO ACTION;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251220095102_FixMultipleCascade'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20251220095102_FixMultipleCascade', N'8.0.8');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251220095622_FixMultipleCascadePaths'
)
BEGIN
    ALTER TABLE [AttendanceRecords] DROP CONSTRAINT [FK_AttendanceRecords_Sections_SectionId];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251220095622_FixMultipleCascadePaths'
)
BEGIN
    DROP INDEX [IX_AttendanceRecords_CourseId_Date] ON [AttendanceRecords];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251220095622_FixMultipleCascadePaths'
)
BEGIN
    DROP INDEX [IX_AttendanceRecords_StudentProfileId_Date] ON [AttendanceRecords];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251220095622_FixMultipleCascadePaths'
)
BEGIN
    CREATE INDEX [IX_AttendanceRecords_CourseId] ON [AttendanceRecords] ([CourseId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251220095622_FixMultipleCascadePaths'
)
BEGIN
    CREATE INDEX [IX_AttendanceRecords_StudentProfileId] ON [AttendanceRecords] ([StudentProfileId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251220095622_FixMultipleCascadePaths'
)
BEGIN
    ALTER TABLE [AttendanceRecords] ADD CONSTRAINT [FK_AttendanceRecords_Sections_SectionId] FOREIGN KEY ([SectionId]) REFERENCES [Sections] ([Id]) ON DELETE CASCADE;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251220095622_FixMultipleCascadePaths'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20251220095622_FixMultipleCascadePaths', N'8.0.8');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251220100106_FixCascadePaths'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20251220100106_FixCascadePaths', N'8.0.8');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251229195500_FixCourseScheduleSessionRelationship'
)
BEGIN
    ALTER TABLE [AttendanceRecords] DROP CONSTRAINT [FK_AttendanceRecords_Courses_CourseId];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251229195500_FixCourseScheduleSessionRelationship'
)
BEGIN
    ALTER TABLE [AttendanceRecords] DROP CONSTRAINT [FK_AttendanceRecords_Sections_SectionId];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251229195500_FixCourseScheduleSessionRelationship'
)
BEGIN
    ALTER TABLE [AttendanceRecords] DROP CONSTRAINT [FK_AttendanceRecords_StudentProfiles_StudentProfileId];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251229195500_FixCourseScheduleSessionRelationship'
)
BEGIN
    ALTER TABLE [CourseAssignments] DROP CONSTRAINT [FK_CourseAssignments_Courses_CourseId];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251229195500_FixCourseScheduleSessionRelationship'
)
BEGIN
    ALTER TABLE [CourseAssignments] DROP CONSTRAINT [FK_CourseAssignments_TeacherProfiles_TeacherProfileId];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251229195500_FixCourseScheduleSessionRelationship'
)
BEGIN
    ALTER TABLE [CourseSchedules] DROP CONSTRAINT [FK_CourseSchedules_Courses_CourseId];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251229195500_FixCourseScheduleSessionRelationship'
)
BEGIN
    ALTER TABLE [CourseSchedules] DROP CONSTRAINT [FK_CourseSchedules_Sections_SectionId];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251229195500_FixCourseScheduleSessionRelationship'
)
BEGIN
    ALTER TABLE [CourseSchedules] DROP CONSTRAINT [FK_CourseSchedules_TeacherProfiles_TeacherProfileId];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251229195500_FixCourseScheduleSessionRelationship'
)
BEGIN
    ALTER TABLE [Enrollments] DROP CONSTRAINT [FK_Enrollments_Courses_CourseId];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251229195500_FixCourseScheduleSessionRelationship'
)
BEGIN
    ALTER TABLE [Enrollments] DROP CONSTRAINT [FK_Enrollments_Sections_SectionId];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251229195500_FixCourseScheduleSessionRelationship'
)
BEGIN
    ALTER TABLE [Enrollments] DROP CONSTRAINT [FK_Enrollments_Sessions_SessionId];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251229195500_FixCourseScheduleSessionRelationship'
)
BEGIN
    ALTER TABLE [Enrollments] DROP CONSTRAINT [FK_Enrollments_StudentProfiles_StudentProfileId];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251229195500_FixCourseScheduleSessionRelationship'
)
BEGIN
    ALTER TABLE [StudentProfiles] DROP CONSTRAINT [FK_StudentProfiles_Sections_SectionId];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251229195500_FixCourseScheduleSessionRelationship'
)
BEGIN
    ALTER TABLE [TeacherProfiles] DROP CONSTRAINT [FK_TeacherProfiles_AspNetUsers_ApplicationUserId];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251229195500_FixCourseScheduleSessionRelationship'
)
BEGIN
    DECLARE @var0 sysname;
    SELECT @var0 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[TeacherProfiles]') AND [c].[name] = N'EmployeeId');
    IF @var0 IS NOT NULL EXEC(N'ALTER TABLE [TeacherProfiles] DROP CONSTRAINT [' + @var0 + '];');
    ALTER TABLE [TeacherProfiles] ALTER COLUMN [EmployeeId] nvarchar(50) NULL;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251229195500_FixCourseScheduleSessionRelationship'
)
BEGIN
    DECLARE @var1 sysname;
    SELECT @var1 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[StudentProfiles]') AND [c].[name] = N'FullName');
    IF @var1 IS NOT NULL EXEC(N'ALTER TABLE [StudentProfiles] DROP CONSTRAINT [' + @var1 + '];');
    ALTER TABLE [StudentProfiles] ALTER COLUMN [FullName] nvarchar(100) NULL;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251229195500_FixCourseScheduleSessionRelationship'
)
BEGIN
    DECLARE @var2 sysname;
    SELECT @var2 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Sessions]') AND [c].[name] = N'StartDate');
    IF @var2 IS NOT NULL EXEC(N'ALTER TABLE [Sessions] DROP CONSTRAINT [' + @var2 + '];');
    ALTER TABLE [Sessions] ALTER COLUMN [StartDate] datetime NOT NULL;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251229195500_FixCourseScheduleSessionRelationship'
)
BEGIN
    DECLARE @var3 sysname;
    SELECT @var3 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Sessions]') AND [c].[name] = N'Name');
    IF @var3 IS NOT NULL EXEC(N'ALTER TABLE [Sessions] DROP CONSTRAINT [' + @var3 + '];');
    ALTER TABLE [Sessions] ALTER COLUMN [Name] nvarchar(100) NULL;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251229195500_FixCourseScheduleSessionRelationship'
)
BEGIN
    DECLARE @var4 sysname;
    SELECT @var4 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Sessions]') AND [c].[name] = N'EndDate');
    IF @var4 IS NOT NULL EXEC(N'ALTER TABLE [Sessions] DROP CONSTRAINT [' + @var4 + '];');
    ALTER TABLE [Sessions] ALTER COLUMN [EndDate] datetime NOT NULL;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251229195500_FixCourseScheduleSessionRelationship'
)
BEGIN
    DECLARE @var5 sysname;
    SELECT @var5 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Sections]') AND [c].[name] = N'Name');
    IF @var5 IS NOT NULL EXEC(N'ALTER TABLE [Sections] DROP CONSTRAINT [' + @var5 + '];');
    ALTER TABLE [Sections] ALTER COLUMN [Name] nvarchar(50) NULL;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251229195500_FixCourseScheduleSessionRelationship'
)
BEGIN
    DECLARE @var6 sysname;
    SELECT @var6 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Courses]') AND [c].[name] = N'Title');
    IF @var6 IS NOT NULL EXEC(N'ALTER TABLE [Courses] DROP CONSTRAINT [' + @var6 + '];');
    ALTER TABLE [Courses] ALTER COLUMN [Title] nvarchar(100) NULL;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251229195500_FixCourseScheduleSessionRelationship'
)
BEGIN
    DECLARE @var7 sysname;
    SELECT @var7 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Courses]') AND [c].[name] = N'CreditHours');
    IF @var7 IS NOT NULL EXEC(N'ALTER TABLE [Courses] DROP CONSTRAINT [' + @var7 + '];');
    ALTER TABLE [Courses] ADD DEFAULT 3 FOR [CreditHours];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251229195500_FixCourseScheduleSessionRelationship'
)
BEGIN
    DECLARE @var8 sysname;
    SELECT @var8 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Courses]') AND [c].[name] = N'Code');
    IF @var8 IS NOT NULL EXEC(N'ALTER TABLE [Courses] DROP CONSTRAINT [' + @var8 + '];');
    ALTER TABLE [Courses] ALTER COLUMN [Code] nvarchar(50) NULL;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251229195500_FixCourseScheduleSessionRelationship'
)
BEGIN
    ALTER TABLE [Courses] ADD [Name] nvarchar(50) NULL;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251229195500_FixCourseScheduleSessionRelationship'
)
BEGIN
    CREATE TABLE [SectionSessions] (
        [Id] int NOT NULL IDENTITY,
        [SectionId] int NOT NULL,
        [SessionId] int NOT NULL,
        CONSTRAINT [PK_SectionSessions] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_SectionSessions_Sections] FOREIGN KEY ([SectionId]) REFERENCES [Sections] ([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_SectionSessions_Sessions] FOREIGN KEY ([SessionId]) REFERENCES [Sessions] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251229195500_FixCourseScheduleSessionRelationship'
)
BEGIN
    CREATE INDEX [IX_CourseSchedules_SessionId] ON [CourseSchedules] ([SessionId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251229195500_FixCourseScheduleSessionRelationship'
)
BEGIN
    CREATE INDEX [IX_CourseAssignments_SectionId] ON [CourseAssignments] ([SectionId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251229195500_FixCourseScheduleSessionRelationship'
)
BEGIN
    CREATE INDEX [IX_CourseAssignments_SessionId] ON [CourseAssignments] ([SessionId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251229195500_FixCourseScheduleSessionRelationship'
)
BEGIN
    CREATE INDEX [IX_SectionSessions_SectionId] ON [SectionSessions] ([SectionId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251229195500_FixCourseScheduleSessionRelationship'
)
BEGIN
    CREATE INDEX [IX_SectionSessions_SessionId] ON [SectionSessions] ([SessionId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251229195500_FixCourseScheduleSessionRelationship'
)
BEGIN
    ALTER TABLE [AttendanceRecords] ADD CONSTRAINT [FK_Attendance_Course] FOREIGN KEY ([CourseId]) REFERENCES [Courses] ([Id]) ON DELETE NO ACTION;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251229195500_FixCourseScheduleSessionRelationship'
)
BEGIN
    ALTER TABLE [AttendanceRecords] ADD CONSTRAINT [FK_Attendance_Section] FOREIGN KEY ([SectionId]) REFERENCES [Sections] ([Id]) ON DELETE NO ACTION;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251229195500_FixCourseScheduleSessionRelationship'
)
BEGIN
    ALTER TABLE [AttendanceRecords] ADD CONSTRAINT [FK_Attendance_Student] FOREIGN KEY ([StudentProfileId]) REFERENCES [StudentProfiles] ([Id]) ON DELETE NO ACTION;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251229195500_FixCourseScheduleSessionRelationship'
)
BEGIN
    ALTER TABLE [CourseAssignments] ADD CONSTRAINT [FK_CourseAssignments_Courses] FOREIGN KEY ([CourseId]) REFERENCES [Courses] ([Id]) ON DELETE CASCADE;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251229195500_FixCourseScheduleSessionRelationship'
)
BEGIN
    ALTER TABLE [CourseAssignments] ADD CONSTRAINT [FK_CourseAssignments_Sections] FOREIGN KEY ([SectionId]) REFERENCES [Sections] ([Id]) ON DELETE CASCADE;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251229195500_FixCourseScheduleSessionRelationship'
)
BEGIN
    ALTER TABLE [CourseAssignments] ADD CONSTRAINT [FK_CourseAssignments_Sessions] FOREIGN KEY ([SessionId]) REFERENCES [Sessions] ([Id]) ON DELETE CASCADE;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251229195500_FixCourseScheduleSessionRelationship'
)
BEGIN
    ALTER TABLE [CourseAssignments] ADD CONSTRAINT [FK_CourseAssignments_TeacherProfiles] FOREIGN KEY ([TeacherProfileId]) REFERENCES [TeacherProfiles] ([Id]) ON DELETE NO ACTION;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251229195500_FixCourseScheduleSessionRelationship'
)
BEGIN
    ALTER TABLE [CourseSchedules] ADD CONSTRAINT [FK_CourseSchedules_Courses] FOREIGN KEY ([CourseId]) REFERENCES [Courses] ([Id]) ON DELETE CASCADE;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251229195500_FixCourseScheduleSessionRelationship'
)
BEGIN
    ALTER TABLE [CourseSchedules] ADD CONSTRAINT [FK_CourseSchedules_Sections] FOREIGN KEY ([SectionId]) REFERENCES [Sections] ([Id]) ON DELETE CASCADE;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251229195500_FixCourseScheduleSessionRelationship'
)
BEGIN
    ALTER TABLE [CourseSchedules] ADD CONSTRAINT [FK_CourseSchedules_Sessions] FOREIGN KEY ([SessionId]) REFERENCES [Sessions] ([Id]) ON DELETE CASCADE;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251229195500_FixCourseScheduleSessionRelationship'
)
BEGIN
    ALTER TABLE [CourseSchedules] ADD CONSTRAINT [FK_CourseSchedules_TeacherProfiles] FOREIGN KEY ([TeacherProfileId]) REFERENCES [TeacherProfiles] ([Id]) ON DELETE NO ACTION;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251229195500_FixCourseScheduleSessionRelationship'
)
BEGIN
    ALTER TABLE [Enrollments] ADD CONSTRAINT [FK_Enrollments_Courses] FOREIGN KEY ([CourseId]) REFERENCES [Courses] ([Id]) ON DELETE CASCADE;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251229195500_FixCourseScheduleSessionRelationship'
)
BEGIN
    ALTER TABLE [Enrollments] ADD CONSTRAINT [FK_Enrollments_Sections] FOREIGN KEY ([SectionId]) REFERENCES [Sections] ([Id]) ON DELETE CASCADE;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251229195500_FixCourseScheduleSessionRelationship'
)
BEGIN
    ALTER TABLE [Enrollments] ADD CONSTRAINT [FK_Enrollments_Sessions] FOREIGN KEY ([SessionId]) REFERENCES [Sessions] ([Id]) ON DELETE CASCADE;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251229195500_FixCourseScheduleSessionRelationship'
)
BEGIN
    ALTER TABLE [Enrollments] ADD CONSTRAINT [FK_Enrollments_StudentProfiles] FOREIGN KEY ([StudentProfileId]) REFERENCES [StudentProfiles] ([Id]) ON DELETE CASCADE;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251229195500_FixCourseScheduleSessionRelationship'
)
BEGIN
    ALTER TABLE [StudentProfiles] ADD CONSTRAINT [FK_StudentProfiles_Sections_SectionId] FOREIGN KEY ([SectionId]) REFERENCES [Sections] ([Id]) ON DELETE NO ACTION;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251229195500_FixCourseScheduleSessionRelationship'
)
BEGIN
    ALTER TABLE [TeacherProfiles] ADD CONSTRAINT [FK_TeacherProfile_AspNetUsers] FOREIGN KEY ([ApplicationUserId]) REFERENCES [AspNetUsers] ([Id]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251229195500_FixCourseScheduleSessionRelationship'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20251229195500_FixCourseScheduleSessionRelationship', N'8.0.8');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251229203642_AddSectionSessionsTable'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20251229203642_AddSectionSessionsTable', N'8.0.8');
END;
GO

COMMIT;
GO

