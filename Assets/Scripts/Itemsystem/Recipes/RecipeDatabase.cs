using System.Collections.Generic;

public static class RecipeDatabase
{
    public static Dictionary<string, Recipe> Recipes = new();

    public static void Register(Recipe recipe)
    {
        Recipes[recipe.Id] = recipe;
    }

    public static Recipe Get(string id)
    {
        return Recipes[id];
    }
}
class Recipes
{
    
}