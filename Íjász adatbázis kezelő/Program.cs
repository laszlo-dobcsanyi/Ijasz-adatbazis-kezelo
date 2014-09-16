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

                    break;
                } while (true);

                FixDatabase();
            } while (true);
        }

        private static void FixDatabase()
        {
            int version = 0;
            SQLiteCommand command;

            command = connection.CreateCommand();
            command.CommandText = "SELECT PRVERZ FROM Verzió;";
            try
            {
                SQLiteDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    version = reader.GetInt32(0);
                }
            }
            catch (SQLiteException) { version = 1; }
            catch (Exception exception) { Console.WriteLine("> Hiba az adatbázis verziójának ellenőrzésekor! (#4)!\n" + exception.Message); return; }

            Console.WriteLine("> Az adatbázis verziója: " + version);

            switch (version)
            {
                case 1:
                    command = connection.CreateCommand();
                    command.CommandText = "CREATE TABLE Verzió (PRVERZ int); INSERT INTO Verzió (PRVERZ) VALUES (2); " +
                        "ALTER TABLE Verseny ADD VEALSZ int; UPDATE Verseny SET VEALSZ = VEOSPO;";
                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch (SQLiteException exception) { Console.WriteLine("> Hiba az adatbázis módosításakor! (#5)!\n" + exception.Message); break; }

                    Console.WriteLine("> Sikeresen módosítottam az adatbázis verzióját 2-re!");
                    break;
            }

            try
            {
                command.Dispose();
                connection.Close();
            }
            catch (SQLiteException exception) { Console.WriteLine("> Hiba az adatbázis bezárásakor! (#6)!\n" + exception.Message); return; }
        }
    }
}
