using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

namespace DesafioCocina
{
    class Program
    {
        private static readonly Stopwatch reloj = Stopwatch.StartNew();
        private static readonly object candadoConsola = new object();

        public static void Log(string mensaje)
        {
            lock (candadoConsola)
                Console.WriteLine($"{reloj.Elapsed.TotalSeconds,5:0.00}s  {mensaje}");
        }

        static void Main()
        {
            const int PEDIDOS_POR_MOZO = 5;
            Bandeja bandeja = new Bandeja(4);

            Mozo[] mozos =
            {
                new Mozo("Mozo Ana  ", bandeja, PEDIDOS_POR_MOZO, 1),
                new Mozo("Mozo Beto ", bandeja, PEDIDOS_POR_MOZO, 2),
                new Mozo("Mozo Carla", bandeja, PEDIDOS_POR_MOZO, 3)
            };
            Cocinero[] cocineros =
            {
                new Cocinero("Cocinero Dani", bandeja, 4),
                new Cocinero("Cocinero Eli ", bandeja, 5)
            };

            List<Thread> hilosMozos = new List<Thread>();
            List<Thread> hilosCocineros = new List<Thread>();
            foreach (Mozo m in mozos) hilosMozos.Add(new Thread(m.Ejecutar));
            foreach (Cocinero k in cocineros) hilosCocineros.Add(new Thread(k.Ejecutar));

            hilosMozos.ForEach(h => h.Start());
            hilosCocineros.ForEach(h => h.Start());

            hilosMozos.ForEach(h => h.Join());      //esperamos a los mozos...
            bandeja.Cerrar();                       //...y cerramos la bandeja
            hilosCocineros.ForEach(h => h.Join());

            int total = 0;
            foreach (Cocinero k in cocineros) total += k.Platos;
            int esperado = mozos.Length * PEDIDOS_POR_MOZO;
            Console.WriteLine();
            Console.WriteLine($"Pedidos: {esperado} | Platos cocinados: {total} | " +
                              (total == esperado ? "OK" : "ERROR: se perdieron pedidos"));
        }
    }
}
