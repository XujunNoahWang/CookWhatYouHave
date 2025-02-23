using System;
using System.Collections.Generic;
using System.Linq;

namespace CookWhatYouHave
{
    public static class InputValidator
    {
        // Gets valid ingredient input from the user
        public static List<string> GetValidIngredientsInput(HashSet<string> validIngredients)
        {
            while (true)
            {
                Console.WriteLine("Please enter ingredients (comma-separated, e.g., egg, noodle, chicken breast, beef, salt, pepper):");
                string input = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(input))
                {
                    Console.WriteLine("Input cannot be empty. Please try again.");
                    continue;
                }

                List<string> ingredients = input.Split(',').Select(i => i.Trim().ToLower()).ToList();
                List<string> invalidIngredients = new List<string>();

                foreach (var item in ingredients)
                {
                    if (!IsValidIngredient(item, validIngredients))
                    {
                        invalidIngredients.Add(item);
                    }
                }

                if (invalidIngredients.Count > 0)
                {
                    Console.WriteLine($"The following inputs are not valid ingredients: {string.Join(", ", invalidIngredients)}");
                    Console.WriteLine("Please re-enter only valid ingredients (e.g., egg, noodle, beef, etc.).");
                }
                else
                {
                    return ingredients;
                }
            }
        }

        // Gets valid appliance input from the user
        public static List<string> GetValidAppliancesInput(HashSet<string> validAppliances)
        {
            while (true)
            {
                Console.WriteLine("Please enter your kitchen appliances (comma-separated, e.g., microwave, rice cooker, air fryer, frying pan, egg steamer):");
                string input = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(input))
                {
                    Console.WriteLine("Input cannot be empty. Please try again.");
                    continue;
                }

                List<string> appliances = input.Split(',').Select(a => a.Trim().ToLower()).ToList();
                List<string> invalidAppliances = new List<string>();

                foreach (var item in appliances)
                {
                    if (!IsValidAppliance(item, validAppliances))
                    {
                        invalidAppliances.Add(item);
                    }
                }

                if (invalidAppliances.Count > 0)
                {
                    Console.WriteLine($"The following inputs are not valid kitchen appliances: {string.Join(", ", invalidAppliances)}");
                    Console.WriteLine("Please re-enter only valid kitchen appliances (e.g., microwave, oven, rice cooker, etc.).");
                }
                else
                {
                    return appliances;
                }
            }
        }

        // Validates if an item is a recognized ingredient
        private static bool IsValidIngredient(string item, HashSet<string> validIngredients)
        {
            item = item.Trim();
            if (string.IsNullOrEmpty(item) || item.All(char.IsDigit)) return false;
            return validIngredients.Contains(item);
        }

        // Validates if an item is a recognized appliance
        private static bool IsValidAppliance(string item, HashSet<string> validAppliances)
        {
            item = item.Trim();
            if (string.IsNullOrEmpty(item) || item.All(char.IsDigit)) return false;
            return validAppliances.Contains(item);
        }
    }
}