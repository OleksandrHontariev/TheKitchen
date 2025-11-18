CREATE TABLE IngredientCategories(
	Id int NOT NULL IDENTITY(1,1) CONSTRAINT PK_IngredientCategories_Id PRIMARY KEY CLUSTERED,
	KitchenId int NOT NULL CONSTRAINT FK_IngredientCategories_KitchenId_Kitchens_Id FOREIGN KEY REFERENCES Kitchens(Id) ON DELETE CASCADE,
	ParentCategoryId int NULL CONSTRAINT FK_IngredientCategories_ParentCategoryId_IngredientCategories_Id FOREIGN KEY REFERENCES IngredientCategories(Id),
	Name nvarchar(200) NOT NULL,
	CreatedAt datetime CONSTRAINT DF_IngredientCategories_CreatedAt DEFAULT GETDATE()
);