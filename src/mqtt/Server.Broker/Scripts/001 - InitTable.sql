CREATE TABLE Device(
    Id UNIQUEIDENTIFIER NOT NULL DEFAULT NEWID() PRIMARY KEY,
	[Name] NVARCHAR(128) NULL,
	[Ip] NVARCHAR(128) NULL,
	LastPing DATETIME NULL,
	Firmware NVARCHAR(200) NULL,
	FirmwareVersion INT NULL,
	[Status] NVARCHAR(200) NULL,
    CreateAt DATETIME NOT NULL DEFAULT GETUTCDATE(),
    CreateBy NVARCHAR(48) NOT NULL,
    DeleteAt DATETIME NULL,
    DeleteBy NVARCHAR(48) NULL,
    UpdateAt DATETIME NULL,
    UpdateBy NVARCHAR(48) NULL
);

CREATE TABLE DeviceLog(
    Id UNIQUEIDENTIFIER NOT NULL DEFAULT NEWID() PRIMARY KEY,
	DeviceId UNIQUEIDENTIFIER NOT NULL,
	[Type] NVARCHAR(200) NULL,
	[RoutineKey] NVARCHAR(200) NULL,
	[Content] NVARCHAR(MAX) NULL,
    CreateAt DATETIME NOT NULL DEFAULT GETUTCDATE(),
    CreateBy NVARCHAR(48) NOT NULL,
    DeleteAt DATETIME NULL,
    DeleteBy NVARCHAR(48) NULL,
    UpdateAt DATETIME NULL,
    UpdateBy NVARCHAR(48) NULL
);


CREATE TABLE Firmware (
    Id UNIQUEIDENTIFIER NOT NULL DEFAULT NEWID() PRIMARY KEY,
	[Name] NVARCHAR(MAX) NOT NULL,
	[Description] NVARCHAR(MAX) NULL,
    CreateAt DATETIME NOT NULL DEFAULT GETUTCDATE(),
    CreateBy NVARCHAR(48) NOT NULL,
    DeleteAt DATETIME NULL,
    DeleteBy NVARCHAR(48) NULL,
    UpdateAt DATETIME NULL,
    UpdateBy NVARCHAR(48) NULL
);

CREATE TABLE FirmwareVersion (
    Id UNIQUEIDENTIFIER NOT NULL DEFAULT NEWID() PRIMARY KEY,
	FirmwareId UNIQUEIDENTIFIER NOT NULL,
	[Version] INT NOT NULL,
	[Content] VARBINARY(MAX) NOT NULL,
    CreateAt DATETIME NOT NULL DEFAULT GETUTCDATE(),
    CreateBy NVARCHAR(48) NOT NULL,
    DeleteAt DATETIME NULL,
    DeleteBy NVARCHAR(48) NULL,
    UpdateAt DATETIME NULL,
    UpdateBy NVARCHAR(48) NULL
);