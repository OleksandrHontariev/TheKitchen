CREATE TABLE Dishes (
	Id int NOT NULL IDENTITY(1,1) CONSTRAINT PK_Dish_Id PRIMARY KEY CLUSTERED(Id),
	KitchenId int NOT NULL CONSTRAINT FK_Dishes_KitchenId_Kitchens_Id FOREIGN KEY REFERENCES Kitchens(Id) ON DELETE CASCADE,
	ChefId int NOT NULL CONSTRAINT FK_Dishes_ChefId_Chefs_Id FOREIGN KEY REFERENCES Chefs(Id) ON DELETE NO ACTION,
	RecipeId int NOT NULL CONSTRAINT FK_Dishes_RecipeId_Recipes_Id FOREIGN KEY REFERENCES Recipes(Id) ON DELETE NO ACTION,
	AvailableCount int NOT NULL CONSTRAINT DF_Dishes_AvailableCount DEFAULT 0,
	PreparationCount int NULL,
	PreparedAt datetime NULL,
	FinishedAt datetime NULL
);