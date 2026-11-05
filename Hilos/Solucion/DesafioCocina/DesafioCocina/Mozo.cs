using System;
using System.Threading;

namespace DesafioCocina
{
    class Mozo
    {
        private static readonly string[] Platos =
            { "Milanesa", "Ravioles", "Bife", "Ensalada", "Empanadas", "Pizza", "Flan" };
        private static int ultimoNumero = 0;

        private readonly string nombre;
        private readonly Bandeja bandeja;
        private readonly int cantidad;
        private readonly Random rnd;

        public Mozo(string nombre, Bandeja bandeja, int cantidad, int semilla)
        {
            this.nombre = nombre;
            this.bandeja = bandeja;
            this.cantidad = cantidad;
            rnd = new Random(semilla);   //una semilla distinta por mozo
        }

        public void Ejecutar()
        {
            for (int i = 0; i < cantidad; i++)
            {
                Thread.Sleep(rnd.Next(200, 600));   //tomar el pedido en la mesa
                Comanda c = new Comanda
                {
                    Numero = Interlocked.Increment(ref ultimoNumero),
                    Mesa = rnd.Next(1, 16),
                    Plato = Platos[rnd.Next(Platos.Length)],
                    Mozo = nombre
                };
                bandeja.Dejar(c);
            }
            Program.Log($"{nombre} terminó su turno");
        }
    }
}
