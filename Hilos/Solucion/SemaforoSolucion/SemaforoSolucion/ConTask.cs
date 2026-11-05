using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

class ConTask
{
    static SemaphoreSlim ascensor = new SemaphoreSlim(3);

    static async Task Main()
    {
        List<Task> personas = new List<Task>();

        for (int i = 1; i <= 6; i++)
        {
            personas.Add(Subir(i));               // arranca la tarea y la guardo
        }

        await Task.WhenAll(personas);             // espero a las 6 sin bloquear
        Console.WriteLine("Todos subieron y bajaron");
    }

    static async Task Subir(int id)
    {
        Console.WriteLine("Persona " + id + " espera");

        await ascensor.WaitAsync();               // antes: ascensor.Wait()
        try
        {
            Console.WriteLine("Persona " + id + " sube");
            await Task.Delay(3000);               // antes: Thread.Sleep(3000)
            Console.WriteLine("Persona " + id + " baja");
        }
        finally
        {
            ascensor.Release();                   // igual que antes
        }
    }
}