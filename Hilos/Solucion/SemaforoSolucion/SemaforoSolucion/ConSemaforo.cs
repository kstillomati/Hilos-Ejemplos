using System;
using System.Collections.Generic;
using System.Threading;

class ConSemaforo
{
    static SemaphoreSlim ascensor = new SemaphoreSlim(3);
    static void Main()
    {
        List<Thread> hilos = new List<Thread>();

        for (int i = 1; i <= 6; i++)
        {
            int id = i;                             // Copia local (si no, todos ven la misma i)
            Thread t = new Thread(() => Subir(id));
            hilos.Add(t);
            t.Start();
        }

        foreach (Thread t in hilos)
            t.Join();                               // Espero a los 6

        Console.WriteLine("Todos subieron y bajaron");
    }

    static void Subir(int id)
    {
        Console.WriteLine("Persona " + id + "espera");

        ascensor.Wait();                            // Pide un lugar, si hay 3 arriba, se bloquea acá
        try
        {
            Console.WriteLine("Persona " + id + "Sube");
            Thread.Sleep(3000);
            
            Console.WriteLine("Persona " + id + "Baja");
        }

        finally
        {
            ascensor.Release();                     // Devuelve el lugar siempre, aunque haya una excepciónm
        }
    }
}