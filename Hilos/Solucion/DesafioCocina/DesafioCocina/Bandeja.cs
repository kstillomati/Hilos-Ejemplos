using System.Collections.Generic;
using System.Threading;

namespace DesafioCocina
{
    class Bandeja
    {
        private readonly Queue<Comanda> comandas = new Queue<Comanda>();
        private readonly object candado = new object();
        private readonly int capacidad;
        private bool cerrada = false;

        public Bandeja(int capacidad) { this.capacidad = capacidad; }

        //Productor: la llama un MOZO.
        public void Dejar(Comanda c)
        {
            bool tomado = false;
            try
            {
                Monitor.Enter(candado, ref tomado);

                //WHILE y no IF: al despertar hay que volver a verificar,
                //otro mozo pudo haber ocupado el lugar antes que yo.
                while (comandas.Count == capacidad)
                {
                    Program.Log($"{c.Mozo} espera: bandeja llena " + Ocupacion());
                    Monitor.Wait(candado);          //libera el lock y duerme
                }

                comandas.Enqueue(c); //Dejar la comanda en la bandeja...
                Program.Log($"{c.Mozo} deja {c}  " + Ocupacion());

                //PulseAll y no Pulse: en la cola de espera hay mozos Y
                //cocineros; con Pulse podría despertar al tipo equivocado.
                Monitor.PulseAll(candado);
            }
            finally
            {
                if (tomado) Monitor.Exit(candado);
            }
        }

        //Consumidor: la llama un COCINERO.
        //Devuelve false cuando está cerrada y vacía (no hay más trabajo).
        public bool Tomar(string cocinero, out Comanda c)
        {
            bool tomado = false;
            try
            {
                Monitor.Enter(candado, ref tomado);

                while (comandas.Count == 0 && !cerrada)
                    Monitor.Wait(candado);//Espero a que un cocinero haga PulseAll

                if (comandas.Count == 0)            //vacía y cerrada
                {
                    c = null;
                    return false;
                }

                c = comandas.Dequeue(); //Tomo el pedido de comanda...
                Program.Log($"{cocinero} toma {c}  " + Ocupacion());
                Monitor.PulseAll(candado);          //ahora hay lugar
                return true;
            }
            finally
            {
                if (tomado) Monitor.Exit(candado);
            }
        }

        //La llama Main cuando todos los mozos terminaron.
        public void Cerrar()
        {
            bool tomado = false;
            try
            {
                Monitor.Enter(candado, ref tomado);
                cerrada = true;
                Monitor.PulseAll(candado);          //despierta a cocineros dormidos
            }
            finally
            {
                if (tomado) Monitor.Exit(candado);
            }
        }

        //Se llama siempre con el lock tomado.
        private string Ocupacion()
        {
            return "[" + new string('#', comandas.Count)
                       + new string('.', capacidad - comandas.Count) + "]";
        }
    }
}
