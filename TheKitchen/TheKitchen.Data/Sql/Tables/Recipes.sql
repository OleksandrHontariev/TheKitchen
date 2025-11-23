CREATE TABLE Recipes (
	Id int IDENTITY(1,1) NOT NULL CONSTRAINT PK_Recipe_Id PRIMARY KEY CLUSTERED(Id),
	KitchenId int NOT NULL CONSTRAINT FK_Recipes_KitchenId_Kitchens_Id FOREIGN KEY REFERENCES Kitchens(Id),
	RecipeCategoryId int NULL CONSTRAINT FK_Recipes_RecipeCategoryId FOREIGN KEY REFERENCES RecipeCategories(Id) ON DELETE NO ACTION,
	Title nvarchar(500) NOT NULL,
	Description nvarchar(max) NOT NULL,
	Portions int NOT NULL CONSTRAINT DF_Recipes_Portions DEFAULT 1
);