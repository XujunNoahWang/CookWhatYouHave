# CookWhatYouHave

A C# application that recommends personalized recipes based on user-provided ingredients and kitchen appliances, leveraging the Spoonacular API. It features dynamic whitelists, appliance compatibility checks, and generates HTML pages for recipe instructions.

## Overview

CookWhatYouHave helps users make the most of what’s in their kitchen by suggesting recipes tailored to their available ingredients and appliances. Starting as a console app, it evolved into a modular system that outputs recipe details in HTML for a richer user experience.

## Features

- **Ingredient and Appliance Input**: Validates user inputs against dynamic whitelists.
- **Recipe Recommendations**: Fetches recipes from Spoonacular API, filtering by available ingredients and appliances.
- **Dynamic Whitelists**: Updates ingredient whitelist based on API data, stored locally in JSON files.
- **HTML Output**: Generates and opens recipe webpages with steps and images.
- **Modular Design**: Separates concerns into distinct classes (e.g., `RecipeFetcher`, `InputValidator`).

## Technologies Used

- **C#**: Core programming language.
- **.NET**: Framework for HTTP requests and JSON handling.
- **Spoonacular API**: Provides recipe data (API key required).
- **Grok (xAI)**: Assisted in code implementation and iteration based on my requirements.

## Getting Started

### Prerequisites
- .NET SDK (version 6.0 or later)
- A Spoonacular API key (free tier available at [Spoonacular](https://spoonacular.com/food-api))

### Installation
1. Clone the repository:
   ```bash
   git clone https://github.com/[YourUsername]/CookWhatYouHave.git
2. Open in Visual Studio or your preferred IDE.
3. Replace the API key in ApiClient.cs with your own:
  var apiClient = new ApiClient(client, "YOUR_API_KEY_HERE"); // There is an API key in the file, but has limited token. You may encounter error due to limitation.
4. Build and run the solution.

### Usage
1. Run the app in a console:
  Welcome to CookWhatYouHave!
  Please enter ingredients (comma-separated, e.g., egg, noodle, chicken breast, beef, salt, pepper):
2. Input ingredients and appliances as prompted.
3. View recipe recommendations and select a number (1-5) to open an HTML page with detailed steps and an image.

### Project Structure
- Program.cs: Entry point, orchestrates the application flow.
- Recipe.cs: Defines recipe data structure.
- Ingredient.cs: Represents an ingredient.
- RecipeInfo.cs: Holds detailed recipe information.
- InputValidator.cs: Validates user input against whitelists.
- RecipeFetcher.cs: Fetches and filters recipes.
- RecipeInstructions.cs: Generates and displays recipe instructions in HTML.
- RecipeDetailsFetcher.cs: Retrieves detailed recipe data.
- ApplianceCompatibility.cs: Checks appliance compatibility.
- WhiteListManager.cs: Manages dynamic whitelists.
- ApiClient.cs: Handles API key and HTTP client.

### Acknowledgments
This project was developed with assistance from Grok, an AI tool by xAI, which helped implement and refine the code based on my specifications. I defined the requirements, iterated on features (e.g., appliance matching, HTML output), and guided its development to meet practical cooking needs.

### Future Improvements
Add a local database to cache recipes and reduce API calls.
Implement a .NET MAUI GUI for cross-platform support.
Support multiple images per recipe (requires premium API).
