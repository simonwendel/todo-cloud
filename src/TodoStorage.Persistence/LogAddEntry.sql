CREATE PROCEDURE 
[LogAddEntry] (
	@machineName NVARCHAR(200),
	@siteName NVARCHAR(200),
	@logged DATETIME,	
	@level VARCHAR(5),
	@userName NVARCHAR(200),
	@message NVARCHAR(MAX),
	@logger NVARCHAR(300),
	@properties NVARCHAR(MAX),
	@serverName NVARCHAR(200),
	@port NVARCHAR(100),
	@url NVARCHAR(2000),
	@https BIT,
	@serverAddress NVARCHAR(100),
	@remoteAddress NVARCHAR(100),
	@callSite NVARCHAR(300),
	@exception NVARCHAR(MAX)) 
AS
BEGIN
	INSERT INTO [Log] (
		[MachineName],
		[SiteName],
		[Logged],
		[Level],
		[UserName],
		[Message],
		[Logger],
		[Properties],
		[ServerName],
		[Port],
		[Url],
		[Https],
		[ServerAddress],
		[RemoteAddress],
		[CallSite],
		[Exception]
	) VALUES (
		@machineName,
		@siteName,
		@logged,
		@level,
		@userName,
		@message,
		@logger,
		@properties,
		@serverName,
		@port,
		@url,
		@https,
		@serverAddress,
		@remoteAddress,
		@callSite,
		@exception
	);
END