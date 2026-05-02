using System;
using Escuela.Models;

class Program
{
    static void Main()
    {
        try
        {
            Console.WriteLine("Creando base de datos...");

            using (var db = new EscuelaContext())
            {
                db.Database.Initialize(true);
            }

            Console.WriteLine("Base de datos creada correctamente.");
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error: " + ex.Message);
        }

        Console.WriteLine("Presiona una tecla para salir...");
        Console.ReadKey();
    }
}