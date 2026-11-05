namespace DesafioCocina
{
    class Comanda
    {
        public int Numero;
        public int Mesa;
        public string Plato;
        public string Mozo;

        public override string ToString()
        {
            return $"#{Numero} Mesa {Mesa}: {Plato}";
        }
    }
}
