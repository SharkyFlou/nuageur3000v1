using System;
using System.IO;
using System.Collections.Generic;

class nuageMot {

  //structure de l'apparition d'une racines, avec ses infos, toute es versions differents du mot apparru et le total d'apparition
  public struct mot {
    public string racine;
    public Dictionary < string,
      int > version;

    public mot(string Xracine, string Xversion, int Xnbr) {
      racine = Xracine;
      version = new Dictionary < string,
        int > ();
      version.Add(Xversion, Xnbr);
    }
  }

  static void Main() {
    Dictionary < string,
      int > cherchemot = new Dictionary < string,
      int > ();
    string fichier;
    Console.Write("Nom du fichier (avec .txt) : "); //avec ou sans arborescence
    fichier = Console.ReadLine();
    if (File.Exists(fichier) == true) {
      //remplit le dictionnaire
      //Console.WriteLine("0");
      cherchemot = remplitDico(fichier);
      //Console.WriteLine("1");

      //fonction qui retourne le nouveau dictionnaire
      if (File.Exists("etape1.txt") && File.Exists("etape2.txt") && File.Exists("etape1.txt") && File.Exists("mot_vide.txt")) {
        //Console.WriteLine("2");

        string[] etapes = new string[3] {
          "etape1.txt",
          "etape2.txt",
          "etape3.txt"
        };
        List < mot > ListM = transformation(cherchemot, etapes);
        //Console.WriteLine("3");
        //affiche nouveau dictionnaire
        ListM = trieBrutMot(ListM);
        //Console.WriteLine("4");
        afficheListMot(ListM);
        //Console.WriteLine("5");
      } else {
        Console.WriteLine("Il manque un ou plusieurs fichier etapes et/ou la liste de mot vide !");
      }

    } else {
      Console.WriteLine("Le fichier " + fichier + " n'existe pas !");
    }
  }

  //renvoie un dictionnaire, avec en clef le mot, et en valeur le nombre d'apparition du mot dans le text;, en parametre le nom du fichier
  public static Dictionary < string,
    int > remplitDico(string filename) {
      Dictionary < string,
        int > newDico = new Dictionary < string,
        int > ();
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
      } else if (((int) Xmot[i] >= 145 && (int) Xmot[i] <= 148) || ((int) Xmot[i] == 8217)) { //transforme les "‘", les "’", les "“" et les "”" en "'" et le charactere chelou de 8217
        mot2 += (char)(39);
      } else if ((int) Xmot[i] >= 97 && (int) Xmot[i] <= 122) { //minuscules
        mot2 += Xmot[i].ToString();
      } else if ((int) Xmot[i] == 39 || (int) Xmot[i] == 45 || (int) Xmot[i] == 156) { //garde les "-" et les "'" et les "œ"
        mot2 += Xmot[i].ToString();
      } else if ((int) Xmot[i] >= 192 && (int) Xmot[i] <= 221 && (int) Xmot[i] != 215) {
        lettre = (char)((int) Xmot[i] + 32); //mise en minuscule du characteres speciaux sauf le "×"
        mot2 += lettre.ToString();
      } else if ((int) Xmot[i] >= 224 && (int) Xmot[i] <= 255) {
        mot2 += Xmot[i].ToString(); //garde les lettres avec accents
      } else if ((int) Xmot[i] == 140) { //transforme "Œ" en "œ"
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
    for (int a = 0; a < (Xmot.Length - Xtermi.Length); a++) { //creer le nouveau mot sans la terminsaison
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
  public static List < mot > transformation(Dictionary < string, int > dicOG, string[] Xetapes) {
    List < mot > ListM = new List < mot > ();

    List < List < string[] >> liste_terminaison = new List < List < string[] >> ();
    foreach(string val in Xetapes) {
      List < string[] > tmp_liste_termi = terminaison(val);
      liste_terminaison.Add(tmp_liste_termi);
    }

    //pour chaque mot deja traite
    foreach(KeyValuePair < string, int > val in dicOG) {
      string racine = val.Key;
      foreach(List < string[] > etape in liste_terminaison) {
        racine = transRacine(racine, etape);
      }

      string tmpMot = val.Key;
      int tmpNbr = val.Value;
      remplitMot(ref ListM, tmpMot, racine, tmpNbr);

    }
    return ListM;
  }

  //fonction qui remplit le dictionnaire de racines
  public static void remplitDico1(ref Dictionary < string, int > dico1, string Xracine, int Xval) {
    bool test = false;
    foreach(string val in dico1.Keys) { //si le mot trie auparavant correspond a la racine
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

  //return i si Xracine dans les .racine de la liste de mot, -1 sinon
  public static int contientRacine(string Xracine, List < mot > ListM) {
    for (int i = 0; i < ListM.Count; i++) {
      if (ListM[i].racine == Xracine) {
        return i;
      }
    }
    return -1;
  }

  //renvoie true si la version existe dans le dico version du mot, false sinon
  public static bool contientVersion(string Xversion, mot Xmot) {
    foreach(string val in Xmot.version.Keys) {
      if (val == Xversion) {
        return true;
      }
    }
    return false;
  }

  //remplit la liste de chaque racine avec ses versions et le nb de fois ou ils apparaissent
  public static void remplitMot(ref List < mot > listefinal, string m, string racine, int nbfois) {

    int porte = contientRacine(racine, listefinal);
    //si la racine se trouve dans la liste des mots
    if (porte != -1) {
      //si la version existe deja
      if (contientVersion(m, listefinal[porte])) {
        listefinal[porte].version[m] += nbfois;
      } else {
        //creation de la version + son nb d'apparitions
        listefinal[porte].version.Add(m, nbfois);
      }
    }
    //si la racine n'apparait jamais
    else {
      mot motnouveau = new mot(racine, m, nbfois);
      listefinal.Add(motnouveau);
    }
  }

  //transforme la racine
  public static string transRacine(string Xmot, List < string[] > Xtermi) {
    bool trouve = false;
    string racine = Xmot;
    string racineTemp;
    for (int i = 0; i < Xtermi.Count && !trouve; i++) {
      //si le mot est comparable
      if (comparaison(Xmot, Xtermi[i][1])) {

        //stockage de la racine
        racineTemp = remplacement(Xmot, Xtermi[i][1], Xtermi[i][2], Xtermi[i][0]); //a modifier
        if (racineTemp != "-1") {
          racine = racineTemp;
          trouve = true;
        }
      }
    }
    return racine;
  }

  //fonction qui genere de l'html et du css du nuage de mot voulu
  public static void afficheListMot(List < mot > XlisteM) {
    int max = 0;
    int tempmax;
    string motmax = "fail ";
    int j = 1;
    Console.WriteLine("<ul class=\"nuage\">");
    List < string > final = new List < string > ();
    //Random rnd = new Random();
    for (int i = XlisteM.Count - 1; i >= 0; i--) {
      max = 0;
      foreach(string val in XlisteM[i].version.Keys) {
        tempmax = XlisteM[i].version[val];
        if (tempmax > max) {
          max = tempmax;
          motmax = val;
        }
      }

      final.Add("<li><a data-weight=\"" + j + "\" href=\"#\">" + motmax + "</a></li>");
      j++;

    }
    final = affiche_liste(final);
    foreach(string val in final) {
      Console.WriteLine(val);
    }
    Console.WriteLine("</ul>");
    Console.WriteLine("#########################");
    j = 1;
    for (j = 1; j <= XlisteM.Count; j++) {
      Console.WriteLine(".nuage a[data-weight=\"" + j + "\"] { --size: " + (j + j) + "; }");
    }
  }

  public static List < string > affiche_liste(List < string > doitafficher) {
    int n = doitafficher.Count;
    Random rnd = new Random();
    string temp;
    int counter = 0;
    while (n > 1 && counter < 25) {
      n--;
      int k = rnd.Next(n + 1);
      temp = doitafficher[k];
      doitafficher[k] = doitafficher[n];
      doitafficher[n] = temp;
      counter++;
    }
    return doitafficher;
  }

  //trie la liste finale pour une meilleure comprehension, et ne garde que les 25 premiers mots
  public static List < mot > trieBrutMot(List < mot > pastrie) {
    int nbr_max = 25;
    List < mot > trie = new List < mot > ();
    int tempmax;
    int max;
    int posmax;
    while (pastrie.Count > 0 && nbr_max > 0) {
      max = 0;
      posmax = 0;
      for (int i = 0; i < pastrie.Count; i++) {
        tempmax = 0;
        foreach(string val in pastrie[i].version.Keys) {
          tempmax += pastrie[i].version[val];
        }
        if (max < tempmax) {
          max = tempmax;
          posmax = i;
        }

      }
      trie.Add(pastrie[posmax]);
      pastrie.RemoveAt(posmax);
      nbr_max--;
    }
    //  trie=randomize(trie);
    return trie;
  }
}