CREATE TABLE ChefDetails(
	Id int IDENTITY(1,1) NOT NULL CONSTRAINT PK_CheafDetails PRIMARY KEY CLUSTERED(Id),
	KitchenId int NOT NULL CONSTRAINT FK_ChefDetails_KitchenId_Kitchens_Id FOREIGN KEY REFERENCES Kitchens(Id) ON DELETE CASCADE,
	ChefId int NOT NULL CONSTRAINT FK_ChefDetails_ChefId_Chefs_Id FOREIGN KEY REFERENCES Chefs(Id) ON DELETE NO ACTION,
	PhoneNumber char(14) CONSTRAINT CK_CheafDetails_PhoneNumber CHECK(PhoneNumber LIKE '([0-9][0-9][0-9])[0-9][0-9][0-9]-[0-9][0-9]-[0-9][0-9]'),
	PasspordNumber nvarchar(50) NULL,
	BirthDate date NULL,
	Country nvarchar(100) NOT NULL,
	Region nvarchar(100) NULL,
	City nvarchar(100) NOT NULL,
	Street nvarchar(150) NOT NULL,
	House nvarchar(20) NOT NULL,
	Apartment nvarchar(20) NULL,
	PostalCode nvarchar(20) NULL,
	Notes nvarchar(max) NULL
);