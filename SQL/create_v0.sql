CREATE TABLE [dbo].[TrashTransport]
(
    [Id] bigint IDENTITY(1,1) NOT NULL, 
	[Date] datetime NOT NULL CONSTRAINT [TrashTransportDate_Default] DEFAULT (getutcdate()),
    [Description] nvarchar(100),
    [RfId0] nvarchar(100),
    [VehicleName] nvarchar(100),
    [VehicleNumber] nvarchar(100),
    [TrashType] nvarchar(100),
    [Container] nvarchar(100),
    [Note] nvarchar(100),
    [MgoType] nvarchar(100),
    [Latitude] decimal(8,6) NOT NULL CONSTRAINT [Location_Latitude_Default] DEFAULT (0),
    [Longitude] decimal(8,6) NOT NULL CONSTRAINT [Location_Longitude_Default] DEFAULT (0)
)