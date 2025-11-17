CREATE TABLE UnitConversions(
	Id int NOT NULL IDENTITY(1,1) CONSTRAINT PK_UnitConversions_Id PRIMARY KEY CLUSTERED(Id),
	KitchenId int NOT NULL CONSTRAINT FK_UnitConversions_KitchenId_Kitchens_Id FOREIGN KEY REFERENCES Kitchens(Id) ON DELETE CASCADE,
	FromUnitId int NOT NULL CONSTRAINT FK_UnitConversions_FromUnitId_Units_Id FOREIGN KEY REFERENCES Units(Id) ON DELETE NO ACTION,
	ToUnitId int NOT NULL CONSTRAINT FK_UnitConversions_ToUnitId_Units_Id FOREIGN KEY REFERENCES Units(Id) ON DELETE NO ACTION,
	Multiplier decimal(10,2) NOT NULL,
	Description nvarchar(500) NULL
);