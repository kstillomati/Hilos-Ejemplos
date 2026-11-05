using System;
using System.Collections.Generic;
using System.Threading;

class SinSemaforo
{
    static void Main()
    {
        List<Thread> hilos  = new List<Thread>();

        for(int i = 1; i <= 6; i++)
        {
            int id = i;                             // Copia local (si no, todos ven la misma i)
            Thread t = new Thread(() => Subir(id));
            hilos.Add(t);
            t.Start();
        }

        foreach(Thread t in hilos)
            t.Join();                               // erspero a los 6

        Console.WriteLine( "Todos subieron y bajaron");
    }

    static void Subir(int id)
    {
        Console.WriteLine("Persona " + id + "espera");
        Console.WriteLine("Persona " + id + "Sube");
        Thread.Sleep(1000);
        Console.WriteLine("Persona " + id + "baja");
    }
}