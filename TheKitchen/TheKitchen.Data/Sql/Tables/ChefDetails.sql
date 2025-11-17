CREATE TABLE ChefDetails(
	Id int IDENTITY(1,1) NOT NULL CONSTRAINT PK_CheafDetails PRIMARY KEY CLUSTERED(Id),
	KitchenId int NOT NULL CONSTRAINT FK_ChefDetails_KitchenId_Kitchens_Id FOREIGN KEY REFERENCES Kitchens(Id) ON DELETE CASCADE,
	ChefId int NOT NULL CONSTRAINT FK_ChefDetails_ChefId_Chefs_Id FOREIGN KEY REFERENCES Chefs(Id) ON DELETE NO ACTION,
	PhoneNumber char(14) CONSTRAINT CK_CheafDetails_PhoneNumber CHECK(PhoneNumber LIKE '([0-9][0-9][0-9])[0-9][0-9][0-9]-[0-9][0-9]-[0-9][0-9]'),
	PasspordNumber nvarchar(50) NULL,
	BirthDate date NULL,
	Address nvarchar(500) NULL,
	Notes nvarchar(max) NULL
);