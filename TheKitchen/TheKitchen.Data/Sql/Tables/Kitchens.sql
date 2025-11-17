CREATE TABLE Kitchens (
	Id int NOT NULL IDENTITY(1,1) PRIMARY KEY CLUSTERED (Id),
	Name nvarchar(500) NOT NULL,
	Description nvarchar(max) NULL,
	TablesCount int NULL
);