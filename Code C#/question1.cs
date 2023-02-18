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
      cherchemot = remplitDico(fichier);
      affiche_dictionnaire(cherchemot);
    } else {
      Console.WriteLine("Le fichier " + fichier + " n'exsite pas !");
    }
  }

  //renvoie un dictionnaire, avec en clef le mot, et en valeur le nombre d'apparition du mot dans le texte, en parametre le nom du fichier
  public static Dictionary < string, int > remplitDico(string filename) {
    Dictionary < string, int > newDico = new Dictionary < string, int > ();

    StreamReader sr = File.OpenText(filename);
    string ligne = "";

    while (!(sr.EndOfStream)) {
      ligne = sr.ReadLine();
      string[] lignedecoupe = ligne.Split(" ");
      foreach(string mot in lignedecoupe) {
        string motT = normalise(mot);
        if (motT != "") {
          if (newDico.ContainsKey(motT)) {
            newDico[motT] += 1;
          } else {
            newDico.Add(motT, 1);
          }
        }
      }
    }
    sr.Close();
    return newDico;
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

  public static void affiche_dictionnaire(Dictionary < string, int > Xtab) {
    foreach(KeyValuePair < string, int > val in Xtab) {
      Console.WriteLine(val.Key + " : " + val.Value);
    }
  }
}