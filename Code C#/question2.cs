using System;
using System.IO;
using System.Collections.Generic;

class nuageMot {
  static void Main() {
    Dictionary < string, int > cherchemot = new Dictionary < string, int > ();
    string fichier;
    Console.Write("Nom du fichier (avec .txt) : "); //avec ou sans arborescence
    fichier = Console.ReadLine();
    if (File.Exists(fichier) == true) {
      //remplit le dictionnaire
      cherchemot = remplitDico(fichier);

      //trie par ordre alphabetique
      //cherchemot=trieBrutDico(cherchemot);

      //affiche dictionnaire
      //affiche_dictionnaire(cherchemot);

      //fonction qui retourne le nouveau dictionnaire

      affiche_dictionnaire(cherchemot);

    } else {
      Console.WriteLine("Le fichier " + fichier + " n'exsite pas !");
    }
  }

  //renvoie un dictionnaire, avec en clef le mot, et en valeur le nombre d'apparition du mot dans le text;, en parametre le nom du fichier
  public static Dictionary < string, int > remplitDico(string filename) {
    Dictionary < string, int > newDico = new Dictionary < string, int > ();
    List < string > mot_inter = new List < string > ();

    //Lecture du texte
    StreamReader sr = File.OpenText(filename);
    string ligne = "";
    mot_inter = mot_interdit("mot_vide.txt");

    //ajoute les mots + ocurrences au dictionnaire
    while (!(sr.EndOfStream)) {
      ligne = sr.ReadLine();
      string[] lignedecoupe = ligne.Split(" ");

      //fait le traitement de chaque ligne
      newDico = traitement(newDico, lignedecoupe, mot_inter);

    }
    sr.Close();
    return newDico;
  }

  //fonction racine de remplitDico
  public static Dictionary < string, int > traitement(Dictionary < string, int > dic, string[] ligne, List < string > interdit) {
    foreach(string mot in ligne) {
      //normalise chaque mot de chaque ligne
      string motT = normalise(mot);

      //si deux mots sont divises par une apostrophe
      string[] test = motT.Split("'");

      foreach(string motY in test) {
        //si le mot n'est pas vide ET le mot n'est pas dans la liste des mots interdits
        if (motY != "" && !estDans(motY, interdit)) {
          if (dic.ContainsKey(motY)) {
            dic[motY] += 1;
          } else {
            dic.Add(motY, 1);
          }
        }
      }
    }
    return dic;
  }

  //fonction qui retourne la liste de tous les mots "vides"
  public static List < string > mot_interdit(string filename2) {
    List < string > listemotvide = new List < string > ();
    StreamReader sr2 = File.OpenText(filename2);
    string ligne;

    while (!(sr2.EndOfStream)) {
      ligne = sr2.ReadLine();
      listemotvide.Add(ligne);
    }
    sr2.Close();
    return listemotvide;
  }

  // Renvoie le mot mis en parametre normalise, sans "." ou "," ou  "'", mais garde les accents
  public static string normalise(string Xmot) {
    string mot2 = "";
    char lettre;

    for (int i = 0; i < Xmot.Length; i++) {
      if ((int) Xmot[i] >= 65 && (int) Xmot[i] <= 90) {
        lettre = (char)((int) Xmot[i] + 32); //mise en minuscule du characteres
        mot2 += lettre.ToString(); //ajout de la lettre en mini
      } else if (((int) Xmot[i] >= 145 && (int) Xmot[i] <= 148) || ((int) Xmot[i] == 8217)) { //transforme les "‘", les "’", les "“" et les "”" en "'" et le charactere chelou de 8217 (les guillemets)
        mot2 += (char)(39);
      } else if ((int) Xmot[i] >= 97 && (int) Xmot[i] <= 122) { //minuscules
        mot2 += Xmot[i].ToString();
      } else if ((int) Xmot[i] == 39 || (int) Xmot[i] == 45 || (int) Xmot[i] == 156) { //garde les "-" et les "'" et les "œ" (les oe colle))
        mot2 += Xmot[i].ToString();
      } else if ((int) Xmot[i] >= 192 && (int) Xmot[i] <= 221 && (int) Xmot[i] != 215) {
        lettre = (char)((int) Xmot[i] + 32); //mise en minuscule du characteres speciaux sauf le "×" (caractere de multiplication)
        mot2 += lettre.ToString();
      } else if ((int) Xmot[i] >= 224 && (int) Xmot[i] <= 255) {
        mot2 += Xmot[i].ToString(); //garde les lettres avec accents
      } else if ((int) Xmot[i] == 140) { //transforme "Œ" en "œ" (les OE colle et oe colle)
        mot2 += (char)(156);
      }
    }
    return mot2;
  }

  //affiche le dictionnaire
  public static void affiche_dictionnaire(Dictionary < string, int > Xtab) {
    foreach(KeyValuePair < string, int > val in Xtab) {
      Console.Write(val.Key + " ");
      for (int i = 0; i < 12 - val.Key.Length; i++) {
        Console.Write(" ");
      }
      Console.WriteLine(val.Value);
    }
  }

  //retourne true si le mot est dans la liste des mots interdits, false sinon
  public static bool estDans(string Xmot, List < string > Xliste) {
    bool trouve = false;
    for (int i = 0; i < Xliste.Count && !trouve; i++) {
      if (Xmot == Xliste[i]) {
        trouve = true;
      }
    }
    return trouve;
  }

  //trie le dictionnaire par ordre decroissant d'apparition(permet une meilleure lecture de celui-ci)
  public static Dictionary < string, int > trieBrutDico(Dictionary < string, int > dico) {
    string clefmax = "";
    int chiffremax = 99999999;

    Dictionary < string, int > dicotrie = new Dictionary < string, int > ();
    while (dico.Count > 0) {
      foreach(KeyValuePair < string, int > val in dico) {
        if (chiffremax > val.Value) {
          chiffremax = val.Value;
          clefmax = val.Key;
        }
      }
      dico.Remove(clefmax);
      dicotrie.Add(clefmax, chiffremax);
      chiffremax = 99999999;
    }
    return dicotrie;
  }
}
