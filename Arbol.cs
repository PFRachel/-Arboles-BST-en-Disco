using System;
using System.IO;
namespace tarea3
{
    public class ArbolBinarioBusqueda
    {
        private string archivo = "arbol_almacenado.txt";  // Archivo donde se guardará el árbol
        private long raizPosicion = -1;  // Posición del nodo raíz en el archivo

        public ArbolBinarioBusqueda()
        {
            if (File.Exists(archivo))
            {
                using (var lector = new StreamReader(archivo))
                {

                   var  linea = lector.ReadLine();
                   if(long.TryParse(linea,out long posicion))
                   {
                    raizPosicion = posicion;
                    Console.WriteLine("Archivo cargado correctamente");
                   }
                   else
                   {
                    Console.WriteLine("Archivo vacio o no tiene posicion valida");
                   }
                }   
            }
            else 
            {
                Console.WriteLine("No se encontro archivo, se creara uno nuevo");
            }
        }

        // Guardar el árbol en disco
        private void Guardar()
        {
            using (var escritor = new StreamWriter(archivo, false))
            {
                escritor.WriteLine(raizPosicion);
            }
            Console.WriteLine("Datos guardados en el archivo ");
        }

        // Inserción de un valor en el árbol
        public void Insertar(int valor)
        {
            if (raizPosicion == -1) // Si el árbol está vacío
            {
                raizPosicion = EscribirNodoEnDisco(new Nodo(valor));
                Console.WriteLine($"Insertando el nodo raiz; {valor}");
            }
            else
            {
                InsertarRecursivo(raizPosicion, valor);
                Console.WriteLine($"Insertando el nodo;{valor}");
            }

            Guardar();
        }

        private void InsertarRecursivo(long posicionActual, int valor)
        {
            var nodoActual = LeerNodoDeDisco(posicionActual);

            if (valor < nodoActual.valor)
            {
                if (nodoActual.posicionIzquierda == -1)
                {
                    nodoActual.posicionIzquierda = EscribirNodoEnDisco(new Nodo(valor));
                    EscribirNodoEnDisco(nodoActual, posicionActual);
                }
                else
                {
                    InsertarRecursivo(nodoActual.posicionIzquierda, valor);
                }
            }
            else if (valor > nodoActual.valor)
            {
                if (nodoActual.posicionDerecha == -1)
                {
                    nodoActual.posicionDerecha = EscribirNodoEnDisco(new Nodo(valor));
                    EscribirNodoEnDisco(nodoActual, posicionActual);
                }
                else
                {
                    InsertarRecursivo(nodoActual.posicionDerecha, valor);
                }
            }
        }

        // Método para escribir un nodo en el archivo y devolver su posición
        private long EscribirNodoEnDisco(Nodo nodo, long posicion = -1)
        {
            using (var fs = new FileStream(archivo, FileMode.OpenOrCreate, FileAccess.Write))
            {
                if (posicion == -1)  // Si no se proporciona una posición, escribir al final
                {
                    fs.Seek(0, SeekOrigin.End);
                    posicion = fs.Position;
                }
                else
                {
                    fs.Seek(posicion, SeekOrigin.Begin);
                }

                using (var escritor = new StreamWriter(fs))
                {
                    escritor.WriteLine(nodo.ToString());
                }
            }
            return posicion;
        }

        // Método para leer un nodo del archivo desde una posición específica
        private Nodo LeerNodoDeDisco(long posicion)
        {
            using (var fs = new FileStream(archivo, FileMode.Open, FileAccess.Read))
            {
                fs.Seek(posicion, SeekOrigin.Begin);
                using (var lector = new StreamReader(fs))
                {
                    var datos = lector.ReadLine();
                    return Nodo.DesdeString(datos);
                }
            }
        }

        // Búsqueda de un valor en el árbol
        public bool Buscar(int valor)
        {
            return BuscarRecursivo(raizPosicion, valor);
        }

        private bool BuscarRecursivo(long posicionActual, int valor)
        {
            if (posicionActual == -1)
            {
                Console.WriteLine($"El valor{valor}no se encontro");
                 return false;
            }

            var nodoActual = LeerNodoDeDisco(posicionActual);

            if (nodoActual.valor == valor)
            {
                return true;
            }
            else if (valor < nodoActual.valor)
            {
                return BuscarRecursivo(nodoActual.posicionIzquierda, valor);
            }
            else
            {
                return BuscarRecursivo(nodoActual.posicionDerecha, valor);
            }
        }

        // Eliminación de un valor en el árbol
        public void Eliminar(int valor)
        {
            raizPosicion = EliminarRecursivo(raizPosicion, valor);
            Guardar();
        }

        private long EliminarRecursivo(long posicionActual, int valor)
        {
            if (posicionActual == -1) return -1;

            var nodoActual = LeerNodoDeDisco(posicionActual);

            if (valor < nodoActual.valor)
            {
                nodoActual.posicionIzquierda = EliminarRecursivo(nodoActual.posicionIzquierda, valor);
                EscribirNodoEnDisco(nodoActual, posicionActual);
            }
            else if (valor > nodoActual.valor)
            {
                nodoActual.posicionDerecha = EliminarRecursivo(nodoActual.posicionDerecha, valor);
                EscribirNodoEnDisco(nodoActual, posicionActual);
            }
            else
            {
                // Caso donde el nodo tiene 0 o 1 hijo
                if (nodoActual.posicionIzquierda == -1)
                    return nodoActual.posicionDerecha;
                else if (nodoActual.posicionDerecha == -1)
                    return nodoActual.posicionIzquierda;

                // Caso donde el nodo tiene dos hijos
                var nodoMinDerecha = MinimoValorNodo(nodoActual.posicionDerecha);
                nodoActual.valor = nodoMinDerecha.valor;
                nodoActual.posicionDerecha = EliminarRecursivo(nodoActual.posicionDerecha, nodoMinDerecha.valor);
                EscribirNodoEnDisco(nodoActual, posicionActual);
            }

            return posicionActual;
        }

        private Nodo MinimoValorNodo(long posicion)
        {
            var nodoActual = LeerNodoDeDisco(posicion);
            while (nodoActual.posicionIzquierda != -1)
            {
                nodoActual = LeerNodoDeDisco(nodoActual.posicionIzquierda);
            }
            return nodoActual;
        }
    }

}