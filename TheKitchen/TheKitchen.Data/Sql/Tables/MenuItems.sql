CREATE TABLE MenuItems (
	Id int IDENTITY(1,1) NOT NULL CONSTRAINT PK_MenuItem_Id PRIMARY KEY CLUSTERED(Id),
	KitchenId int NOT NULL CONSTRAINT FK_MenuItems_KitchenId_Kitchens_Id FOREIGN KEY REFERENCES Kitchens(Id) ON DELETE CASCADE,
	Title nvarchar(200) NOT NULL,
	Description nvarchar(max) NULL
);