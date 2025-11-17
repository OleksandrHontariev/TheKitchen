CREATE TABLE Chefs(
	Id int NOT NULL IDENTITY(1,1) CONSTRAINT PK_Chefs_Id PRIMARY KEY CLUSTERED(Id),
	KitchenId int NOT NULL CONSTRAINT FK_Chefs_KitchenId_Kitchens_Id FOREIGN KEY REFERENCES Kitchens (Id) ON DELETE CASCADE,
	FName nvarchar(50) NOT NULL,
	MName nvarchar(50) NOT NULL,
	LName nvarchar(50) NOT NULL,
	HideDate datetime NULL,
	ExpirienceYears int NULL
);