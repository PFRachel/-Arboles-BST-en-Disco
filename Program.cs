namespace tarea3
{
   class Program
{
    static void Main(string[] args)
    {
        var arbol = new ArbolBinarioBusqueda();
        
        arbol.Insertar(50);
        arbol.Insertar(30);
        arbol.Insertar(70);
        arbol.Insertar(20);
        arbol.Insertar(40);
        arbol.Insertar(60);
        arbol.Insertar(80);

        Console.WriteLine("Buscar 40: " + (arbol.Buscar(40) ? "Encontrado" : "No encontrado"));
        Console.WriteLine("Eliminar 40");
        arbol.Eliminar(40);
        Console.WriteLine("Buscar 40: " + (arbol.Buscar(40) ? "Encontrado" : "No encontrado"));
    }
}

}
