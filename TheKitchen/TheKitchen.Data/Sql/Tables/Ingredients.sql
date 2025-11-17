CREATE TABLE Ingredients (
	Id int NOT NULL IDENTITY(1,1) CONSTRAINT PK_Ingredients_Id PRIMARY KEY CLUSTERED(Id),
	KitchenId int NOT NULL CONSTRAINT FK_Ingredients_KitchenId_Kitchens_Id FOREIGN KEY REFERENCES Kitchens(Id) ON DELETE CASCADE,
	Title nvarchar(500) NOT NULL,
	BaseUnit int NOT NULL CONSTRAINT FK_Ingredients_UnitId_Units_Id FOREIGN KEY REFERENCES Units(Id) ON DELETE NO ACTION,
	StockQuantity decimal(10,2) NOT NULL CONSTRAINT DF_Ingredients_StockQuantity DEFAULT 0.0
);