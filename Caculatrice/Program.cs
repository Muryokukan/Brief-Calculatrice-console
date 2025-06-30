using System;
using System.Collections.Generic;
using MesFonction;

//MesFonctions.Hello();

Console.Title = "Calculatrice";

// Initialisation de la liste pour l'historique des calculs
List<string> historiqueCalculs = new List<string>();

bool quitter = true;
bool continuer = true;

while (quitter)
{
    MesFonctions.AfficherMenu();

    int userChoice;
    try
    {
        userChoice = Convert.ToInt32(Console.ReadLine());
    }
    catch
    {
        userChoice = 0;
    }

    switch (userChoice)
    {
        case 1:
            Console.Clear();
            MesFonctions.LigneSeparation();
            MesFonctions.EcrireCentre("Calculatrice");
            MesFonctions.LigneSeparation();
            Console.WriteLine();

            continuer = true; // Réinitialiser pour chaque nouveau calcul
            historiqueCalculs.Clear(); // Vider l'historique précédent

            while (continuer)
            {
                // Si l'historique est vide OU si le dernier élément est un opérateur → demander un nombre
                if (historiqueCalculs.Count == 0 || EstUnOperateur(historiqueCalculs[historiqueCalculs.Count - 1]))
                {
                    Console.Write(new string(' ', (Console.WindowWidth - "Entrez un nombre : ".Length) / 2) + "Entrez un nombre : ");
                    string saisie = Console.ReadLine();

                    // Vérifier si c'est un nombre valide
                    if (double.TryParse(saisie, out double nombre))
                    {
                        historiqueCalculs.Add(saisie);
                    }else
                    {
                        Console.WriteLine("Veuillez entrer un nombre valide");
                        continue;
                    }
                }
                else
                {
                    Console.Write(new string(' ', (Console.WindowWidth - "Entrez un opérateur (+, -, *, /) ou '=' pour calculer : ".Length) / 2) + "Entrez un opérateur (+, -, *, /) ou '=' pour calculer : ");
                    string operateur = Console.ReadLine();

                    if (operateur == "=")
                    {
                        // Calculer le résultat
                        double resultat = CalculerExpression(historiqueCalculs);
                        Console.WriteLine();
                        MesFonctions.EcrireCentre($"Résultat : {string.Join(" ", historiqueCalculs)} = {resultat}");
                        Console.WriteLine();
                        Console.WriteLine("Appuyez sur une touche pour continuer...");
                        Console.ReadKey();
                        continuer = false;
                    }else if (EstUnOperateur(operateur))
                    {
                        historiqueCalculs.Add(operateur);
                    }else
                    {
                        Console.WriteLine("Opérateur non valide !");
                        continue;
                    }
                }

                // Afficher l'historique actuel
                if (historiqueCalculs.Count > 0)
                {
                    Console.Clear();
                    Console.WriteLine();
                    MesFonctions.EcrireCentre("Calcul en cours : " + string.Join(" ", historiqueCalculs));
                    Console.WriteLine();
                }
            }
            break;

        case 2: // Quitter
            Console.Clear();
            MesFonctions.LigneSeparation();
            MesFonctions.EcrireCentre("MERCI D'AVOIR UTILISE NOTRE PROGRAMME !");
            MesFonctions.EcrireCentre("Au revoir !");
            MesFonctions.LigneSeparation();
            quitter = false;
            break;

        default: // Gestion des choix invalides
            Console.Clear();
            MesFonctions.LigneSeparation();
            MesFonctions.EcrireCentre("CHOIX INVALIDE");
            MesFonctions.LigneSeparation();
            Console.WriteLine();

            MesFonctions.EcrireCentre("Veuillez choisir une option entre 1 et 2");
            Console.WriteLine("Appuyez sur une touche pour continuer");
            Console.ReadKey();
            break;
    }
}

Console.ReadKey();

// Fonction pour centrer le texte dans la console
static void EcrireCentre(string texte)
{
    int largeurConsole = Console.WindowWidth;
    int espaces = (largeurConsole - texte.Length) / 2;
    Console.WriteLine(new string(' ', Math.Max(0, espaces)) + texte);
}

// Fonction pour créer une ligne de séparation au centre
static void LigneSeparation(int longueur = 50)
{
    EcrireCentre(new string('-', longueur));
}

// Fonction pour afficher le menu principal
static void AfficherMenu()
{
    Console.Clear();
    LigneSeparation(60);
    EcrireCentre("SUPER CALCULATIRCE");
    LigneSeparation(60);
    Console.WriteLine();

    EcrireCentre("Que souhaitez-vous faire ?");
    Console.WriteLine();

    EcrireCentre("1. Commencer à calculer");
    //EcrireCentre("2. Afficher l'historique"); // TODO: Check comment accéder aux précédents calculs
    EcrireCentre("2. Quitter le programme");

    Console.WriteLine();
    LigneSeparation(30);
    Console.Write(new string(' ', (Console.WindowWidth - "Votre choix : ".Length) / 2) + "Votre choix : ");
}

// Fonction pour vérifier si c'est un opérateur
static bool EstUnOperateur(string texte)
{
    return texte == "+" || texte == "-" || texte == "*" || texte == "/";
}

// Fonction pour calculer l'expression
static double CalculerExpression(List<string> expression)
{
    if (expression.Count < 3) return 0;

    double resultat = double.Parse(expression[0]);

    for (int i = 1; i < expression.Count; i += 2)
    {
        if (i + 1 < expression.Count)
        {
            string operateur = expression[i];
            double nombre = double.Parse(expression[i + 1]);

            switch (operateur)
            {
                case "+":
                    resultat += nombre;
                    break;
                case "-":
                    resultat -= nombre;
                    break;
                case "*":
                    resultat *= nombre;
                    break;
                case "/":
                    if (nombre != 0)
                        resultat /= nombre;
                    else
                    {
                        Console.WriteLine("Erreur : Division par zéro !");
                        return 0;
                    }
                    break;
            }
        }
    }

    return resultat;
}
