using System.Collections.Generic;

namespace CookWhatYouHave
{
    // Represents a recipe with its details
    public class Recipe
    {
        public int Id { get; set; } // Recipe ID
        public string Title { get; set; } // Recipe title
        public string Image { get; set; } // URL of the recipe image
        public List<Ingredient> UsedIngredients { get; set; } // Ingredients used in the recipe
        public List<Ingredient> MissedIngredients { get; set; } // Ingredients missing for the recipe
        public List<Ingredient> UnusedIngredients { get; set; } // Input ingredients not used

        // Returns names of used ingredients
        public List<string> UsedIngredientNames() =>
            UsedIngredients != null ? UsedIngredients.ConvertAll(i => i.Name ?? "Unknown ingredient") : new List<string>();

        // Returns names of missed ingredients
        public List<string> MissedIngredientNames() =>
            MissedIngredients != null ? MissedIngredients.ConvertAll(i => i.Name ?? "Unknown ingredient") : new List<string>();

        // Returns names of unused ingredients
        public List<string> UnusedIngredientNames() =>
            UnusedIngredients != null ? UsedIngredients.ConvertAll(i => i.Name ?? "Unknown ingredient") : new List<string>();
    }
}