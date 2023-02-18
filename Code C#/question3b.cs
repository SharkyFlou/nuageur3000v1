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

      //fonction qui retourne le nouveau dictionnai
      Dictionary < string, int > dicRacine1 = transformation(cherchemot, "etape1.txt");
      Dictionary < string, int > dicRacine2 = transformation(dicRacine1, "etape2.txt");
      Dictionary < string, int > dicRacine3 = transformation(dicRacine2, "etape3.txt");
      //affiche nouveau dictionnaire
      dicRacine3 = trieBrutDico(dicRacine3);
      affiche_dictionnaire(dicRacine3);

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

  //renvoie true si Xmot finit par Xtermi, false sinon
  public static bool comparaison(string Xmot, string Xtermi) {
    bool renvoie = false;
    if (Xmot.Length > Xtermi.Length) { //verifie si taille du mot plus grand que la taille de la terminaison
      if ((Xmot.Substring(Xmot.Length - Xtermi.Length, Xtermi.Length)) == Xtermi) { //verifie si la terminaison est dans la fin du mot
        renvoie = true;
      }
    }
    return renvoie;
  }

  //renvoie le mot modifie, -1 si la contrainte VC n'est pas respecte
  public static string remplacement(string Xmot, string Xtermi, string Xparam, string nbr) {
    string nvmot = "";
    int repet = int.Parse(nbr);
    for (int a = 0; a < (Xmot.Length - Xtermi.Length); a++) { //creer le nouveau mot sans la terminaison, nous aurions pu aussi utiliser simplement Substring
      nvmot += Xmot[a];
    }
    if (Xparam != "epsilon") { //si colone 2 de etape 1 differente de epsilon, rajoute la colonne 2
      for (int b = 0; b < (Xparam.Length); b++) {
        nvmot += Xparam[b];
      }
    }
    if (compteVC(nvmot) <= repet) { //si la contrainte de repetition de VC n'est pas respecte, renvoie -1
      nvmot = "-1";
    }
    return nvmot;
  }

  //fonction qui retourne une liste avec toutes les terminaisons (etape 1)
  public static List < string[] > terminaison(string filepath) {
    List < string[] > l_terminaison = new List < string[] > ();

    StreamReader sr = File.OpenText(filepath);
    string ligne;

    //lecture du fichier + addition a la lsite
    while (!(sr.EndOfStream)) {
      ligne = sr.ReadLine();
      string[] lignedecoupe = ligne.Split(" ");
      //creation d'une liste de tableaux
      l_terminaison.Add(lignedecoupe);
    }
    return l_terminaison;
  }

  //fonction qui retourne un dictionnaire compose de racines
  public static Dictionary < string, int > transformation(Dictionary < string, int > dicOG, string filepath) {
    Dictionary < string, int > dicoRacine = new Dictionary < string, int > ();
    List < string[] > liste_terminaison1 = terminaison(filepath);
    //pour chaque mot deja traite
    foreach(KeyValuePair < string, int > val in dicOG) {
      string racine = val.Key;
      bool trouve = false;
      for (int i = 0; i < liste_terminaison1.Count && !trouve; i++) {
        //si le mot est comparable
        if (comparaison(val.Key, liste_terminaison1[i][1])) {

          //stockage de la racine
          racine = remplacement(val.Key, liste_terminaison1[i][1], liste_terminaison1[i][2], liste_terminaison1[i][0]);
          if (racine != "-1") {
            trouve = true;
          } else {
            racine = val.Key; //evite un bug ou cela renvoie -1 car derniere terminaison teste
          }

        }
      }
      remplitDico1(ref dicoRacine, racine, val.Value);

    }
    return dicoRacine;
  }

  //fonction qui remplit le dictionnaire de racines
  public static void remplitDico1(ref Dictionary < string, int > dico1, string Xracine, int Xval) {
    bool test = false;
    foreach(string val in dico1.Keys) { //si le mot trie auparavant correspond a la racine, comme un ContainsKey
      if (val == Xracine) {
        test = true;
      }
    }
    if (test) { //si le mot existe deja
      dico1[Xracine] += Xval;
    } else { //si le mot n'existe pas
      dico1.Add(Xracine, Xval);
    }

  }

  //renvoie le nombre de repitition de VC dans Xmot
  public static int compteVC(string Xmot) {
    //string de toutes les voyelles possibles
    string voyelles = "aäâàeéèëêiïîoöôuùüûyÿ"; //dans le preview de l'html les caractere precedent seront buge, car les voyelles avec accents sont aussi affiche ici
    int compteurVC = 0;
    int longueur = 0;
    bool voyelle, consonne;
    int motlong = Xmot.Length;

    while (longueur != motlong) {
      consonne = false;
      voyelle = false;
      while (longueur != motlong && charDansString(Xmot[longueur], voyelles)) { //cherche une consonne consonne, doit skip au moins une voyelle
        voyelle = true;
        longueur++;
      }
      while (longueur != motlong && !charDansString(Xmot[longueur], voyelles)) { //cherche une voyelle, doit skip au moins une consonne
        consonne = true;
        longueur++;
      }
      if (consonne && voyelle) {
        compteurVC++;
      }
    }

    return compteurVC;

  }

  //renvoie true si chara dans Xmot, false sinon
  public static bool charDansString(char chara, string Xmot) {
    bool trouve = false;
    for (int i = 0; i < Xmot.Length && !trouve; i++) {
      if (chara == Xmot[i]) {
        trouve = true;
      }
    }
    return trouve;
  }

}