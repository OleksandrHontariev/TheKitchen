CREATE TABLE RecipeDetails (
	Id int IDENTITY(1,1) NOT NULL CONSTRAINT PK_RecipeDetails_Id PRIMARY KEY CLUSTERED(Id),
	KitchenId int NOT NULL CONSTRAINT FK_RecipeDetails_KitchenId_Kitchens_Id FOREIGN KEY REFERENCES Kitchens(Id) ON DELETE CASCADE,
	Ordering int NOT NULL,
	RecipeId int NOT NULL CONSTRAINT FK_RecipeDetails_RecipeId_Recipes_Id FOREIGN KEY REFERENCES Recipes(Id) ON DELETE NO ACTION,
	IngredientId int NULL CONSTRAINT FK_RecipeDetails_IngredientId_Ingredients_Id FOREIGN KEY REFERENCES Ingredients(Id) ON DELETE NO ACTION,
	UnitId int NULL CONSTRAINT FK_RecipeDetails_UnitId_Units_Id FOREIGN KEY REFERENCES Units(Id) ON DELETE NO ACTION,
	Quantity decimal(10,2) NOT NULL
);