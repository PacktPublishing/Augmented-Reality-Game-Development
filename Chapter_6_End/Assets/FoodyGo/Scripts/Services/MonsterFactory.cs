using UnityEngine;
using packt.FoodyGO.Database;

namespace packt.FoodyGO.Services
{
    public static class MonsterFactory
    {
        public static string[] names = {
            "Chef",
            "Child",
            "Sous",
            "Poulet",
            "Duck",
            "Slice",
            "Sauce",
            "Bacon",
            "Benedict",
            "Beef",
            "Sage"
            };

        public static string[] skills =
        {
            "French",
            "Chinese",
            "Sushi",
            "Breakfast",
            "Hamburger",
            "Indian",
            "BBQ",
            "Mexican",
            "Cajun",
            "Thai",
            "Italian",
            "Fish",
            "Beef",
            "Bacon",
            "Hog",
            "Chicken"
        };

        public static int power = 1;
        public static int level = 1;        
        
        public static Monster CreateRandomMonster()
        {
            var monster = new Monster
            {
                Name = BuildName(),
                Skills = BuildSkills(),
                Power = Random.Range(1, power),
                Level = Random.Range(1, level)
            };
            return monster;
        }

        private static string BuildSkills()
        {
            var max = skills.Length - 1;
            return skills[Random.Range(0, max)] + "," + skills[Random.Range(0, max)];
        }

        private static string BuildName()
        {
            var max = names.Length - 1;
            return names[Random.Range(0, max)] + " " + names[Random.Range(0, max)];
        }
    }
}
