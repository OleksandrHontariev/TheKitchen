SET IDENTITY_INSERT IngredientCategories ON
INSERT INTO IngredientCategories (Id, KitchenId, Name)
VALUES
(1, {{kitchenId}}, N'Бакалея'),
(2, {{kitchenId}}, N'Молочные продукты'),
(3, {{kitchenId}}, N'Мясо'),
(4, {{kitchenId}}, N'Овощи корнеплоды'),
(5, {{kitchenId}}, N'Овощи свежие'),
(6, {{kitchenId}}, N'Жиры и масла'),
(7, {{kitchenId}}, N'Специи'),
(8, {{kitchenId}}, N'Яйца и продукты из яиц'),
(9, {{kitchenId}}, N'Хлебобулочные изделия'),
(10, {{kitchenId}}, N'Сыры'),
(11, {{kitchenId}}, N'Рыба и морепродукты'),
(12, {{kitchenId}}, N'Замороженные продукты'),
(13, {{kitchenId}}, N'Фрукты свежие'),
(14, {{kitchenId}}, N'Бобовые'),
(15, {{kitchenId}}, N'Соусы и заправки'),
(16, {{kitchenId}}, N'Консервация')
SET IDENTITY_INSERT IngredientCategories OFF