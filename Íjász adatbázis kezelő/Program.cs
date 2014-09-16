using System;
using System.IO;
using System.Data.SQLite;

namespace Íjász_adatbázis_kezelő
{
    public sealed class Program
    {
        private static SQLiteConnection connection;

        public static void Main(string[] args)
        {
            do
            {
                bool accepted = false;
                do
                {
                    Console.Write("< Adatbázis neve: ");
                    string name = Console.ReadLine();

                    try
                    {
                        if (!File.Exists(name)) { Console.WriteLine("> Nem található a megadott adatbázis (#1)!"); continue; }
                    }
                    catch (Exception exception) { Console.WriteLine("> Hiba a file ellenőrzése során (#2)!\n" + exception.Message); continue; }

                    connection = new SQLiteConnection("Data Source=" + name + "; Version=3; New=False; Compress=True;");
                    try
                    {
                        connection.Open();
                    }
                    catch (Exception exception) { Console.WriteLine("> Hiba az adatbázis megnyitása során (#3)!\n" + exception.Message); continue; }

                } while (!accepted);

                FixDatabase();
            } while (true);
        }

        private static void FixDatabase()
        {

        }
    }
}
