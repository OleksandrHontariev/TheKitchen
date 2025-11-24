using Microsoft.Data.SqlClient;
using System;
using System.Data;
using TheKitchen.Data;
using System.IO;
using TheKitchen.Data.Abstractions;
using TheKitchen.Data.Repos;
using TheKitchen.Data.Entities;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using System.Text;
using TheKitchen.Data.Infrastructure;
using System.Linq;

CreateDatabase();

IConfigurationRoot config = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false)
    .Build();

CRUDL_IngredientCategories(config);
CRUDL_RecipeCategories(config);
CRUDL_Ingredients(config);
CRUDL_Recipes(config);
GetPagedBySearchInAllRepos(config);

Console.ReadLine();

static void CreateDatabase()
{
    string dbName = "TheKitchen";
    string masterCS = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=master;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False";
    using (IDbConnection connection = new SqlConnection(masterCS))
    {
        connection.Open();
        DatabaseCreator.CreateDatabase(connection, dbName);
    }

    string targetCS = @$"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog={dbName};Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False";
    using (IDbConnection connection_2 = new SqlConnection(targetCS))
    {
        connection_2.Open();
        string scriptsFolter = Path.Combine(AppContext.BaseDirectory, "Sql", "Tables");
        DatabaseCreator.CreateDatabaseTables(connection_2, scriptsFolter);
        IKitchenRepository kitchenRepository = new KitchenRepository(connection_2);
        int id = kitchenRepository.Add(new Kitchen {
                                        Name = "Puzata Hata",
                                        Description = "Общепит пузатая хата",
                                        TablesCount = 78
                                    });

        DatabaseCreator.SeedStaticData(connection_2, id, Path.Combine(AppContext.BaseDirectory, "Sql", "Seed"));
    }
    Console.WriteLine("Database created successfuly!");
}

static void CRUDL_Kitchen(IConfigurationRoot config)
{
    string connectionString = config.GetConnectionString("DefaultConnection");
    using (IDbConnection connection = new SqlConnection(connectionString))
    {
        connection.Open();

        IKitchenRepository repo = new KitchenRepository(connection);

        // GetAll
        IEnumerable<Kitchen> all = repo.GetAll();

        // Adding
        int mcId = repo.Add(new Kitchen { Name = "McDonald's", Description = "Имитация ресторана Макдональдц", TablesCount = 80 });
        int mushlaId = repo.Add(new Kitchen { Name = "Mushla", Description = "Имитация ресторана Борисова Мушля", TablesCount = 55 });
        int pusataHataId = repo.Add(new Kitchen { Name = "Пузата Хата", Description = "Имитация ресторана Пузата Хата", TablesCount = 67 });

        // Updating
        Kitchen mcDonaldc = repo.GetById(mcId);
        mcDonaldc.Description = "MaaakDonald's";
        repo.Update(mcDonaldc);

        // Deleting
        repo.Delete(mcId);
        repo.Delete(mcId);
    }
}

static void CRUDL_Ingredients(IConfigurationRoot config)
{
    string connectionString = config.GetConnectionString("DefaultConnection");
    using (IDbConnection connection = new SqlConnection(connectionString))
    {
        connection.Open();

        IIngredientRepository ingredientsRepository = new IngredientRepository(connection);

        // GetData
        // GetBySearchParams kitchen = 1, categoryId = null, query = null, page = 1, pageSize = 10
        var result = ingredientsRepository.GetPagedBySearch(1, null, null, 1, 10);

        Console.OutputEncoding = Encoding.UTF8;

        Console.WriteLine("TotalCount: {0}", result.TotalCount);
        Console.WriteLine("PageNumber: {0}", result.PageNumber);
        Console.WriteLine("PageSize: {0}", result.PageSize);
        Console.WriteLine("TotalPages: {0}", result.TotalPages);
        Console.WriteLine("Items:");
        foreach (var r in result.Items)
        {
            Console.WriteLine(r);
        }
        Console.WriteLine("-----------------------------------");

        // GetBySearchParams kitchen = 1, categoryId = 1, query = "", page = 2, pageSize = 3
        var result_2 = ingredientsRepository.GetPagedBySearch(1, 1, "", 2, 3);

        Console.WriteLine("TotalCount: {0}", result_2.TotalCount);
        Console.WriteLine("PageNumber: {0}", result_2.PageNumber);
        Console.WriteLine("PageSize: {0}", result_2.PageSize);
        Console.WriteLine("TotalPages: {0}", result_2.TotalPages);
        Console.WriteLine("Items:");
        foreach (var r in result_2.Items)
        {
            Console.WriteLine(r);
        }
        Console.WriteLine("-----------------------------------");

        // GetBySearchParams kitchen = 1, categoryId = null, query = "яйц", page = 1, pageSize = 5
        var result_3 = ingredientsRepository.GetPagedBySearch(1, null, "яйц", 1, 5);

        Console.WriteLine("TotalCount: {0}", result_3.TotalCount);
        Console.WriteLine("PageNumber: {0}", result_3.PageNumber);
        Console.WriteLine("PageSize: {0}", result_3.PageSize);
        Console.WriteLine("TotalPages: {0}", result_3.TotalPages);
        Console.WriteLine("Items:");
        foreach (var r in result_3.Items)
        {
            Console.WriteLine(r);
        }
        Console.WriteLine("-----------------------------------");

        // Add
        ingredientsRepository.Add(new Ingredient
        {
            Title = "Яйцо страусиное",
            KitchenId = 1,
            IngredientCategoryId = null,
            BaseUnit = 1,
            StockQuantity = 0
        });

        // GetBySearchParams kitchen = 1, categoryId = null, query = null, page = 1, pageSize = 5
        var result_4 = ingredientsRepository.GetPagedBySearch(1, null, null, 1, 5);
        Console.WriteLine("TotalCount: {0}", result_4.TotalCount);
        Console.WriteLine("PageNumber: {0}", result_4.PageNumber);
        Console.WriteLine("PageSize: {0}", result_4.PageSize);
        Console.WriteLine("TotalPages: {0}", result_4.TotalPages);
        Console.WriteLine("Items:");
        foreach (var r in result_4.Items)
        {
            Console.WriteLine(r);
        }

        // Remove
        ingredientsRepository.Delete(61);
        Ingredient ingredient = ingredientsRepository.GetById(61);
        if (ingredient == null)
        {
            Console.WriteLine($"Ingredient not found {ingredient}");
        } else
        {
            Console.WriteLine(ingredient);
        }

        // Update
        Ingredient ingredient_62 = ingredientsRepository.GetById(62);
        ingredient_62.IngredientCategoryId = 3;
        ingredientsRepository.Update(ingredient_62);

        Ingredient updated = ingredientsRepository.GetById(62);
        Console.WriteLine(updated);
    }
}

static void CRUDL_IngredientCategories(IConfigurationRoot config)
{
    Console.OutputEncoding = Encoding.UTF8;
    string connectionString = config.GetConnectionString("DefaultConnection");
    using (IDbConnection connection = new SqlConnection(connectionString))
    {
        connection.Open();
        IIngredientCategoryRepository ingredientCategoryRepository = new IngredientCategoryRepository(connection);
        // GetAll
        // GetBySearchedParams: kitchenId = 1, parentCategoryId = null, query = null, page = 1, pageSize = 10
        PagedResult<IngredientCategory> result = ingredientCategoryRepository.GetPagedBySearch(1, null, null, 1, 10);
        Console.WriteLine("TotalCount: {0}", result.TotalCount);
        Console.WriteLine("PageNumber: {0}", result.PageNumber);
        Console.WriteLine("PageSize: {0}", result.PageSize);
        Console.WriteLine("TotalPages: {0}", result.TotalPages);
        Console.WriteLine("Items:");
        foreach (var r in result.Items)
        {
            Console.WriteLine(r);
        }
        Console.WriteLine("--------------------------------------------");

        // GetBySearchedParams: kitchenId = 1, parentCategoryId = null, query = "продукты", page = 1, pageSize = 4
        PagedResult<IngredientCategory> result_2 = ingredientCategoryRepository.GetPagedBySearch(1, null, "продукты", 1, 4);
        Console.WriteLine("TotalCount: {0}", result_2.TotalCount);
        Console.WriteLine("PageNumber: {0}", result_2.PageNumber);
        Console.WriteLine("PageSize: {0}", result_2.PageSize);
        Console.WriteLine("TotalPages: {0}", result_2.TotalPages);

        foreach (var r in result_2.Items)
        {
            Console.WriteLine(r);
        }
        Console.WriteLine("--------------------------------------------");

        // Add
        int addedId = ingredientCategoryRepository.Add(new IngredientCategory {
            KitchenId = 1,
            ParentIngredientCategoryId = null,
            Name = "Тестовая категория"
        });

        // GetById
        IngredientCategory ingredientCategory = ingredientCategoryRepository.GetById(addedId);
        Console.WriteLine(ingredientCategory);


        // Update
        IngredientCategory ingredientCategory_2 = ingredientCategoryRepository.GetById(addedId);
        ingredientCategory_2.Name = "Обновленная категория";
        if (ingredientCategoryRepository.Update(ingredientCategory_2))
        {
            Console.WriteLine(ingredientCategory_2);
        }

        // Delete
        if (ingredientCategoryRepository.Delete(addedId))
        {
            Console.WriteLine("Deleted by id {0}", addedId);
        }
    }
}

static void CRUDL_RecipeCategories(IConfigurationRoot config)
{
    Console.OutputEncoding = Encoding.UTF8;
    string connectionString = config.GetConnectionString("DefaultConnection");
    using (IDbConnection connection = new SqlConnection(connectionString))
    {
        IRecipeCategoryRepository recipeCategoryRepository = new RecipeCategoryRepository(connection);
        // Get
        // GetBySearchedParams: kitchenId = 1, parentCategoryId = null, query = null, page = 1, pageSize = 10
        PagedResult<RecipeCategory> result = recipeCategoryRepository.GetPagedBySearch(1, null, null, 1, 10);
        Console.WriteLine("TotalCount: {0}", result.TotalCount);
        Console.WriteLine("PageNumber: {0}", result.PageNumber);
        Console.WriteLine("PageSize: {0}", result.PageSize);
        Console.WriteLine("TotalPages: {0}", result.TotalPages);
        Console.WriteLine("Items:");
        foreach (var r in result.Items)
        {
            Console.WriteLine(r);
        }
        Console.WriteLine("--------------------------------------------------");

        // GetBySearchedParams: kitchenId = 1, parentCategoryId = 3, query = null, page = 1, pageSize = 4
        PagedResult<RecipeCategory> result_2 = recipeCategoryRepository.GetPagedBySearch(1, 3, null, 1, 4);
        Console.WriteLine("TotalCount: {0}", result_2.TotalCount);
        Console.WriteLine("PageNumber: {0}", result_2.PageNumber);
        Console.WriteLine("PageSize: {0}", result_2.PageSize);
        Console.WriteLine("TotalPages: {0}", result_2.TotalPages);
        Console.WriteLine("Items:");
        foreach (var r in result_2.Items)
        {
            Console.WriteLine(r);
        }
        Console.WriteLine("--------------------------------------------------");
        // GetBySearchedParams: kitchenId = 1, parentCategoryId = null, query = "салаты", page = 1, pageSize = 5
        PagedResult<RecipeCategory> result_3 = recipeCategoryRepository.GetPagedBySearch(1, null, "салаты", 1, 5);
        Console.WriteLine("TotalCount: {0}", result_3.TotalCount);
        Console.WriteLine("PageNumber: {0}", result_3.PageNumber);
        Console.WriteLine("PageSize: {0}", result_3.PageSize);
        Console.WriteLine("TotalPages: {0}", result_3.TotalPages);
        Console.WriteLine("Items:");
        foreach (var r in result_3.Items)
        {
            Console.WriteLine(r);
        }
        Console.WriteLine("--------------------------------------------------");

        // Add
        int addedId = recipeCategoryRepository.Add(new RecipeCategory {
            KitchenId = 1,
            ParentRecipeCategoryId = null,
            Name = "Тестовая категория"
        });

        // GetById
        RecipeCategory toUpdate = recipeCategoryRepository.GetById(addedId);
        Console.WriteLine(toUpdate);

        // Update
        toUpdate.Name = "Обновленная категория";
        if (recipeCategoryRepository.Update(toUpdate))
        {
            Console.WriteLine(toUpdate);
        }

        // Delete
        if (recipeCategoryRepository.Delete(addedId))
        {
            Console.WriteLine("Deleted with id = {0}", addedId);
        }
    }
}

static void CRUDL_Recipes(IConfigurationRoot config)
{
    Console.OutputEncoding = Encoding.UTF8;
    string connectionString = config.GetConnectionString("DefaultConnection");
    using (IDbConnection connection = new SqlConnection(connectionString))
    {
        connection.Open();
        IRecipeRepository recipeRepository = new RecipeRepository(connection);

        // Get
        // GetPagedBySearch: kitchenId = 1, categoryId = null, query = null, page = 1, pageSize = 10
        PagedResult<Recipe> result = recipeRepository.GetPagedBySearch(1, null, null, 1, 10);
        Console.WriteLine("TotalCount: {0}", result.TotalCount);
        Console.WriteLine("PageNumber: {0}", result.PageNumber);
        Console.WriteLine("PageSize: {0}", result.PageSize);
        Console.WriteLine("TotalPages: {0}", result.TotalPages);
        Console.WriteLine("Items:");
        foreach(Recipe r in result.Items)
        {
            Console.WriteLine(r);
        }
        Console.WriteLine("-----------------------------------");

        // GetPagedBySearch: kitchenId = 1, categoryId = 1, query = null, page = 1, pageSize = 10
        PagedResult<Recipe> result_2 = recipeRepository.GetPagedBySearch(1, 1, null, 1, 10);
        Console.WriteLine("TotalCount: {0}", result_2.TotalCount);
        Console.WriteLine("PageNumber: {0}", result_2.PageNumber);
        Console.WriteLine("PageSize: {0}", result_2.PageSize);
        Console.WriteLine("TotalPages: {0}", result_2.TotalPages);
        Console.WriteLine("Items:");
        foreach (Recipe r in result_2.Items)
        {
            Console.WriteLine(r);
        }
        Console.WriteLine("-----------------------------------");

        // GetPagedBySearch: kitchenId = 1, categoryId = null, query = "суп", page = 1, pageSize = 10
        PagedResult<Recipe> result_3 = recipeRepository.GetPagedBySearch(1, null, "суп", 1, 10);
        Console.WriteLine("TotalCount: {0}", result_3.TotalCount);
        Console.WriteLine("PageNumber: {0}", result_3.PageNumber);
        Console.WriteLine("PageSize: {0}", result_3.PageSize);
        Console.WriteLine("TotalPages: {0}", result_3.TotalPages);
        Console.WriteLine("Items:");
        foreach (Recipe r in result_3.Items)
        {
            Console.WriteLine(r);
        }
        Console.WriteLine("-----------------------------------");

        Recipe recipe = new Recipe {
            KitchenId = 1,
            RecipeCategoryId = 3,
            Title = "Тестовый рецепт",
            Description = "Тестовое описание рецепта",
            Portions = 3
        };

        // Add
        int createdId = recipeRepository.Add(recipe);
        Console.WriteLine("Created recipe with id = {0}, Title = {1}", createdId, recipe.Title);

        // GetById
        Recipe foundRecipe = recipeRepository.GetById(createdId);
        Console.WriteLine("Found recipe with id = {0}", createdId);
        Console.WriteLine(foundRecipe);

        // Update
        Recipe recipeToUpdate = recipeRepository.GetById(createdId);
        recipeToUpdate.Title = "Отредактированный тайтл тестового рецепта";
        if (recipeRepository.Update(recipeToUpdate))
        {
            Recipe updatedRecipe = recipeRepository.GetById(createdId);
            Console.WriteLine(updatedRecipe);
        }

        // Delete
        if (recipeRepository.Delete(createdId))
        {
            Console.WriteLine("Recipe deleted with id: {0}", createdId);
        }
    }
}

// Test GetPagedBySearch Without Setting Category And QueryString
static void GetPagedBySearchInAllRepos(IConfigurationRoot config)
{
    string connectionString = config.GetConnectionString("DefaultConnection");
    using (IDbConnection connection = new SqlConnection(connectionString))
    {
        connection.Open();

        // RecipeRepository
        IRecipeRepository recipeRepository = new RecipeRepository(connection);
        // GetPagedBySearch: kitchenId = 1, recipeCategoryId = null, query = "суп", page = 1, pageSize = 10
        PagedResult<Recipe> recipes = recipeRepository.GetPagedBySearch(1, null, "суп", 1, 10);
        Console.WriteLine("Recipes count in result: {0}", recipes.Items.Count());

        // RecipeCategoryRepository
        IRecipeCategoryRepository recipeCategoryRepository = new RecipeCategoryRepository(connection);
        // GetPagedBySearch: kitchenId = 1, parentCategoryId = null, query = "блюда", page = 1, pageSize = 10
        PagedResult<RecipeCategory> recipeCategories = recipeCategoryRepository.GetPagedBySearch(1, null, "блюда", 1, 10);
        Console.WriteLine("Recipe categories count in result: {0}", recipeCategories.Items.Count());

        // IngredientRepository
        IIngredientRepository ingredientRepository = new IngredientRepository(connection);
        // GetPagedBySearch: kitchenId = 1, categoryId = null, query = "Хлеб", page = 1, pageSize = 10
        PagedResult<Ingredient> ingredients = ingredientRepository.GetPagedBySearch(1, null, "Хлеб", 1, 10);
        Console.WriteLine("Ingredients count in result: {0}", ingredients.Items.Count());

        // IngredientCategoryRepository
        IIngredientCategoryRepository ingredientCategoryRepository = new IngredientCategoryRepository(connection);
        // GetPagedBySearch: kitchenId = 1, parentCategoryId = null, query = "продукты", page = 1, pageSize = 10
        PagedResult<IngredientCategory> ingredientCategories = ingredientCategoryRepository.GetPagedBySearch(1, null, "продукты", 1, 10);
        Console.WriteLine("Ingredient categories count in result: {0}", ingredientCategories.Items.Count());
    }
}