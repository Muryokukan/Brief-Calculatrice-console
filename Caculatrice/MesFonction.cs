using System;

namespace MesFonction { 

    public static class MesFonctions
    {
        // Fonction pour centrer le texte dans la console
        public static void EcrireCentre(string texte)
        {
            int largeurConsole = Console.WindowWidth;
            int espaces = (largeurConsole - texte.Length) / 2;
            Console.WriteLine(new string(' ', Math.Max(0, espaces)) + texte);
        }

        // Fonction pour créer une ligne de séparation au centre
        public static void LigneSeparation(int longueur = 50)
        {
            EcrireCentre(new string('-', longueur));
        }

        // Fonction pour afficher le menu principal
        public static void AfficherMenu()
        {
            Console.Clear();
            LigneSeparation(60);
            EcrireCentre("SUPER CALCULATIRCE");
            LigneSeparation(60);
            Console.WriteLine();

            EcrireCentre("Que souhaitez-vous faire ?");
            Console.WriteLine();

            EcrireCentre("1. Commencer à calculer");
            EcrireCentre("2. Quitter le programme");

            Console.WriteLine();
            LigneSeparation(30);
            Console.Write(new string(' ', (Console.WindowWidth - "Votre choix : ".Length) / 2) + "Votre choix : ");
        }
    }
}
