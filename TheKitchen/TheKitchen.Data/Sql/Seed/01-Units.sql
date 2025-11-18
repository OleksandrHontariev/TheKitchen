SET IDENTITY_INSERT Units ON
INSERT INTO Units (Id, KitchenId, Code, Title, IsCountable)
VALUES
(1, {{kitchenId}}, N'pcs', N'Штука', 1),
(2, {{kitchenId}}, N'g', N'Грамм', 0),
(3, {{kitchenId}}, N'kg', N'Килограмм', 0),
(4, {{kitchenId}}, N'l', N'Литр', 0),
(5, {{kitchenId}}, N'ml', N'Миллилитр', 0),
(6, {{kitchenId}}, N'tbsp', N'Столовая ложка', 0),
(7, {{kitchenId}}, N'tsp', N'Чайная ложка', 0),
(8, {{kitchenId}}, N'cup', N'Чашка', 0),
(9, {{kitchenId}}, N'bunch', N'Пучок', 1),
(10, {{kitchenId}}, N'slice', N'Ломтик', 1);
SET IDENTITY_INSERT Units OFF