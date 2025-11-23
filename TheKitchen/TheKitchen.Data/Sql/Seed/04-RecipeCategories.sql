SET IDENTITY_INSERT RecipeCategories ON;

INSERT INTO RecipeCategories (Id, KitchenId, ParentCategoryId, Name)
VALUES
-- Основные категории
(1, {{kitchenId}}, NULL, N'Супы'),
(2, {{kitchenId}}, NULL, N'Горячие блюда'),
(3, {{kitchenId}}, NULL, N'Салаты'),
(4, {{kitchenId}}, NULL, N'Закуски'),
(5, {{kitchenId}}, NULL, N'Десерты'),
(6, {{kitchenId}}, NULL, N'Напитки'),
(7, {{kitchenId}}, NULL, N'Соусы'),

-- Подкатегории "Супы"
(8,  {{kitchenId}}, 1, N'Крем-супы'),
(9,  {{kitchenId}}, 1, N'Бульоны'),
(10, {{kitchenId}}, 1, N'Холодные супы'),

-- Подкатегории "Горячие блюда"
(11, {{kitchenId}}, 2, N'Мясные блюда'),
(12, {{kitchenId}}, 2, N'Рыбные блюда'),
(13, {{kitchenId}}, 2, N'Паста'),
(14, {{kitchenId}}, 2, N'Гриль'),

-- Подкатегории "Салаты"
(15, {{kitchenId}}, 3, N'Овощные салаты'),
(16, {{kitchenId}}, 3, N'Мясные салаты'),
(17, {{kitchenId}}, 3, N'Тёплые салаты'),

-- Подкатегории "Закуски"
(18, {{kitchenId}}, 4, N'Холодные закуски'),
(19, {{kitchenId}}, 4, N'Горячие закуски'),

-- Подкатегории "Десерты"
(20, {{kitchenId}}, 5, N'Торты'),
(21, {{kitchenId}}, 5, N'Пирожные'),
(22, {{kitchenId}}, 5, N'Мороженое'),

-- Подкатегории "Напитки"
(23, {{kitchenId}}, 6, N'Холодные напитки'),
(24, {{kitchenId}}, 6, N'Горячие напитки'),

-- Подкатегории "Соусы"
(25, {{kitchenId}}, 7, N'Холодные соусы'),
(26, {{kitchenId}}, 7, N'Горячие соусы');

SET IDENTITY_INSERT RecipeCategories OFF;
