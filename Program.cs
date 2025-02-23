using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace CookWhatYouHave
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Welcome to CookWhatYouHave!");

            // Load whitelists
            var validIngredients = WhiteListManager.LoadIngredients();
            var validAppliances = WhiteListManager.LoadAppliances();

            // Get valid input
            List<string> ingredients = InputValidator.GetValidIngredientsInput(validIngredients);
            List<string> appliances = InputValidator.GetValidAppliancesInput(validAppliances);

            // Create HttpClient and API client
            using (HttpClient client = new HttpClient())
            {
                var apiClient = new ApiClient(client, "58b5eaaeaabc4df29b92b45de3baab92");

                // Fetch and filter recipes
                var recipesToShow = await RecipeFetcher.FetchAndFilterRecipes(client, ingredients, appliances, apiClient.ApiKey);

                // Update whitelists
                var allRecipes = recipesToShow.Select(r => r.Recipe).ToList();
                WhiteListManager.UpdateWhiteLists(allRecipes, validIngredients, validAppliances);

                // Display recipe recommendations
                RecipeFetcher.DisplayRecommendations(recipesToShow, ingredients);

                // User interaction to view recipe steps
                await RecipeInstructions.InteractWithUser(client, recipesToShow, apiClient.ApiKey);
            }

            Console.WriteLine("Press Enter to exit...");
            Console.ReadLine();
        }
    }
}