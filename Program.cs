using System;
using System.ComponentModel;
using System.Diagnostics.Metrics;
using System.Diagnostics;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace projekt
{
    class Program
    {
        static readonly Stopwatch timer = new Stopwatch();

        public static int wprowadzDaneMenu()
        {
            try
            {
                Console.Write("> ");
                int x = Convert.ToInt32(Console.ReadLine());

                if (x < 1 || x > 6)
                {
                    Console.WriteLine("Użyto niepoprawnego formatu. Spróbuj ponownie");
                    return wprowadzDaneMenu();
                }

                return x;
            }
            catch (System.FormatException)
            {
                Console.WriteLine("Użyto niepoprawnego formatu. Spróbuj ponownie");
                return wprowadzDaneMenu();
            }
        }

        public static int wprowadzDaneInt()
        {
            try
            {
                Console.Write("> ");
                int x = Convert.ToInt32(Console.ReadLine());

                if (x < 0)
                {
                    Console.WriteLine("Użyto niepoprawnego formatu. Spróbuj ponownie");
                    return wprowadzDaneInt();
                }

                return x;
            }
            catch (System.FormatException)
            {
                Console.WriteLine("Użyto niepoprawnego formatu. Spróbuj ponownie");
                return wprowadzDaneInt();
            }
        }

        public static double wprowadzDaneDouble()
        {
            try
            {
                Console.Write("> ");
                double x = Double.Parse(Console.ReadLine());

                if (x < 0)
                {
                    Console.WriteLine("Użyto niepoprawnego formatu. Spróbuj ponownie:");
                    return wprowadzDaneDouble();
                }

                return x;
            }
            catch (System.FormatException ex)
            {
                Console.WriteLine("Użyto niepoprawnego formatu. Spróbuj ponownie:");
                return wprowadzDaneDouble();
            }
        }

        public static string wprowadzDaneString()
        {
            try
            {
                Console.Write("> ");
                string x = Console.ReadLine();

                return x;
            }
            catch (System.FormatException ex)
            {
                Console.WriteLine("Użyto niepoprawnego formatu. Spróbuj ponownie:");
                return wprowadzDaneString();
            }
        }

        static void Main(string[] args)
        {
            Wezel korzen = null;

            while (true)
            {
                Console.WriteLine("\nDokonaj wyboru: ");
                Console.WriteLine("1 - Wstaw element");
                Console.WriteLine("2 - Usuń element");
                Console.WriteLine("3 - Wyszukaj element");
                Console.WriteLine("4 - Wyszukaj elementy o konkretnej części całkowitej");
                Console.WriteLine("5 - Wyświetl drzewo");
                Console.WriteLine("6 - Załaduj skrypt poleceń");

                int wybor = wprowadzDaneMenu();

                switch (wybor)
                {
                    case 1:
                        Console.WriteLine("Podaj element do wstawienia: ");
                        double do_wstawienia = wprowadzDaneDouble();

                        WstawElement.Wstaw(ref korzen, do_wstawienia);

                        break;
                    case 2:
                        Console.WriteLine("Podaj element do usunięcia: ");
                        double do_usuniecia = wprowadzDaneDouble();

                        UsunElement.Usun(ref korzen, do_usuniecia);

                        break;
                    case 3:
                        Console.WriteLine("Podaj element do odszukania: ");
                        double szukana = wprowadzDaneDouble();

                        bool wynik = SzukajElement.Szukaj(ref korzen, szukana);

                        if (wynik)
                        {
                            Console.WriteLine("Znaleziono");
                        }
                        else
                        {
                            Console.WriteLine("Nie znaleziono");
                        }

                        break;
                    case 4:
                        Console.WriteLine("Podaj szukaną część całkowitą: ");
                        int szukana_calkowita = wprowadzDaneInt();

                        int liczba_szukanych = SzukajCzescCalkowita.Licz(ref korzen, szukana_calkowita);

                        if (liczba_szukanych == 1)
                        {
                            Console.WriteLine("Znaleziono " + liczba_szukanych + " liczbę o części całkowitej " + szukana_calkowita);
                        }
                        else if (liczba_szukanych > 1 && liczba_szukanych < 5)
                        {
                            Console.WriteLine("Znaleziono " + liczba_szukanych + " liczby o części całkowitej " + szukana_calkowita);
                        }
                        else
                        {
                            Console.WriteLine("Znaleziono " + liczba_szukanych + " liczb o części całkowitej " + szukana_calkowita);
                        }

                        break;
                    case 5:
                        WypiszDrzewo.Wypisz(ref korzen);

                        break;
                    case 6:
                        string nazwa;

                        while (true)
                        {
                            Console.WriteLine("\nWprowadź nazwę pliku:");
                            nazwa = wprowadzDaneString();

                            if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + nazwa + ".txt"))
                            {
                                break;
                            }
                            else
                            {
                                Console.WriteLine("Plik nie istnieje\n");
                                continue;
                            }
                        }

                        string path = AppDomain.CurrentDomain.BaseDirectory + nazwa + ".txt";

                        int l_operacji = 0;
                        timer.Reset();
                        timer.Start();

                        using (StreamReader file = new StreamReader(path))
                        {
                            string ln = file.ReadLine();
                            l_operacji = Convert.ToInt32(ln);

                            for (int i = 0; i < l_operacji; i++)
                            {
                                if ((ln = file.ReadLine()) != null)
                                {
                                    String[] linia = ln.Split(" ", StringSplitOptions.RemoveEmptyEntries);

                                    string operacja = linia[0];
                                    double operand = Convert.ToDouble(linia[1]);

                                    switch (operacja)
                                    {
                                        case "W":
                                            WstawElement.Wstaw(ref korzen, operand);
                                            break;

                                        case "U":
                                            UsunElement.Usun(ref korzen, operand);
                                            break;

                                        case "S":
                                            bool wynik_S = SzukajElement.Szukaj(ref korzen, operand);

                                            if (wynik_S)
                                            {
                                                Console.WriteLine("Znaleziono");
                                            }
                                            else
                                            {
                                                Console.WriteLine("Nie znaleziono");
                                            }
                                            break;

                                        case "L":
                                            int operand_L = Convert.ToInt32(operand);
                                            int wynik_L = SzukajCzescCalkowita.Licz(ref korzen, operand_L);

                                            if (wynik_L == 1)
                                            {
                                                Console.WriteLine("Znaleziono " + wynik_L + " liczbę o części całkowitej " + operand_L);
                                            }
                                            else if (wynik_L > 1 && wynik_L < 5)
                                            {
                                                Console.WriteLine("Znaleziono " + wynik_L + " liczby o części całkowitej " + operand_L);
                                            }
                                            else
                                            {
                                                Console.WriteLine("Znaleziono " + wynik_L + " liczb o części całkowitej " + operand_L);
                                            }

                                            break;

                                        default:
                                            break;
                                    }
                                }
                            }

                            file.Close();
                        }

                        timer.Stop();
                        Console.WriteLine("\nWykonano skrypt w czasie " + timer.Elapsed.ToString());

                        break;
                    default:
                        break;
                }
            }//*/
        }
    }

    public class Wezel
    {
        public double? Liczba { get; set; }
        public Wezel Lewy { get; set; }
        public Wezel Prawy { get; set; }
        public int? Waga { get; set; }

        public Wezel(double? liczba)
        {
            this.Liczba = liczba;
            this.Lewy = null;
            this.Prawy = null;
            this.Waga = 0;
        }

        public Wezel(Wezel wezel)
        {
            if (wezel != null)
            {
                this.Liczba = wezel.Liczba;
                this.Lewy = wezel.Lewy;
                this.Prawy = wezel.Prawy;
                this.Waga = wezel.Waga;
            }
        }

        public void Delete()
        {
            this.Liczba = null;
            this.Lewy = null;
            this.Prawy = null;
            this.Waga = null;
        }
    }

    public class WstawElement
    {
        public static void Wstaw(ref Wezel korzen, double do_wstawienia)
        {
            if (korzen == null)
            {
                korzen = new Wezel(do_wstawienia);
                return;
            }
            else if (korzen.Liczba == null)
            {
                korzen.Liczba = do_wstawienia;
                korzen.Waga = 0;
                return;
            }

            bool CzyWstawiony = SzukajElement.Szukaj(ref korzen, do_wstawienia);

            if (!CzyWstawiony)
            {
                bool aktualizuj = false;
                Rekurencja(ref korzen, do_wstawienia, ref aktualizuj, ref korzen);
            }
        }

        private static void Rekurencja(ref Wezel wezel, double do_wstawienia, ref bool aktualizuj, ref Wezel korzen)
        {
            if (wezel == null)
            {
                return;
            }
            else if (wezel.Liczba == null)
            {
                wezel.Liczba = do_wstawienia;
                wezel.Waga = 0;
                return;
            }

            if (wezel.Liczba > do_wstawienia)
            {
                if (wezel.Lewy == null)
                {
                    wezel.Lewy = new Wezel(do_wstawienia);
                    aktualizuj = true;
                }
                Wezel x = wezel.Lewy;
                Rekurencja(ref x, do_wstawienia, ref aktualizuj, ref korzen);

                if (aktualizuj) wezel.Waga++;
                if (wezel.Waga == 0) aktualizuj = false;
            }
            else if (wezel.Liczba < do_wstawienia)
            {
                if (wezel.Prawy == null)
                {
                    wezel.Prawy = new Wezel(do_wstawienia);
                    aktualizuj = true;
                }
                Wezel y = wezel.Prawy;
                Rekurencja(ref y, do_wstawienia, ref aktualizuj, ref korzen);

                if (aktualizuj) wezel.Waga--;
                if (wezel.Waga == 0) aktualizuj = false;
            }

            if (wezel.Waga == 2 || wezel.Waga == -2) Rotacje.RotujWstawianie(ref wezel, ref korzen, ref aktualizuj);
        }
    }

    public class Rotacje
    {
        public static void RotujWstawianie(ref Wezel wezel, ref Wezel korzen, ref bool aktualizuj)
        {
            if (wezel.Lewy != null) // Rotacja RR
            {
                if (wezel.Waga == 2 && wezel.Lewy.Waga == 1)
                {
                    aktualizuj = false;
                    if (wezel == korzen)
                    {
                        Wezel lewy_prawy = korzen.Lewy.Prawy;

                        Wezel k = korzen;
                        korzen = korzen.Lewy;
                        korzen.Prawy = k;
                        k.Lewy = lewy_prawy;

                        korzen.Waga = 0;
                        korzen.Prawy.Waga = 0;
                    }
                    else
                    {
                        Wezel ojciec = SzukajElement.SzukajOjca(ref korzen, Convert.ToDouble(wezel.Liczba));
                        bool l_r = false; //Lewy - False, Prawy - True

                        if (ojciec.Prawy != null)
                        {
                            if (ojciec.Prawy.Liczba == wezel.Liczba)
                            {
                                l_r = true;
                            }
                        }

                        Wezel lewy_prawy = wezel.Lewy.Prawy;

                        Wezel k = wezel;
                        wezel = wezel.Lewy;
                        wezel.Prawy = k;
                        k.Lewy = lewy_prawy;

                        wezel.Waga = 0;
                        wezel.Prawy.Waga = 0;

                        if (l_r == false)
                        {
                            ojciec.Lewy = wezel;
                        }
                        else
                        {
                            ojciec.Prawy = wezel;
                        }
                    }
                }
            }

            if (wezel.Prawy != null) // Rotacja LL
            {
                if (wezel.Waga == -2 && wezel.Prawy.Waga == -1)
                {
                    aktualizuj = false;
                    if (wezel == korzen)
                    {
                        Wezel prawy_lewy = wezel.Prawy.Lewy;

                        Wezel k = wezel;
                        korzen = korzen.Prawy;
                        korzen.Lewy = k;
                        k.Prawy = prawy_lewy;
                        korzen.Waga = 0;
                        korzen.Lewy.Waga = 0;
                    }
                    else
                    {
                        Wezel ojciec = SzukajElement.SzukajOjca(ref korzen, Convert.ToDouble(wezel.Liczba));
                        bool l_r = false; //Lewy - False, Prawy - True

                        if (ojciec.Prawy != null)
                        {
                            if (ojciec.Prawy.Liczba == wezel.Liczba)
                            {
                                l_r = true;
                            }
                        }

                        Wezel prawy_lewy = wezel.Prawy.Lewy;

                        Wezel k = wezel;
                        wezel = wezel.Prawy;
                        wezel.Lewy = k;
                        k.Prawy = prawy_lewy;
                        wezel.Waga = 0;
                        wezel.Lewy.Waga = 0;

                        if (l_r == false)
                        {
                            ojciec.Lewy = wezel;
                        }
                        else
                        {
                            ojciec.Prawy = wezel;
                        }
                    }
                }
            }

            if (wezel.Lewy != null) //Rotacja LR
            {
                if (wezel.Waga == 2 && wezel.Lewy.Waga == -1)
                {
                    aktualizuj = false;

                    bool czy_korzen = (wezel == korzen);
                    int c = Convert.ToInt32(wezel.Lewy.Prawy.Waga);

                    //Rotacja LL
                    Wezel wezelLL = wezel.Lewy;

                    Wezel ojciec = wezel;
                    bool l_r = false; //Lewy - False, Prawy - True

                    /*if (ojciec.Prawy != null)
                    {
                        if (ojciec.Prawy.Liczba == wezelLL.Liczba)
                        {
                            l_r = true;
                        }
                    }*/

                    Wezel prawy_lewy = wezelLL.Prawy.Lewy;

                    Wezel k = wezelLL;
                    wezelLL = wezelLL.Prawy;
                    wezelLL.Lewy = k;
                    k.Prawy = prawy_lewy;
                    wezelLL.Waga = 0;
                    wezelLL.Lewy.Waga = 0;

                    if (l_r == false)
                    {
                        ojciec.Lewy = wezelLL;
                    }
                    else
                    {
                        ojciec.Prawy = wezelLL;
                    }

                    //Rotacja RR

                    if (wezel == korzen)
                    {
                        Wezel lewy_prawy = wezel.Lewy.Prawy;

                        Wezel k2 = wezel;
                        korzen = wezel.Lewy;
                        k2.Lewy = null;
                        korzen.Prawy = k2;
                        k2.Lewy = lewy_prawy;
                        korzen.Waga = 0;
                        korzen.Prawy.Waga = 0;
                    }
                    else
                    {
                        Wezel ojciec2 = SzukajElement.SzukajOjca(ref korzen, Convert.ToDouble(wezel.Liczba));
                        bool l_r2 = false; //Lewy - False, Prawy - True

                        if (ojciec2.Prawy != null)
                        {
                            if (ojciec2.Prawy.Liczba == wezel.Liczba)
                            {
                                l_r2 = true;
                            }
                        }

                        Wezel lewy_prawy = wezel.Lewy.Prawy;

                        Wezel k2 = wezel;
                        wezel = wezel.Lewy;
                        wezel.Prawy = k2;
                        k2.Lewy = lewy_prawy;
                        wezel.Waga = 0;
                        wezel.Prawy.Waga = 0;

                        if (l_r2 == false)
                        {
                            ojciec2.Lewy = wezel;
                        }
                        else
                        {
                            ojciec2.Prawy = wezel;
                        }
                    }


                    if (czy_korzen)
                    {
                        if (c == 0)
                        {
                            korzen.Prawy.Waga = 0;
                            korzen.Lewy.Waga = 0;
                        }
                        else if (c == -1)
                        {
                            korzen.Prawy.Waga = 0;
                            korzen.Lewy.Waga = 1;
                        }
                        else if (c == 1)
                        {
                            korzen.Prawy.Waga = -1;
                            korzen.Lewy.Waga = 0;
                        }
                    }
                    else
                    {
                        if (c == 0)
                        {
                            wezel.Prawy.Waga = 0;
                            wezel.Lewy.Waga = 0;
                        }
                        else if (c == -1)
                        {
                            wezel.Prawy.Waga = 0;
                            wezel.Lewy.Waga = 1;
                        }
                        else if (c == 1)
                        {
                            wezel.Prawy.Waga = -1;
                            wezel.Lewy.Waga = 0;
                        }
                    }
                }
            }

            if (wezel.Prawy != null) //Rotacja RL
            {
                if (wezel.Waga == -2 && wezel.Prawy.Waga == 1)
                {
                    aktualizuj = false;

                    bool czy_korzen = (wezel == korzen);
                    int c = Convert.ToInt32(wezel.Prawy.Lewy.Waga);

                    //Rotacja RR
                    Wezel wezelRR = wezel.Prawy;

                    Wezel ojciec = wezel;
                    bool l_r = true; //Lewy - False, Prawy - True

                    /*if (ojciec.Prawy != null)
                    {
                        if (ojciec.Prawy.Liczba == wezelRR.Liczba)
                        {
                            l_r = true;
                        }
                    }*/

                    Wezel lewy_prawy = wezelRR.Lewy.Prawy;

                    Wezel k = wezelRR;
                    wezelRR = wezelRR.Lewy;
                    wezelRR.Prawy = k;
                    k.Lewy = lewy_prawy;
                    wezelRR.Waga = 0;
                    wezelRR.Prawy.Waga = 0;

                    if (l_r == false)
                    {
                        ojciec.Lewy = wezelRR;
                    }
                    else
                    {
                        ojciec.Prawy = wezelRR;
                    }

                    //Rotacja LL

                    if (wezel == korzen)
                    {
                        Wezel prawy_lewy = wezel.Prawy.Lewy;

                        Wezel k2 = wezel;
                        korzen = korzen.Prawy;
                        korzen.Lewy = k2;
                        k2.Prawy = prawy_lewy;
                        korzen.Waga = 0;
                        korzen.Lewy.Waga = 0;
                    }
                    else
                    {
                        Wezel ojciec2 = SzukajElement.SzukajOjca(ref korzen, Convert.ToDouble(wezel.Liczba));
                        bool l_r2 = false; //Lewy - False, Prawy - True

                        if (ojciec2.Prawy != null)
                        {
                            if (ojciec2.Prawy.Liczba == wezel.Liczba)
                            {
                                l_r2 = true;
                            }
                        }

                        Wezel prawy_lewy = wezel.Prawy.Lewy;

                        Wezel k2 = wezel;
                        wezel = wezel.Prawy;
                        wezel.Lewy = k2;
                        k2.Prawy = prawy_lewy;
                        wezel.Waga = 0;
                        wezel.Lewy.Waga = 0;

                        if (l_r2 == false)
                        {
                            ojciec2.Lewy = wezel;
                        }
                        else
                        {
                            ojciec2.Prawy = wezel;
                        }
                    }

                    if (czy_korzen)
                    {
                        if (c == 0)
                        {
                            korzen.Prawy.Waga = 0;
                            korzen.Lewy.Waga = 0;
                        }
                        else if (c == -1)
                        {
                            korzen.Prawy.Waga = 0;
                            korzen.Lewy.Waga = 1;
                        }
                        else if (c == 1)
                        {
                            korzen.Prawy.Waga = -1;
                            korzen.Lewy.Waga = 0;
                        }
                    }
                    else
                    {
                        if (c == 0)
                        {
                            if (wezel.Prawy != null) wezel.Prawy.Waga = 0;
                            if (wezel.Lewy != null) wezel.Lewy.Waga = 0;
                        }
                        else if (c == -1)
                        {
                            if (wezel.Prawy != null) wezel.Prawy.Waga = 0;
                            if (wezel.Lewy != null) wezel.Lewy.Waga = 1;
                        }
                        else if (c == 1)
                        {
                            if (wezel.Prawy != null) wezel.Prawy.Waga = -1;
                            if (wezel.Lewy != null) wezel.Lewy.Waga = 0;
                        }
                    }
                }
            }

        }

        public static void RotujUsuwanie(ref Wezel wezel, ref Wezel korzen)
        {
            if (wezel.Lewy != null) // Rotacja RR
            {
                if (wezel.Waga == 2 && wezel.Lewy.Waga >= 0)
                {
                    if (wezel == korzen)
                    {
                        Wezel lewy_prawy = wezel.Lewy.Prawy;

                        Wezel k = wezel;
                        korzen = wezel.Lewy;
                        k.Lewy = null;
                        korzen.Prawy = k;
                        k.Lewy = lewy_prawy;
                        korzen.Waga = 0;
                        korzen.Prawy.Waga = 0;
                    }
                    else
                    {
                        Wezel ojciec = SzukajElement.SzukajOjca(ref korzen, Convert.ToDouble(wezel.Liczba));
                        bool l_r = false; //Lewy - False, Prawy - True

                        if (ojciec.Prawy != null)
                        {
                            if (ojciec.Prawy.Liczba == wezel.Liczba)
                            {
                                l_r = true;
                            }
                        }

                        Wezel lewy_prawy = wezel.Lewy.Prawy;

                        Wezel k = wezel;
                        wezel = wezel.Lewy;
                        wezel.Prawy = k;
                        k.Lewy = lewy_prawy;
                        wezel.Waga = 0;
                        wezel.Prawy.Waga = 0;

                        if (l_r == false)
                        {
                            ojciec.Lewy = wezel;
                        }
                        else
                        {
                            ojciec.Prawy = wezel;
                        }
                    }
                }
            }

            if (wezel.Prawy != null) // Rotacja LL
            {
                if (wezel.Waga == -2 && wezel.Prawy.Waga <= 0)
                {
                    if (wezel == korzen)
                    {
                        Wezel prawy_lewy = wezel.Prawy.Lewy;

                        Wezel k = wezel;
                        korzen = korzen.Prawy;
                        korzen.Lewy = k;
                        k.Prawy = prawy_lewy;
                        korzen.Waga = 0;
                        korzen.Lewy.Waga = 0;
                    }
                    else
                    {
                        Wezel ojciec = SzukajElement.SzukajOjca(ref korzen, Convert.ToDouble(wezel.Liczba));
                        bool l_r = false; //Lewy - False, Prawy - True

                        if (ojciec.Prawy != null)
                        {
                            if (ojciec.Prawy.Liczba == wezel.Liczba)
                            {
                                l_r = true;
                            }
                        }

                        Wezel prawy_lewy = wezel.Prawy.Lewy;

                        Wezel k = wezel;
                        wezel = wezel.Prawy;
                        wezel.Lewy = k;
                        k.Prawy = prawy_lewy;
                        wezel.Waga = 0;
                        wezel.Lewy.Waga = 0;

                        if (l_r == false)
                        {
                            ojciec.Lewy = wezel;
                        }
                        else
                        {
                            ojciec.Prawy = wezel;
                        }
                    }
                }
            }

            if (wezel.Lewy != null) //Rotacja LR
            {
                if (wezel.Waga == 2 && wezel.Lewy.Waga == -1)
                {
                    bool czy_korzen = (wezel == korzen);
                    int c = Convert.ToInt32(wezel.Lewy.Prawy.Waga);

                    //Rotacja LL
                    Wezel wezelLL = wezel.Lewy;

                    Wezel ojciec = wezel;
                    bool l_r = false; //Lewy - False, Prawy - True

                    /*if (ojciec.Prawy != null)
                    {
                        if (ojciec.Prawy.Liczba == wezelLL.Liczba)
                        {
                            l_r = true;
                        }
                    }*/

                    Wezel prawy_lewy = wezelLL.Prawy.Lewy;

                    Wezel k = wezelLL;
                    wezelLL = wezelLL.Prawy;
                    wezelLL.Lewy = k;
                    k.Prawy = prawy_lewy;
                    wezelLL.Waga = 0;
                    wezelLL.Lewy.Waga = 0;

                    if (l_r == false)
                    {
                        ojciec.Lewy = wezelLL;
                    }
                    else
                    {
                        ojciec.Prawy = wezelLL;
                    }

                    //Rotacja RR

                    if (czy_korzen)
                    {
                        Wezel lewy_prawy = korzen.Lewy.Prawy;

                        Wezel k2 = korzen;
                        korzen = korzen.Lewy;
                        korzen.Prawy = k2;
                        k2.Lewy = lewy_prawy;

                        korzen.Waga = 0;
                        korzen.Prawy.Waga = 0;
                    }
                    else
                    {
                        Wezel ojciec2 = SzukajElement.SzukajOjca(ref korzen, Convert.ToDouble(wezel.Liczba));
                        bool l_r2 = false; //Lewy - False, Prawy - True

                        if (ojciec2.Prawy != null)
                        {
                            if (ojciec2.Prawy.Liczba == wezel.Liczba)
                            {
                                l_r2 = true;
                            }
                        }

                        Wezel lewy_prawy = wezel.Lewy.Prawy;

                        Wezel k2 = wezel;
                        wezel = wezel.Lewy;
                        wezel.Prawy = k2;
                        k2.Lewy = lewy_prawy;
                        wezel.Waga = 0;
                        wezel.Prawy.Waga = 0;

                        if (l_r2 == false)
                        {
                            ojciec2.Lewy = wezel;
                        }
                        else
                        {
                            ojciec2.Prawy = wezel;
                        }
                    }

                    if (czy_korzen)
                    {
                        if (c == 0)
                        {
                            korzen.Prawy.Waga = 0;
                            korzen.Lewy.Waga = 0;
                        }
                        else if (c == -1)
                        {
                            korzen.Prawy.Waga = 0;
                            korzen.Lewy.Waga = 1;
                        }
                        else if (c == 1)
                        {
                            korzen.Prawy.Waga = -1;
                            korzen.Lewy.Waga = 0;
                        }
                    }
                    else
                    {
                        if (c == 0)
                        {
                            wezel.Prawy.Waga = 0;
                            wezel.Lewy.Waga = 0;
                        }
                        else if (c == -1)
                        {
                            wezel.Prawy.Waga = 0;
                            wezel.Lewy.Waga = 1;
                        }
                        else if (c == 1)
                        {
                            wezel.Prawy.Waga = -1;
                            wezel.Lewy.Waga = 0;
                        }
                    }
                }
            }

            if (wezel.Prawy != null) //Rotacja RL
            {
                if (wezel.Waga == -2 && wezel.Prawy.Waga == 1)
                {
                    bool czy_korzen = (wezel == korzen);
                    int c = Convert.ToInt32(wezel.Prawy.Lewy.Waga);

                    //Rotacja RR
                    Wezel wezelRR = wezel.Prawy;

                    Wezel ojciec = wezel;
                    bool l_r = true; //Lewy - False, Prawy - True

                    /*if (ojciec.Prawy != null)
                    {
                        if (ojciec.Prawy.Liczba == wezelRR.Liczba)
                        {
                            l_r = true;
                        }
                    }*/

                    Wezel lewy_prawy = wezelRR.Lewy.Prawy;

                    Wezel k = wezelRR;
                    wezelRR = wezelRR.Lewy;
                    wezelRR.Prawy = k;
                    k.Lewy = lewy_prawy;
                    wezelRR.Waga = 0;
                    wezelRR.Prawy.Waga = 0;

                    if (l_r == false)
                    {
                        ojciec.Lewy = wezelRR;
                    }
                    else
                    {
                        ojciec.Prawy = wezelRR;
                    }

                    //Rotacja LL

                    if (wezel == korzen)
                    {
                        Wezel prawy_lewy = wezel.Prawy.Lewy;

                        Wezel k2 = wezel;
                        korzen = korzen.Prawy;
                        korzen.Lewy = k2;
                        k2.Prawy = prawy_lewy;
                        korzen.Waga = 0;
                        korzen.Lewy.Waga = 0;
                    }
                    else
                    {
                        Wezel ojciec2 = SzukajElement.SzukajOjca(ref korzen, Convert.ToDouble(wezel.Liczba));
                        bool l_r2 = false; //Lewy - False, Prawy - True

                        if (ojciec2.Prawy != null)
                        {
                            if (ojciec2.Prawy.Liczba == wezel.Liczba)
                            {
                                l_r2 = true;
                            }
                        }

                        Wezel prawy_lewy = wezel.Prawy.Lewy;

                        Wezel k2 = wezel;
                        wezel = wezel.Prawy;
                        wezel.Lewy = k2;
                        k2.Prawy = prawy_lewy;
                        wezel.Waga = 0;
                        wezel.Lewy.Waga = 0;

                        if (l_r2 == false)
                        {
                            ojciec2.Lewy = wezel;
                        }
                        else
                        {
                            ojciec2.Prawy = wezel;
                        }
                    }

                    if (czy_korzen)
                    {
                        if (c == 0)
                        {
                            korzen.Prawy.Waga = 0;
                            korzen.Lewy.Waga = 0;
                        }
                        else if (c == -1)
                        {
                            korzen.Prawy.Waga = 0;
                            korzen.Lewy.Waga = 1;
                        }
                        else if (c == 1)
                        {
                            korzen.Prawy.Waga = -1;
                            korzen.Lewy.Waga = 0;
                        }
                    }
                    else
                    {
                        if (c == 0)
                        {
                            if (wezel.Prawy != null) wezel.Prawy.Waga = 0;
                            if (wezel.Lewy != null) wezel.Lewy.Waga = 0;
                        }
                        else if (c == -1)
                        {
                            if (wezel.Prawy != null) wezel.Prawy.Waga = 0;
                            if (wezel.Lewy != null) wezel.Lewy.Waga = 1;
                        }
                        else if (c == 1)
                        {
                            if (wezel.Prawy != null) wezel.Prawy.Waga = -1;
                            if (wezel.Lewy != null) wezel.Lewy.Waga = 0;
                        }
                    }
                }
            }

        }
    }

    public class UsunElement
    {
        public static void Usun(ref Wezel korzen, double do_usuniecia)
        {
            bool CzyWstawiony = SzukajElement.Szukaj(ref korzen, do_usuniecia);

            if (CzyWstawiony && korzen.Liczba == do_usuniecia)
            {
                if ((korzen.Lewy == null || korzen.Lewy.Liczba == null) && (korzen.Prawy == null || korzen.Prawy.Liczba == null))
                {
                    //nie ma dzieci
                    korzen.Delete();
                }
                else if ((korzen.Prawy == null || korzen.Prawy.Liczba == null) && korzen.Lewy != null)
                {
                    //ma lewe dziecko
                    korzen.Liczba = korzen.Lewy.Liczba;
                    korzen.Waga = korzen.Lewy.Waga;

                    Wezel lewy = korzen.Lewy.Lewy;
                    Wezel prawy = korzen.Lewy.Prawy;

                    korzen.Lewy = lewy;
                    korzen.Prawy = prawy;
                }
                else if (korzen.Prawy != null && (korzen.Lewy == null || korzen.Lewy.Liczba == null))
                {
                    //ma prawe dziecko
                    korzen.Liczba = korzen.Prawy.Liczba;
                    korzen.Waga = korzen.Prawy.Waga;

                    Wezel lewy = korzen.Prawy.Lewy;
                    Wezel prawy = korzen.Prawy.Prawy;

                    korzen.Lewy = lewy;
                    korzen.Prawy = prawy;
                }
                else if (korzen.Lewy != null && korzen.Prawy != null)
                {
                    //ma dwoje dzieci
                    double korzen_liczba = Convert.ToDouble(korzen.Liczba);
                    Wezel x = korzen.Prawy;
                    Wezel nastepnik = ZnajdzMinMax.Min(ref x, ref korzen); //Ta funkcja aktualizuje również wagi

                    if (korzen.Liczba != korzen_liczba)
                    {
                        korzen.Prawy = korzen.Prawy.Prawy;
                    }
                    else
                    {
                        if (nastepnik.Prawy != null)
                        {
                            korzen.Liczba = nastepnik.Liczba;
                            nastepnik.Liczba = nastepnik.Prawy.Liczba;
                            nastepnik.Prawy = nastepnik.Prawy.Prawy;

                            nastepnik.Waga = 0;
                        }
                        else
                        {
                            korzen.Liczba = nastepnik.Liczba;
                            nastepnik.Delete();
                        }
                    }
                }
                return;
            }

            if (CzyWstawiony)
            {
                bool aktualizuj = false;
                Rekurencja(ref korzen, do_usuniecia, ref aktualizuj, ref korzen);
            }
        }

        private static void Rekurencja(ref Wezel wezel, double do_usuniecia, ref bool aktualizuj, ref Wezel korzen)
        {
            if (wezel == null || wezel.Liczba == null)
            {
                return;
            }

            //czy istnieją poniższe węzły, true - istnieje, false - nie istnieje
            bool[] Wezly = new bool[6]; //L, P, LL, LP, PL, PP

            for (int i = 0; i < 6; i++)
            {
                Wezly[i] = false;
            }

            try
            {
                if (wezel.Lewy != null)
                {
                    Wezly[0] = (wezel.Lewy.Liczba != null);
                }
            }
            catch (System.NullReferenceException)
            {
                Wezly[0] = false;
            }

            try
            {
                if (wezel.Prawy != null)
                {
                    Wezly[1] = (wezel.Prawy.Liczba != null);
                }
            }
            catch (System.NullReferenceException)
            {
                Wezly[1] = false;
            }

            if (Wezly[0])
            {
                if (wezel.Lewy.Liczba == do_usuniecia)
                {
                    try
                    {
                        if (wezel.Lewy.Lewy != null)
                        {
                            Wezly[2] = (wezel.Lewy.Lewy.Liczba != null);
                        }
                    }
                    catch (System.NullReferenceException)
                    {
                        Wezly[2] = false;
                    }

                    try
                    {
                        if (wezel.Lewy.Prawy != null)
                        {
                            Wezly[3] = (wezel.Lewy.Prawy.Liczba != null);
                        }
                    }
                    catch (System.NullReferenceException)
                    {
                        Wezly[3] = false;
                    }

                }
            }

            if (Wezly[1])
            {
                if (wezel.Prawy.Liczba == do_usuniecia)
                {
                    try
                    {
                        if (wezel.Prawy.Lewy != null)
                        {
                            Wezly[4] = (wezel.Prawy.Lewy.Liczba != null);
                        }
                    }
                    catch (System.NullReferenceException)
                    {
                        Wezly[4] = false;
                    }

                    try
                    {
                        if (wezel.Prawy.Prawy != null)
                        {
                            Wezly[5] = (wezel.Prawy.Prawy.Liczba != null);
                        }
                    }
                    catch (System.NullReferenceException)
                    {
                        Wezly[5] = false;
                    }
                }
            }

            if (Wezly[0] == true && wezel.Lewy.Liczba == do_usuniecia) //Lewy Wezel
            {
                if (Wezly[2] == false && Wezly[3] == false)
                {
                    //nie ma dzieci
                    wezel.Lewy.Delete();
                    aktualizuj = true;
                }
                else if (Wezly[3] == false && Wezly[2] == true)
                {
                    //ma lewe dziecko
                    wezel.Lewy = wezel.Lewy.Lewy;
                    aktualizuj = true;
                }
                else if (Wezly[3] == true && Wezly[2] == false)
                {
                    //ma prawe dziecko
                    wezel.Lewy = wezel.Lewy.Prawy;
                    aktualizuj = true;
                }
                else if (Wezly[2] == true && Wezly[3] == true)
                {
                    //ma dwoje dzieci
                    double wezel_liczba = Convert.ToDouble(wezel.Liczba);
                    Wezel x = wezel.Lewy.Prawy;
                    Wezel nastepnik = ZnajdzMinMax.Min(ref x, ref korzen); //Ta funkcja aktualizuje również wagi

                    if (wezel.Liczba != wezel_liczba)
                    {
                        wezel.Prawy = wezel.Prawy.Prawy;
                    }
                    else
                    {
                        if (nastepnik.Prawy != null)
                        {
                            wezel.Lewy.Liczba = nastepnik.Liczba;
                            nastepnik.Liczba = nastepnik.Prawy.Liczba;
                            nastepnik.Prawy = nastepnik.Prawy.Prawy;
                            nastepnik.Waga = 0;
                        }
                        else
                        {
                            wezel.Lewy.Liczba = nastepnik.Liczba;
                            nastepnik.Delete();
                        }
                    }
                }
            }
            else if (Wezly[1] == true && wezel.Prawy.Liczba == do_usuniecia) //Prawy Wezel
            {
                if (Wezly[4] == false && Wezly[5] == false)
                {
                    //nie ma dzieci
                    wezel.Prawy.Delete();
                    aktualizuj = true;
                }
                else if (Wezly[5] == false && Wezly[4] == true)
                {
                    //ma lewe dziecko
                    wezel.Prawy = wezel.Prawy.Lewy;
                    aktualizuj = true;
                }
                else if (Wezly[5] == true && Wezly[4] == false)
                {
                    //ma prawe dziecko
                    wezel.Prawy = wezel.Prawy.Prawy;
                    aktualizuj = true;
                }
                else if (Wezly[4] == true && Wezly[5] == true)
                {
                    //ma dwoje dzieci
                    double wezel_liczba = Convert.ToDouble(wezel.Liczba);
                    Wezel y = wezel.Prawy.Prawy;
                    Wezel nastepnik = ZnajdzMinMax.Min(ref y, ref korzen); //Ta funkcja aktualizuje również wagi

                    if (wezel.Liczba != wezel_liczba)
                    {
                        wezel.Prawy = wezel.Prawy.Prawy;
                    }
                    else
                    {
                        if (nastepnik.Prawy != null)
                        {
                            wezel.Prawy.Liczba = nastepnik.Liczba;
                            nastepnik.Liczba = nastepnik.Prawy.Liczba;
                            nastepnik.Prawy = nastepnik.Prawy.Prawy;
                            nastepnik.Waga = 0;
                        }
                        else
                        {
                            wezel.Prawy.Liczba = nastepnik.Liczba;
                            nastepnik.Delete();
                        }
                    }
                }
            }
            else
            {
                if (wezel.Liczba > do_usuniecia)
                {
                    Wezel x = wezel.Lewy;
                    Rekurencja(ref x, do_usuniecia, ref aktualizuj, ref korzen);
                }
                else
                {
                    Wezel y = wezel.Prawy;
                    Rekurencja(ref y, do_usuniecia, ref aktualizuj, ref korzen);
                }
            }

            if (wezel.Liczba > do_usuniecia)
            {
                if (aktualizuj) wezel.Waga--;
                if (wezel.Waga == 1 || wezel.Waga == -1) aktualizuj = false;

                if (wezel.Waga == 2 || wezel.Waga == -2) Rotacje.RotujUsuwanie(ref wezel, ref korzen);
            }
            else
            {
                if (aktualizuj) wezel.Waga++;
                if (wezel.Waga == 1 || wezel.Waga == -1) aktualizuj = false;

                if (wezel.Waga == 2 || wezel.Waga == -2) Rotacje.RotujUsuwanie(ref wezel, ref korzen);
            }
        }
    }

    public class ZnajdzMinMax
    {
        private static Wezel min;
        private static Wezel max;

        public static Wezel Min(ref Wezel korzen, ref Wezel korzen_glowny)
        {
            min = korzen;
            bool aktualizuj = false;
            RekurencjaMin(ref korzen);
            RekurencjaWagi(ref korzen_glowny, ref aktualizuj, ref korzen_glowny);
            return min;
        }

        private static void RekurencjaMin(ref Wezel wezel)
        {
            if (wezel == null)
            {
                return;
            }

            if (wezel.Lewy != null)
            {
                if (wezel.Lewy.Liczba != null) min = wezel.Lewy;
                Wezel x = wezel.Lewy;
                RekurencjaMin(ref x);
            }
        }

        private static void RekurencjaWagi(ref Wezel wezel, ref bool aktualizuj, ref Wezel korzen)
        {
            if (wezel == null || wezel.Liczba == null)
            {
                return;
            }

            if (wezel.Liczba == min.Liczba)
            {
                aktualizuj = true;
                return;
            }

            if (wezel.Liczba > min.Liczba)
            {
                Wezel x = wezel.Lewy;
                RekurencjaWagi(ref x, ref aktualizuj, ref korzen);

                if (aktualizuj) wezel.Waga--;
                if (wezel.Waga == 1 || wezel.Waga == -1) aktualizuj = false;

                if (wezel.Waga == 2 || wezel.Waga == -2) Rotacje.RotujUsuwanie(ref wezel, ref korzen);
            }
            else
            {
                Wezel y = wezel.Prawy;
                RekurencjaWagi(ref y, ref aktualizuj, ref korzen);

                if (aktualizuj) wezel.Waga++;
                if (wezel.Waga == 1 || wezel.Waga == -1) aktualizuj = false;

                if (wezel.Waga == 2 || wezel.Waga == -2) Rotacje.RotujUsuwanie(ref wezel, ref korzen);
            }
        }

        public static Wezel Max(Wezel korzen)
        {
            max = korzen;
            RekurencjaMax(korzen);
            return max;
        }

        private static void RekurencjaMax(Wezel wezel)
        {
            if (wezel == null)
            {
                return;
            }

            if (wezel.Prawy != null)
            {
                max = wezel.Prawy;
                RekurencjaMax(wezel.Prawy);
            }
        }
    }

    public class SzukajElement
    {
        public static bool Szukaj(ref Wezel korzen, double szukana)
        {
            bool wynik = Rekurencja(korzen, szukana);
            return wynik;
        }

        public static Wezel SzukajOjca(ref Wezel korzen, double szukana)
        {
            //Console.WriteLine(Szukaj(ref korzen, szukana));
            Wezel wynik = RekurencjaO(korzen, szukana);
            return wynik;
        }

        private static Wezel RekurencjaO(Wezel wezel, double szukana)
        {
            if (wezel == null)
            {
                return null;
            }

            if (wezel.Lewy != null)
            {
                if (wezel.Lewy.Liczba == szukana)
                {
                    return wezel;
                }
            }
            if (wezel.Prawy != null)
            {
                if (wezel.Prawy.Liczba == szukana)
                {
                    return wezel;
                }
            }

            Wezel x = null;
            Wezel y = null;

            if (wezel.Liczba > szukana)
            {
                x = RekurencjaO(wezel.Lewy, szukana);
            }
            else
            {
                y = RekurencjaO(wezel.Prawy, szukana);
            }

            if (x != null)
            {
                return x;
            }
            else if (y != null)
            {
                return y;
            }
            else
            {
                return null;
            }
        }

        private static bool Rekurencja(Wezel wezel, double szukana)
        {
            if (wezel == null)
            {
                return false;
            }

            if (wezel.Liczba == szukana)
            {
                return true;
            }

            bool x = false;
            bool y = false;

            if (wezel.Liczba > szukana)
            {
                x = Rekurencja(wezel.Lewy, szukana);
            }
            else
            {
                y = Rekurencja(wezel.Prawy, szukana);
            }

            if (x == true || y == true)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }

    public class SzukajCzescCalkowita
    {
        public static int Licz(ref Wezel korzen, int szukana_calkowita)
        {
            int wynik = 0;
            Rekurencja(korzen, szukana_calkowita, ref wynik);
            return wynik;
        }

        private static void Rekurencja(Wezel wezel, int szukana_calkowita, ref int wynik)
        {
            if (wezel == null)
            {
                return;
            }

            double x = System.Convert.ToDouble(wezel.Liczba);
            int czesc_calkowita = System.Convert.ToInt32(Math.Truncate(x));

            if (czesc_calkowita > szukana_calkowita)
            {
                Rekurencja(wezel.Lewy, szukana_calkowita, ref wynik);
            }
            else if (czesc_calkowita < szukana_calkowita)
            {
                Rekurencja(wezel.Prawy, szukana_calkowita, ref wynik);
            }
            else
            {
                Rekurencja(wezel.Lewy, szukana_calkowita, ref wynik);
                Rekurencja(wezel.Prawy, szukana_calkowita, ref wynik);
            }

            if (czesc_calkowita == szukana_calkowita)
            {
                wynik++;
                return;
            }
        }
    }

    public class WypiszDrzewo
    {
        public static void Wypisz(ref Wezel korzen)
        {
            //Rekurencja(korzen, 0);
            Rekurencja_wagi(korzen, 0);
        }

        private static void Rekurencja(Wezel wezel, int glebokosc)
        {
            if (wezel == null)
            {
                return;
            }

            if (wezel.Liczba != null)
            {
                Console.WriteLine(string.Concat(new string(' ', glebokosc * 2), wezel.Liczba));
            }

            Rekurencja(wezel.Lewy, glebokosc + 1);
            Rekurencja(wezel.Prawy, glebokosc + 1);
        }

        private static void Rekurencja_wagi(Wezel wezel, int glebokosc)
        {
            if (wezel == null)
            {
                return;
            }

            if (wezel.Liczba != null)
            {
                Console.WriteLine(string.Concat(new string(' ', glebokosc * 2), wezel.Liczba + "(" + wezel.Waga + ")"));
            }

            Rekurencja_wagi(wezel.Lewy, glebokosc + 1);
            Rekurencja_wagi(wezel.Prawy, glebokosc + 1);
        }
    }
}