using System;
using System.Threading;

namespace DesafioCocina
{
    class Cocinero
    {
        private readonly string nombre;
        private readonly Bandeja bandeja;
        private readonly Random rnd;
        public int Platos { get; private set; }

        public Cocinero(string nombre, Bandeja bandeja, int semilla)
        {
            this.nombre = nombre;
            this.bandeja = bandeja;
            rnd = new Random(semilla);
        }

        public void Ejecutar()
        {
            Comanda c;
            while (bandeja.Tomar(nombre, out c))
            {
                Thread.Sleep(rnd.Next(500, 1200));
                Platos++;
                Program.Log($"{nombre} terminó {c} (de {c.Mozo})");
            }
            Program.Log($"{nombre} se va: cocinó {Platos} platos");
        }
    }
}
