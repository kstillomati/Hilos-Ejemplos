using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    internal class Program
    {
        public const int FIN = -1;

        static void Main(string[] args)
        {
            BufferAcotado buffer = new BufferAcotado(50);

            Thread p = new Thread(new Productor(buffer, 100).Ejecutar);
            Thread c1 = new Thread(new Consumidor(buffer, "Consumidor 1").Ejecutar);
            Thread c2 = new Thread(new Consumidor(buffer, "Consumidor 2").Ejecutar);

            p.Start(); c1.Start(); c2.Start();

            p.Join();                           // el productor terminó...
            buffer.Poner(FIN);                  // ...aviso a cada consumidor
            buffer.Poner(FIN);

            c1.Join(); c2.Join();
            Console.WriteLine("Fin del programa");
        }
    }
}
