
USE master
GO

IF EXISTS
(
    SELECT NAME
    FROM Sys.Databases
    WHERE Name = N'ApplicationLogsTests'
)
BEGIN
    DROP DATABASE [ApplicationLogsTests]
END
GO

CREATE DATABASE [ApplicationLogsTests]
GO

USE [ApplicationLogsTests]
GO

CREATE SCHEMA [Logs];
GO

CREATE TABLE [ApplicationLogsTests].[Logs].[ApplicationLog]
(
    [ApplicationLogId] INT NOT NULL IDENTITY,
    [Type] INT NOT NULL,
    [UserId] VARCHAR(50),
    [Source] VARCHAR(256),
    [Message] VARCHAR(1024) NOT NULL,
    [Data] VARCHAR(1024),
    [Url] VARCHAR(512),
    [StackTrace] VARCHAR(2048),
    [HostIpAddress] VARCHAR(25),
    [UserIpAddress] VARCHAR(25),
    [UserAgent] VARCHAR(25),
    [When] DATETIME NOT NULL DEFAULT GETDATE()
    CONSTRAINT ApplicationLog_PK PRIMARY KEY ([ApplicationLogId])
);
GO

CREATE PROCEDURE [Logs].[pApplicationLog_Delete]
    @applicationLogId INT
AS
BEGIN
    DELETE FROM [ApplicationLogsTests].[Logs].[ApplicationLog]
    WHERE [ApplicationLogId] = @applicationLogId;

END;
GO

CREATE PROCEDURE [Logs].[pApplicationLog_DeleteOlderLogs]
    @when DATETIME
AS
BEGIN
    DELETE FROM [ApplicationLogsTests].[Logs].[ApplicationLog]
    WHERE [When] < @when;

END;
GO

CREATE PROCEDURE [Logs].[pApplicationLog_Insert]
    @type INT,
    @userId VARCHAR(50) = NULL,
    @source VARCHAR(256) = NULL,
    @message VARCHAR(1024),
    @data VARCHAR(1024) = NULL,
    @url VARCHAR(512) = NULL,
    @stackTrace VARCHAR(2048) = NULL,
    @hostIpAddress VARCHAR(25) = NULL,
    @userIpAddress VARCHAR(25) = NULL,
    @userAgent VARCHAR(25) = NULL
AS
BEGIN
    DECLARE @applicationLogOutputData TABLE
    (
        [ApplicationLogId] INT,
        [When] DATETIME
    );

    INSERT INTO [ApplicationLogsTests].[Logs].[ApplicationLog]
    (
        [Type],
        [UserId],
        [Source],
        [Message],
        [Data],
        [Url],
        [StackTrace],
        [HostIpAddress],
        [UserIpAddress],
        [UserAgent]
    )
    OUTPUT
        INSERTED.[ApplicationLogId],
        INSERTED.[When]
        INTO @applicationLogOutputData
    VALUES
    (
        @type,
        @userId,
        @source,
        @message,
        @data,
        @url,
        @stackTrace,
        @hostIpAddress,
        @userIpAddress,
        @userAgent
    );

    SELECT
        [ApplicationLogId],
        [When]
    FROM @applicationLogOutputData;

END;
GO

CREATE PROCEDURE [Logs].[pApplicationLog_Get]
    @$select NVARCHAR(MAX) = NULL,
    @$filter NVARCHAR(MAX) = NULL,
    @$orderby NVARCHAR(MAX) = NULL,
    @$skip NVARCHAR(10) = NULL,
    @$top NVARCHAR(10) = NULL,
    @count INT OUTPUT
AS
BEGIN
    EXEC [dbo].[pExecuteDynamicQuery]
        @$select = @$select,
        @$filter = @$filter,
        @$orderby = @$orderby,
        @$skip = @$skip,
        @$top = @$top,
        @selectList = N'    a.[ApplicationLogId] AS "Id",
    a.[Type] AS "Type",
    a.[UserId] AS "UserId",
    a.[Source] AS "Source",
    a.[Message] AS "Message",
    a.[Data] AS "Data",
    a.[Url] AS "Url",
    a.[StackTrace] AS "StackTrace",
    a.[HostIpAddress] AS "HostIpAddress",
    a.[UserIpAddress] AS "UserIpAddress",
    a.[UserAgent] AS "UserAgent",
    a.[When] AS "When"',
        @from = N'[ApplicationLogsTests].[Logs].[ApplicationLog] a',
        @count = @count OUTPUT

END;
GO

CREATE PROCEDURE [Logs].[pApplicationLog_GetById]
    @applicationLogId INT
AS
BEGIN
    SELECT
        a.[ApplicationLogId] AS "Id",
        a.[Type] AS "Type",
        a.[UserId] AS "UserId",
        a.[Source] AS "Source",
        a.[Message] AS "Message",
        a.[Data] AS "Data",
        a.[Url] AS "Url",
        a.[StackTrace] AS "StackTrace",
        a.[HostIpAddress] AS "HostIpAddress",
        a.[UserIpAddress] AS "UserIpAddress",
        a.[UserAgent] AS "UserAgent",
        a.[When] AS "When"
    FROM [ApplicationLogsTests].[Logs].[ApplicationLog] a
    WHERE a.[ApplicationLogId] = @applicationLogId;

END;
GO

CREATE PROCEDURE [pExecuteDynamicQuery]
	@$select NVARCHAR(MAX) = NULL,
	@$filter NVARCHAR(MAX) = NULL,
	@$orderby NVARCHAR(MAX) = NULL,
	@$skip NVARCHAR(10) = NULL,
	@$top NVARCHAR(10) = NULL,
	@selectList NVARCHAR(MAX),
	@from NVARCHAR(MAX),
	@count INT OUTPUT
AS
BEGIN

	DECLARE @sqlCommand NVARCHAR(MAX);
	DECLARE @paramDefinition NVARCHAR(100);

	SET @paramDefinition = N'@cnt INT OUTPUT'

	SET @sqlCommand = 
'
	SELECT
		 @cnt = COUNT(1)
	FROM ' + @from + '
';

	IF @$filter IS NOT NULL
	BEGIN 
		SET @sqlCommand = @sqlCommand + 
' 
	WHERE ' + @$filter;
	END

	SET @sqlCommand = @sqlCommand + 
'
	SELECT
	';

	IF ISNULL(@$select, '*') = '*'
	BEGIN
		SET @sqlCommand = @sqlCommand + @selectList;
	END
	ELSE
	BEGIN
		SET @sqlCommand = @sqlCommand + @$select;
	END

	SET @sqlCommand = @sqlCommand +
'
	FROM ' + @from + '
';

	IF @$filter IS NOT NULL
	BEGIN 
		SET @sqlCommand = @sqlCommand + 
' 
	WHERE ' + @$filter;
	END

	IF @$orderby IS NOT NULL
	BEGIN 
		SET @sqlCommand = @sqlCommand + 
' 
	ORDER BY ' + @$orderby;
	END
	ELSE
	BEGIN

	-- At least a dummy order by is required is $skip and $top are provided
		IF @$skip IS NOT NULL OR @$top IS NOT NULL
		BEGIN  
			SET @sqlCommand = @sqlCommand + 
' 
	ORDER BY 1 ASC';
		END
	END

	IF @$skip IS NOT NULL
	BEGIN 
		SET @sqlCommand = @sqlCommand + 
' 
	OFFSET ' + @$skip + ' ROWS';
	END

	IF @$top IS NOT NULL
	BEGIN 
		SET @sqlCommand = @sqlCommand + 
' 
	FETCH NEXT ' + @$top + ' ROWS ONLY';
	END

	EXECUTE sp_executesql @sqlCommand, @paramDefinition, @cnt = @count OUTPUT

END;
GO

