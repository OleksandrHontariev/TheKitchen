CREATE TABLE MenuItemDetails(
	Id int IDENTITY(1,1) NOT NULL CONSTRAINT PK_MenuItemDetails_Id PRIMARY KEY CLUSTERED(Id),
	KitchenId int NOT NULL CONSTRAINT FK_MenuItemDetails_KitchenId_Kitchens_Id FOREIGN KEY REFERENCES Kitchens(Id) ON DELETE CASCADE,
	MenuItemId int NOT NULL CONSTRAINT FK_MenuItemDetails_MenuItemId_MenuItems_Id FOREIGN KEY REFERENCES MenuItems(Id) ON DELETE NO ACTION,
	DishId int NOT NULL CONSTRAINT FK_MenuItemDetails_DishId_Dishes_Id FOREIGN KEY REFERENCES Dishes(Id) ON DELETE NO ACTION,
	PortionCount decimal(10,2) NOT NULL CONSTRAINT DF_MenuItemDetails_PortionCount DEFAULT 1,
	Ordering int NOT NULL CONSTRAINT DF_MenuItemDetails_Ordering DEFAULT 1
);