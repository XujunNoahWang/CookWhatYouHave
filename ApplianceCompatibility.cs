using System.Collections.Generic;
using System.Linq;

namespace CookWhatYouHave
{
    public static class ApplianceCompatibility
    {
        // Checks if a recipe's instructions are compatible with the user's appliances
        public static bool IsApplianceCompatible(string instructions, List<string> appliances)
        {
            if (string.IsNullOrEmpty(instructions)) return true;

            instructions = instructions.ToLower();
            var applianceRules = GetApplianceRules();

            foreach (var rule in applianceRules)
            {
                if (instructions.Contains(rule.Key))
                {
                    bool hasCompatibleAppliance = rule.Value.Any(required => appliances.Contains(required));
                    if (!hasCompatibleAppliance)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        // Gets the list of required appliances based on instructions
        public static List<string> GetRequiredAppliances(string instructions)
        {
            if (string.IsNullOrEmpty(instructions)) return new List<string>();

            instructions = instructions.ToLower();
            var applianceRules = GetApplianceRules();
            var requiredAppliances = new List<string>();

            foreach (var rule in applianceRules)
            {
                if (instructions.Contains(rule.Key))
                {
                    requiredAppliances.AddRange(rule.Value);
                }
            }

            return requiredAppliances.Distinct().ToList();
        }

        // Defines common cooking methods and their required appliances
        private static Dictionary<string, List<string>> GetApplianceRules()
        {
            return new Dictionary<string, List<string>>
            {
                { "bake", new List<string> { "oven", "air fryer" } },
                { "roast", new List<string> { "oven", "air fryer" } },
                { "fry", new List<string> { "frying pan", "air fryer", "wok" } },
                { "stir-fry", new List<string> { "wok", "frying pan" } },
                { "microwave", new List<string> { "microwave" } },
                { "cook rice", new List<string> { "rice cooker" } },
                { "steam", new List<string> { "steamer", "egg steamer", "rice cooker" } }
            };
        }
    }
}