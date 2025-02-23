using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace CookWhatYouHave
{
    public static class RecipeFetcher
    {
        // Fetches and filters recipes based on ingredients and appliances
        public static async Task<List<(Recipe Recipe, List<string> RequiredAppliances)>> FetchAndFilterRecipes(HttpClient client, List<string> ingredients, List<string> appliances, string apiKey)
        {
            string url = $"https://api.spoonacular.com/recipes/findByIngredients?ingredients={string.Join(",", ingredients)}&number=10&ranking=2&apiKey={apiKey}";
            HttpResponseMessage response = await client.GetAsync(url);
            response.EnsureSuccessStatusCode();
            string jsonResult = await response.Content.ReadAsStringAsync();

            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var allRecipes = JsonSerializer.Deserialize<List<Recipe>>(jsonResult, options);

            if (allRecipes == null || allRecipes.Count == 0)
            {
                Console.WriteLine("No recipes found. Input may be invalid or API returned empty.");
                return new List<(Recipe, List<string>)>();
            }

            List<(Recipe Recipe, List<string> RequiredAppliances)> compatibleRecipes = new List<(Recipe, List<string>)>();
            foreach (var recipe in allRecipes)
            {
                var detailedRecipe = await RecipeDetailsFetcher.GetRecipeDetails(client, recipe.Id, apiKey);
                if (detailedRecipe != null && ApplianceCompatibility.IsApplianceCompatible(detailedRecipe.Instructions, appliances))
                {
                    var requiredAppliances = ApplianceCompatibility.GetRequiredAppliances(detailedRecipe.Instructions);
                    compatibleRecipes.Add((recipe, requiredAppliances));
                }
            }

            var exactMatches = compatibleRecipes
                .Where(r => r.Recipe.MissedIngredients == null || r.Recipe.MissedIngredients.Count == 0)
                .ToList();

            if (exactMatches.Count == 0)
            {
                Console.WriteLine("\nNo recipes fully match your ingredients and appliances. The following may require additional ingredients or cooking method adjustments:");
            }
            else if (exactMatches.Count < 5)
            {
                Console.WriteLine($"\nFound only {exactMatches.Count} recipes fully matching your ingredients and appliances. The following include additional suggestions that may require extra ingredients or adjustments:");
            }
            else
            {
                Console.WriteLine("\nFound enough fully matching recipes. Here are the recommendations:");
            }

            List<(Recipe Recipe, List<string> RequiredAppliances)> recipesToShow = exactMatches.Take(5).ToList();
            if (recipesToShow.Count < 5)
            {
                int remaining = 5 - recipesToShow.Count;
                var additionalRecipes = compatibleRecipes
                    .Where(r => !exactMatches.Contains(r))
                    .Take(remaining)
                    .ToList();
                recipesToShow.AddRange(additionalRecipes);
            }

            return recipesToShow;
        }

        // Displays recipe recommendations in the console
        public static void DisplayRecommendations(List<(Recipe Recipe, List<string> RequiredAppliances)> recipesToShow, List<string> ingredients)
        {
            Console.WriteLine("\nRecommended Recipes:");
            for (int i = 0; i < recipesToShow.Count; i++)
            {
                var (recipe, requiredAppliances) = recipesToShow[i];
                Console.WriteLine($"{i + 1}. {recipe.Title ?? "Unknown recipe"}");

                var usedInput = recipe.UsedIngredients
                    .Where(ui => ingredients.Any(ii => ui.Name.ToLower().Contains(ii)))
                    .Select(ui => ingredients.First(ii => ui.Name.ToLower().Contains(ii)))
                    .Distinct()
                    .ToList();
                Console.WriteLine("  Ingredients I have: " + (usedInput.Count > 0 ? string.Join(", ", usedInput) : "None"));

                Console.WriteLine("  Additional ingredients: " + (recipe.MissedIngredients != null && recipe.MissedIngredients.Count > 0
                    ? string.Join(", ", recipe.MissedIngredientNames()) : "None"));

                var unusedInput = recipe.UnusedIngredients
                    .Where(ui => ingredients.Contains(ui.Name.ToLower()))
                    .Select(ui => ui.Name)
                    .Distinct()
                    .ToList();
                Console.WriteLine("  Unused input ingredients: " + (unusedInput.Count > 0 ? string.Join(", ", unusedInput) : "None"));

                Console.WriteLine("  Required kitchen appliances: " + (requiredAppliances.Count > 0
                    ? string.Join(", ", requiredAppliances) : "No specific requirements"));
                Console.WriteLine();
            }
        }
    }
}