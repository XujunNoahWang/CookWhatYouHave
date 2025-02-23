using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using System.Diagnostics;

namespace CookWhatYouHave
{
    public static class RecipeInstructions
    {
        // Interacts with the user to display recipe instructions
        public static async Task InteractWithUser(HttpClient client, List<(Recipe Recipe, List<string> RequiredAppliances)> recipesToShow, string apiKey)
        {
            while (true)
            {
                Console.WriteLine("Please enter a number (1-5) to view recipe instructions, or 0 to exit:");
                string choice = Console.ReadLine();
                if (int.TryParse(choice, out int selection))
                {
                    if (selection == 0) break;
                    if (selection >= 1 && selection <= 5)
                    {
                        int index = selection - 1;
                        var selectedRecipe = recipesToShow[index].Recipe;
                        await ShowRecipeInstructions(client, selectedRecipe.Id, apiKey);
                    }
                    else
                    {
                        Console.WriteLine("Please enter a valid number (1-5) or 0 to exit.");
                    }
                }
                else
                {
                    Console.WriteLine("Invalid input. Please enter a number 1-5 or 0 to exit.");
                }
            }
        }

        // Fetches and displays recipe instructions in an HTML page
        public static async Task ShowRecipeInstructions(HttpClient client, int recipeId, string apiKey)
        {
            string url = $"https://api.spoonacular.com/recipes/{recipeId}/information?apiKey={apiKey}&includeNutrition=false";
            try
            {
                HttpResponseMessage response = await client.GetAsync(url);
                response.EnsureSuccessStatusCode();
                string jsonResult = await response.Content.ReadAsStringAsync();

                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                var recipeInfo = JsonSerializer.Deserialize<RecipeInfo>(jsonResult, options);

                // Generate HTML file
                string htmlContent = GenerateRecipeHtml(recipeInfo);
                string filePath = $"recipe_{recipeId}.html";
                File.WriteAllText(filePath, htmlContent);

                // Open browser
                try
                {
                    Process.Start(new ProcessStartInfo
                    {
                        FileName = filePath,
                        UseShellExecute = true
                    });
                    Console.WriteLine($"Recipe webpage generated and opened: {Path.GetFullPath(filePath)}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Failed to open webpage: {ex.Message}");
                    Console.WriteLine("Please manually open the file: " + Path.GetFullPath(filePath));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching instructions: {ex.Message}");
            }
        }

        // Generates an HTML page with recipe details
        private static string GenerateRecipeHtml(RecipeInfo recipeInfo)
        {
            string html = $@"
<!DOCTYPE html>
<html lang='en'>
<head>
    <meta charset='UTF-8'>
    <title>{recipeInfo.Title}</title>
    <style>
        body {{ font-family: Arial, sans-serif; margin: 20px; }}
        h1 {{ color: #333; }}
        img {{ max-width: 300px; margin: 10px 0; }}
        ol {{ line-height: 1.6; }}
    </style>
</head>
<body>
    <h1>{recipeInfo.Title}</h1>";

            // Add main image
            if (!string.IsNullOrEmpty(recipeInfo.Image))
            {
                html += $"<img src='{recipeInfo.Image}' alt='{recipeInfo.Title}'><br>";
            }

            // Add instructions
            if (!string.IsNullOrEmpty(recipeInfo.Instructions))
            {
                html += "<h2>Instructions</h2><ol>";
                var steps = ParseInstructions(recipeInfo.Instructions);
                foreach (var step in steps)
                {
                    html += $"<li>{step}</li>";
                }
                html += "</ol>";
            }
            else
            {
                html += "<p>Sorry, no detailed instructions available for this recipe.</p>";
            }

            html += "</body></html>";
            return html;
        }

        // Parses recipe instructions into a list of steps
        public static List<string> ParseInstructions(string instructions)
        {
            var steps = new List<string>();
            if (string.IsNullOrEmpty(instructions)) return steps;

            if (instructions.Contains("<li>"))
            {
                instructions = instructions.Replace("<ol>", "").Replace("</ol>", "");
                var parts = instructions.Split(new[] { "<li>", "</li>" }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var part in parts)
                {
                    var cleaned = part.Trim();
                    if (!string.IsNullOrEmpty(cleaned) && !cleaned.StartsWith("<") && !cleaned.EndsWith(">"))
                    {
                        steps.Add(cleaned);
                    }
                }
            }
            else
            {
                var lines = instructions.Split(new[] { "\n", "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var line in lines)
                {
                    var trimmedLine = line.Trim();
                    if (!string.IsNullOrEmpty(trimmedLine))
                    {
                        if (trimmedLine.Contains("."))
                        {
                            var sentences = trimmedLine.Split(new[] { "." }, StringSplitOptions.RemoveEmptyEntries)
                                .Select(s => s.Trim())
                                .Where(s => !string.IsNullOrEmpty(s));
                            steps.AddRange(sentences);
                        }
                        else
                        {
                            steps.Add(trimmedLine);
                        }
                    }
                }
            }

            return steps
                .Where(s => s.Length > 10 &&
                            !s.ToLower().Contains("instruction") &&
                            !s.ToLower().Contains("enjoy") &&
                            !s.ToLower().Contains("serve") &&
                            !s.ToLower().Contains("great") &&
                            !s.ToLower().StartsWith("step"))
                .ToList();
        }
    }
}