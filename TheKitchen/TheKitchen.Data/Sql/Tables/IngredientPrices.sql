CREATE TABLE IngredientPrices(
	Id int IDENTITY(1,1) CONSTRAINT PK_IngredientPrices_Id PRIMARY KEY CLUSTERED(Id),
	KitchenId int NOT NULL CONSTRAINT FK_IngredientPrices_KitchenId_Kitchens_Id FOREIGN KEY REFERENCES Kitchens(Id) ON DELETE CASCADE,
	IngredientId int NOT NULL CONSTRAINT FK_IngredientPrices_IngredientId_Ingredients_Id FOREIGN KEY REFERENCES Ingredients(Id) ON DELETE NO ACTION,
	UnitId int NOT NULL CONSTRAINT FK_IngredientPrices_UnitId_Units_Id FOREIGN KEY REFERENCES Units(Id) ON DELETE NO ACTION,
	Quantity decimal(10,2) NOT NULL,
	Price decimal(10,2) NOT NULL,
	PricePerBaseUnit decimal(10,2) NULL
);