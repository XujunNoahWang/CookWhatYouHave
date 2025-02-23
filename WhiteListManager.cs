using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace CookWhatYouHave
{
    public static class WhiteListManager
    {
        private static readonly string IngredientsFile = "valid_ingredients.json";
        private static readonly string AppliancesFile = "valid_appliances.json";

        // Loads or initializes the ingredients whitelist
        public static HashSet<string> LoadIngredients()
        {
            if (File.Exists(IngredientsFile))
            {
                string json = File.ReadAllText(IngredientsFile);
                return JsonSerializer.Deserialize<HashSet<string>>(json) ?? new HashSet<string>();
            }
            return new HashSet<string>
            {
                "egg", "noodle", "chicken", "chicken breast", "beef", "salt", "pepper", "rice", "potato", "onion",
                "garlic", "carrot", "tomato", "flour", "sugar", "oil", "butter", "milk", "cheese", "bread", "oil", "olive oil"
            }; // Default initial values
        }

        // Loads or initializes the appliances whitelist
        public static HashSet<string> LoadAppliances()
        {
            if (File.Exists(AppliancesFile))
            {
                string json = File.ReadAllText(AppliancesFile);
                return JsonSerializer.Deserialize<HashSet<string>>(json) ?? new HashSet<string>();
            }
            return new HashSet<string>
            {
                "microwave", "rice cooker", "air fryer", "frying pan", "wok", "oven", "steamer", "egg steamer",
                "slow cooker", "blender", "toaster", "pot", "pan", "stove"
            }; // Default initial values
        }

        // Updates whitelists with new ingredients from recipes and saves them
        public static void UpdateWhiteLists(List<Recipe> recipes, HashSet<string> ingredients, HashSet<string> appliances)
        {
            foreach (var recipe in recipes)
            {
                // Update ingredients
                if (recipe.UsedIngredients != null)
                {
                    foreach (var ingredient in recipe.UsedIngredients)
                    {
                        ingredients.Add(ingredient.Name.ToLower());
                    }
                }
                if (recipe.MissedIngredients != null)
                {
                    foreach (var ingredient in recipe.MissedIngredients)
                    {
                        ingredients.Add(ingredient.Name.ToLower());
                    }
                }
            }

            // Save to files
            File.WriteAllText(IngredientsFile, JsonSerializer.Serialize(ingredients));
            File.WriteAllText(AppliancesFile, JsonSerializer.Serialize(appliances));
        }
    }
}