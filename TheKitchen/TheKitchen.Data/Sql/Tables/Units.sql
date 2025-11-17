CREATE TABLE Units(
	Id int NOT NULL IDENTITY(1,1) CONSTRAINT PK_Units_Id PRIMARY KEY CLUSTERED(Id),
	KitchenId int NOT NULL CONSTRAINT FK_Units_KitchenId_Kitchens_Id FOREIGN KEY REFERENCES Kitchens(Id),
	Code nvarchar(20) NOT NULL,
	Title nvarchar(100) NOT NULL,
	IsCountable bit NOT NULL CONSTRAINT DF_Units_IsCountable DEFAULT 0
);