using System;
using System.Collections.Generic;
using MesFonction;

Console.Title = "Calculatrice";

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
            AfficherTitre();
            continuer = true;

            // Réinitialiser pour chaque nouveau calcul
            historiqueCalculs.Clear();

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
                        historiqueCalculs.Add(FormaterNombre(nombre));
                    }
                    else
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
                        MesFonctions.EcrireCentre($"Résultat : {string.Join(" ", historiqueCalculs)} = {FormaterNombre(resultat)}");
                        Console.WriteLine();

                        // Demander si continuation
                        MesFonctions.EcrireCentre("Nouveau calcul ? (o/n) : ");
                        string reponse = Console.ReadLine()?.ToLower();

                        continuer = (reponse == "o" || reponse == "oui");

                        if (continuer)
                        {
                            historiqueCalculs.Clear();
                        }
                    }
                    else if (EstUnOperateur(operateur))
                    {
                        historiqueCalculs.Add(operateur);
                    }
                    else
                    {
                        Console.WriteLine("Opérateur non valide !");
                        continue;
                    }
                }

                // Afficher l'historique actuel
                if (historiqueCalculs.Count > 0)
                {
                    AfficherTitre();
                    Console.WriteLine();
                    MesFonctions.EcrireCentre("Calcul en cours : " + string.Join(" ", historiqueCalculs));
                    Console.WriteLine();
                }
            }
            break;

        case 2:
            Console.Clear();
            MesFonctions.LigneSeparation();
            MesFonctions.EcrireCentre("MERCI D'AVOIR UTILISE MA SUPER CALCULATRICE !");
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

Console.ReadKey(); // Car il ferme avant de voir le message de fermeture (bug i guess)

static string FormaterNombre(double nombre)
{
    if (nombre == Math.Floor(nombre))
        return nombre.ToString("0");
    else
        return nombre.ToString("0.####");
}

// Fonction Afficher le titre "Calculatrice"
static void AfficherTitre()
{
    Console.Clear();
    MesFonctions.LigneSeparation();
    MesFonctions.EcrireCentre("Calculatrice");
    MesFonctions.LigneSeparation();
    Console.WriteLine();
}

// Fonction pour vérifier si c'est un opérateur utiliser
static bool EstUnOperateur(string texte)
{
    return texte == "+" || texte == "-" || texte == "*" || texte == "/";
}

// Fonction pour calculer
static double CalculerExpression(List<string> expression)
{
    // au moins 3 éléments
    if (expression.Count < 3) return 0;

    // je copie ma liste
    List<string> calcul = new List<string>(expression);

    // premièrement, les * et / 
    FaireOperations(calcul, "*");
    FaireOperations(calcul, "/");

    // ensuite le reste (+ et - dans notre cas)
    FaireOperations(calcul, "+");
    FaireOperations(calcul, "-");

    return double.Parse(calcul[0]);
}

static void FaireOperations(List<string> calcul, string operateur)
{
    // boucle calculer les nombres
    for (int i = 0; i < calcul.Count; i++)
    {
        if (calcul[i] == operateur)
        {
            // Je prends les nombres collé à l'opérateur
            double nombre1 = double.Parse(calcul[i - 1]);
            double nombre2 = double.Parse(calcul[i + 1]);

            double resultat = 0;
            if (operateur == "+") resultat = nombre1 + nombre2;
            if (operateur == "-") resultat = nombre1 - nombre2;
            if (operateur == "*") resultat = nombre1 * nombre2;
            if (operateur == "/") resultat = nombre1 / nombre2;

            // Je remplace et je supprime
            calcul[i - 1] = FormaterNombre(resultat);
            calcul.RemoveAt(i);
            calcul.RemoveAt(i);

            // boucle pour revenir au nombres précédent 
            i = -1;
        }
    }
}
