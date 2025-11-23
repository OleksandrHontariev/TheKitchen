CREATE TABLE RecipeCategories (
	Id int NOT NULL IDENTITY(1,1) CONSTRAINT PK_RecipeCategories_Id PRIMARY KEY CLUSTERED,
	KitchenId int NOT NULL CONSTRAINT FK_RecipeCategories_KitchenId_Kitchens_Id FOREIGN KEY REFERENCES Kitchens(Id) ON DELETE CASCADE,
	ParentCategoryId int NULL CONSTRAINT FK_RecipeCategories_ParentCategoryId_RecipeCategories_Id FOREIGN KEY REFERENCES RecipeCategories(Id) ON DELETE NO ACTION,
	Name nvarchar(200) NOT NULL,
	CreatedAt datetime CONSTRAINT DF_RecipeCategories_CreatedAt DEFAULT GETDATE()
);