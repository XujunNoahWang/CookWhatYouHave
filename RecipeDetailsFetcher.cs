using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace CookWhatYouHave
{
    public static class RecipeDetailsFetcher
    {
        // Fetches detailed recipe information from the API
        public static async Task<RecipeInfo> GetRecipeDetails(HttpClient client, int recipeId, string apiKey)
        {
            string url = $"https://api.spoonacular.com/recipes/{recipeId}/information?apiKey={apiKey}";
            try
            {
                HttpResponseMessage response = await client.GetAsync(url);
                response.EnsureSuccessStatusCode();
                string jsonResult = await response.Content.ReadAsStringAsync();

                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                return JsonSerializer.Deserialize<RecipeInfo>(jsonResult, options);
            }
            catch (Exception)
            {
                return null; // Returns null if fetching fails
            }
        }
    }
}