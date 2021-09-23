using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Projet_perso_Cocktail
{
    class CocktailPerso : Cocktail
    {
        public static bool cocktailList = false;
        public float ingredientAdd = 1.5f;
        public CocktailPerso() : base("Cocktail personnalisé", 5, false, null)
        {
            this.ingredients = new List<string>();

            Console.WriteLine("Vous souhaitez: \n" +
               "1 - Choisir un cocktail dans la liste\n" +
               "2 - Créer votre cocktail personnalisée");

            string choice_str = Console.ReadLine();
            int choice_int = int.Parse(choice_str);
            
            switch (choice_int)
            {
                case 1:
                    Console.Write("Quel cocktail choisissez-vous ? : ");
                    string choiceCocktail = Console.ReadLine();
                    choiceCocktail = choiceCocktail.ToUpper();

                    Console.WriteLine("Vous avez choisi le " + choiceCocktail);

                    cocktailList = true;
                    break;
                case 2:
                    CocktailIngredients();
                    break;
            }
        }
        public void CocktailIngredients()
        {
            while(true)
            {
                Console.Write("Que souhaitez vous ajouter dans votre cocktail (ENTRER pour terminer): ");
                string choiceIngredient = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(choiceIngredient))
                {
                    break;
                }
                else
                {
                    if (ingredients.Contains(choiceIngredient))
                    {
                        Console.WriteLine("----- ERREUR: Cet ingrédient est déjà dans la liste -----");
                    }
                    else
                    {
                        this.ingredients.Add(choiceIngredient);
                        this.price += ingredientAdd;

                        var ingredientsAfficher = ingredients.Select(i => FirstLetterUpper(i)).ToList();
                        Console.WriteLine(String.Join(", ", ingredientsAfficher));
                    }
                }
                Console.WriteLine();
            }
            Console.WriteLine();
            Console.WriteLine("Total :");
            Console.WriteLine();
        }
    }
    class Cocktail
    {
        public string name { get; private set; }
        public float price { get; protected set; }
        public bool alcohol { get; private set; }
        public List<string> ingredients { get; protected set; }

        public Cocktail(string name, float price, bool alcohol, List<string> ingredients)
        {
            this.name = name;
            this.price = price;
            this.alcohol = alcohol;
            this.ingredients = ingredients;
        }

        public void Display()
        {
            string withAlcohol = alcohol ? " (A)" : "";

            string nameUpper = name.ToUpper();

            Console.WriteLine(nameUpper + withAlcohol + " - " + price + "€");

            var ingredientsAfficher = ingredients.Select(i => FirstLetterUpper(i)).ToList();
            Console.WriteLine(String.Join(", ", ingredientsAfficher));
            Console.WriteLine();
        }

        public static string FirstLetterUpper(string s)
        {
            var ingredientUpper = s.ToUpper();
            var ingredientLower = s.ToLower();
            var ingredients = ingredientUpper[0] + ingredientLower[1..];

            return ingredients;
        }   

        public static List<Cocktail> ChoiceCocktailsDisplay(List<Cocktail> cocktails)
        {
            int choice_int = 0;

            while(choice_int < 1 || choice_int > 3)
            {
                Console.WriteLine("Voir les cocktails :\n" +
               "1 - Tous les cocktails\n" +
               "2 - Les cocktails AVEC alcool\n" +
               "3 - Les cocktails SANS alcool\n");

                string choice_str = Console.ReadLine();
                try
                {
                    choice_int = int.Parse(choice_str);
                }
                catch
                {
                    choice_int = 0;
                }

                switch (choice_int)
                {
                    case 1:
                        return cocktails;
                    case 2:
                        cocktails = cocktails.Where(c => c.alcohol == true).ToList();
                        return cocktails;
                    case 3:
                        cocktails = cocktails.Where(c => c.alcohol == false).ToList();
                        return cocktails;
                    default:
                        Console.WriteLine("Le numéro n'est pas valide. Réessayer");
                        Console.WriteLine();
                        break;
                }
            }
            return cocktails;
        }
    }

    class Program 
    {
        static List<Cocktail> GetCocktailsFromCode()
        {
            var cocktails = new List<Cocktail>()
            {
                new Cocktail("mojito", 6.5f, true, new List<string> { "rhum", "menthe", "citron vert" }),
                new Cocktail("Virgin Margarita", 5.5f, false,  new List<string> { "jus d'orange", "agave", "citron vert" }),
                new Cocktail("Rosmary & Lime", 8.5f, true, new List<string> { "liqueur de sureau", "fraise", "citron vert" }),
                new Cocktail("monaco", 6f, true, new List<string> { "grenadine", "bière", "limonade" }),
                new Cocktail("Lagon Bleu", 10.5f, false, new List<string> { "pamplemousse", "menthe", "schweppes" }),
                new Cocktail("Long Drink", 7f, true, new List<string> { "romarin", "cordial", "pamplemousse" }),
            };

            return cocktails;
        }

        static List<Cocktail> GetCocktailsFromFile(string filename)
        {
            string cocktailJson = null;
            try
            {
                cocktailJson = File.ReadAllText("cocktails.txt");
            }
            catch
            {
                Console.WriteLine("Erreur de lecture du fichier : " + filename);
                return null;
            }

            List<Cocktail> cocktails = null;
            try
            {
                cocktails = JsonConvert.DeserializeObject<List<Cocktail>>(cocktailJson);
            }
            catch
            {
                Console.WriteLine("Erreur : Les données json ne sont pas valide");
                return null;
            }
            return cocktails;
        }
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.Unicode;

            //cocktails = cocktails.OrderBy(c => c.price).ToList();
            //cocktails = cocktails.OrderBy(c => c.name).ToList();
            //cocktails = cocktails.Where(c => c.ingredients.Where(i => i.Contains("pamplemousse")).ToList().Count > 0).ToList();
            //cocktails = cocktails.Where(c => c.alcohol == true).ToList();

            var filename = "cocktails.txt";
            var cocktails = GetCocktailsFromFile(filename);

            cocktails = Cocktail.ChoiceCocktailsDisplay(cocktails);

            //cocktails = cocktails.Where(n => n.name.Contains("MOJITO")).ToList();

            if (cocktails != null)
            {
                foreach (var cocktail in cocktails)
                {
                    cocktail.Display();
                }
            }

            var cocktailPerso = new List<Cocktail>() { new CocktailPerso() };

            if (!CocktailPerso.cocktailList)
            {
                foreach (var cocktail in cocktailPerso)
                {
                    cocktail.Display();
                }
            }

        }
    }
}
