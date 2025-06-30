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
            AfficherTitre();

            continuer = true; 
            // Réinitialiser pour chaque nouveau calcul
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
                }else
                {
                    Console.Write(new string(' ', (Console.WindowWidth - "Entrez un opérateur (+, -, *, /) ou '=' pour calculer : ".Length) / 2) + "Entrez un opérateur (+, -, *, /) ou '=' pour calculer : ");
                    string operateur = Console.ReadLine();

                    if (operateur == "=")
                    {
                        // Calculer le résultat
                        double resultat = CalculerExpression(historiqueCalculs);
                        Console.WriteLine();
                        MesFonctions.EcrireCentre($"Résultat : {string.Join(" ", historiqueCalculs)} = {resultat:F4}");
                        Console.WriteLine();

                        // Demander si continuation
                        MesFonctions.EcrireCentre("Nouveau calcul ? (o/n) : ");
                        string reponse = Console.ReadLine()?.ToLower();

                        continuer = (reponse == "o" || reponse == "oui");

                        if (continuer)
                        {
                            historiqueCalculs.Clear(); // Reset pour nouveau calcul
                        }
                            

                    }else if (EstUnOperateur(operateur))
                    {
                        historiqueCalculs.Add(operateur);
                    }else
                    {
                        Console.WriteLine("Opérateur non valide !");
                        continue;
                    }
                }

                // Afficher l'historique actuel, TODO: Essaye de mettre en place un système de rafraîchissement
                if (historiqueCalculs.Count > 0)
                {
                    AfficherTitre();
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

// Fonction Afficher le titre Calculatrice
static void AfficherTitre()
{
    Console.Clear();
    MesFonctions.LigneSeparation();
    MesFonctions.EcrireCentre("Calculatrice");
    MesFonctions.LigneSeparation();
    Console.WriteLine();
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

// Fonction pour calculer
static double CalculerExpression(List<string> expression)
{
    // Si j'ai moins de 3 éléments, ce n'est pas un calcul valide
    // Exemple : ["5"] ou ["5", "+"] ne sont pas complets
    if (expression.Count < 3) return 0;

    // Je fais une COPIE de ma liste pour pouvoir la modifier sans perdre l'originale
    List<string> calcul = new List<string>(expression);
    
    // ÉTAPE 1 : Je m'occupe d'abord des × et ÷ (priorité haute)
    TraiterOperateurs(calcul, new string[] { "*", "/" });
    
    // ÉTAPE 2 : Ensuite je m'occupe des + et - (priorité basse)
    TraiterOperateurs(calcul, new string[] { "+", "-" });
    
    // À la fin, il ne reste qu'un seul nombre : mon résultat !
    return double.Parse(calcul[0]);
}

// Fonction qui traite certains opérateurs
static void TraiterOperateurs(List<string> calcul, string[] operateurs)
{
    // Je regarde chaque position d'opérateur (1, 3, 5, 7...)
    int position = 1;
    
    while (position < calcul.Count)
    {
        // Est-ce que l'opérateur à cette position m'intéresse ?
        bool operateurTrouve = false;
        foreach (string op in operateurs)
        {
            if (calcul[position] == op)
            {
                operateurTrouve = true;
                break;
            }
        }
        
        if (operateurTrouve)
        {
            // Je récupère : nombre AVANT, opérateur, nombre APRÈS
            double nombreAvant = double.Parse(calcul[position - 1]);
            string operateur = calcul[position];
            double nombreApres = double.Parse(calcul[position + 1]);
            
            // Je calcule selon l'opérateur
            double resultat = 0;
            if (operateur == "+") resultat = nombreAvant + nombreApres;
            if (operateur == "-") resultat = nombreAvant - nombreApres;
            if (operateur == "*") resultat = nombreAvant * nombreApres;
            if (operateur == "/") resultat = nombreAvant / nombreApres;
            
            // Je remplace les 3 éléments par le résultat
            calcul[position - 1] = resultat.ToString();  // Le résultat remplace le 1er nombre
            calcul.RemoveAt(position);                   // Je supprime l'opérateur
            calcul.RemoveAt(position);                   // Je supprime le 2ème nombre
            
            // Je ne bouge pas position car la liste a raccourci
        }
        else
        {
            // Cet opérateur ne m'intéresse pas, je passe au suivant
            position += 2;
        }
    }
}


