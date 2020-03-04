# ProfilerLite

This is a simple website for displaying details of sql calls that are logged to a database by another application.  This project is nothing to do with the capturing of the sql - that is handled outside this code.

## Database creation

The website expects a database to be pre configured.  The SQL to create it is:

```
CREATE DATABASE SqlDbLog;
GO

USE SqlDbLog;

CREATE TABLE [dbo].[__Session](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[SessionId] [uniqueidentifier] NOT NULL,
	[Url] [nvarchar](500) NULL,
	[CreatedDate] [datetime] NOT NULL,
	[Method] [nvarchar](25) NULL,
 CONSTRAINT [PK___Session_1] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[__SessionQuery](
    [Id] [int] IDENTITY(1,1) NOT NULL,
    [SessionId] [uniqueidentifier] NOT NULL,
    [CommandText] [nvarchar](max) NOT NULL,
    [Parameters] [nvarchar](max) NULL,
    [CommandId] [uniqueidentifier] NULL,
    [Rows] [int] NULL,
    [Time] [int] NULL,
 CONSTRAINT [PK___SessionQuery] PRIMARY KEY CLUSTERED 
(
    [Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
```

## To run

Update appsettings.json with the connection string to your MSSQL database that contains the SQL logging.

```
cd .\src\ProfilerLite
dotnet run
```

Then in a browser navigate to https://localhost:5001
