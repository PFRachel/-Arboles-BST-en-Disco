using System;
using System.IO;
namespace tarea3
{
   using System;
using System.IO;

public class ArbolBinarioBusqueda
{
        private string archivo = "arbol_almacenado.bin"; // Archivo binario
        private long raizPosicion = -1; // Posición del nodo raíz en el archivo

        public ArbolBinarioBusqueda()
        {
            if (File.Exists(archivo))
            {
                using (var fs = new FileStream(archivo, FileMode.Open, FileAccess.Read))
                using (var reader = new BinaryReader(fs))
                {
                    raizPosicion = reader.ReadInt64(); // Leer la posición del nodo raíz
                }
                Console.WriteLine("Archivo cargado correctamente.");
            }
            else
            {
                Console.WriteLine("No se encontró archivo. Se creará uno nuevo.");
            }
        }

        // Método para guardar la posición de la raíz al inicio del archivo
        private void Guardar()
        {
            using (var fs = new FileStream(archivo, FileMode.OpenOrCreate, FileAccess.Write))
            using (var writer = new BinaryWriter(fs))
            {
                writer.Seek(0, SeekOrigin.Begin);
                writer.Write(raizPosicion); // Guardar la posición de la raíz
            }
            Console.WriteLine("Datos guardados en el archivo.");
        }

        // Método para insertar un nuevo valor en el árbol
        public void Insertar(int valor)
        {
            raizPosicion = InsertarRecursivo(raizPosicion, valor);
            Guardar(); // Guardar cambios en el archivo
        }

        private long InsertarRecursivo(long posicionActual, int valor)
        {
            if (posicionActual == -1)
            {
                // Crear un nuevo nodo si no hay posición
                Nodo nuevoNodo = new Nodo(valor);
                return EscribirNodoEnDisco(nuevoNodo);
            }

            // Leer el nodo actual
            Nodo nodoActual = LeerNodoDeDisco(posicionActual);
            
            // Determinar dónde insertar el nuevo valor
            if (valor < nodoActual.valor)
            {
                // Insertar en el subárbol izquierdo
                if (nodoActual.posicionIzquierda == -1)
                {
                    nodoActual.posicionIzquierda = EscribirNodoEnDisco(nodoActual, posicionActual); // Actualizar posición en disco
                }
                else
                {
                    nodoActual.posicionIzquierda = InsertarRecursivo(nodoActual.posicionIzquierda, valor);
                }
            }
            else if (valor > nodoActual.valor)
            {
                // Insertar en el subárbol derecho
                if (nodoActual.posicionDerecha == -1)
                {
                    nodoActual.posicionDerecha = EscribirNodoEnDisco(nodoActual, posicionActual); // Actualizar posición en disco
                }
                else
                {
                    nodoActual.posicionDerecha = InsertarRecursivo(nodoActual.posicionDerecha, valor);
                }
            }
            // Guardar el nodo actualizado en disco
            EscribirNodoEnDisco(nodoActual, posicionActual);
            return posicionActual; // Devolver la posición actual
        }

        // Método para buscar un valor en el árbol
        public bool Buscar(int valor)
        {
            return BuscarRecursivo(raizPosicion, valor);
        }

        private bool BuscarRecursivo(long posicionActual, int valor)
        {
            if (posicionActual == -1) return false; // No encontrado

            Nodo nodoActual = LeerNodoDeDisco(posicionActual);

            if (valor == nodoActual.valor)
                return true; // Valor encontrado
            else if (valor < nodoActual.valor)
                return BuscarRecursivo(nodoActual.posicionIzquierda, valor); // Buscar en el subárbol izquierdo
            else
                return BuscarRecursivo(nodoActual.posicionDerecha, valor); // Buscar en el subárbol derecho
        }

        // Método para eliminar un valor del árbol
        public void Eliminar(int valor)
        {
            raizPosicion = EliminarRecursivo(raizPosicion, valor);
            Guardar(); // Guardar cambios en el archivo
        }

        private long EliminarRecursivo(long posicionActual, int valor)
        {
            if (posicionActual == -1) return posicionActual; // No encontrado

            Nodo nodoActual = LeerNodoDeDisco(posicionActual);

            // Encontrar el nodo a eliminar
            if (valor < nodoActual.valor)
            {
                nodoActual.posicionIzquierda = EliminarRecursivo(nodoActual.posicionIzquierda, valor); // Eliminar en el subárbol izquierdo
            }
            else if (valor > nodoActual.valor)
            {
                nodoActual.posicionDerecha = EliminarRecursivo(nodoActual.posicionDerecha, valor); // Eliminar en el subárbol derecho
            }
            else
            {
                // Nodo a eliminar encontrado
                if (nodoActual.posicionIzquierda == -1) return nodoActual.posicionDerecha; // Sin hijo izquierdo
                else if (nodoActual.posicionDerecha == -1) return nodoActual.posicionIzquierda; // Sin hijo derecho

                // Nodo con dos hijos: buscar el menor en el subárbol derecho
                Nodo sucesor = LeerNodoDeDisco(nodoActual.posicionDerecha);
                nodoActual.valor = sucesor.valor; // Reemplazar valor
                nodoActual.posicionDerecha = EliminarRecursivo(nodoActual.posicionDerecha, sucesor.valor); // Eliminar sucesor
            }

            // Guardar el nodo actualizado en disco
            EscribirNodoEnDisco(nodoActual, posicionActual);
            return posicionActual; // Devolver la posición actual
        }

        // Método para escribir un nodo en el archivo y devolver su posición
        private long EscribirNodoEnDisco(Nodo nodo, long posicion = -1)
        {
            using (var fs = new FileStream(archivo, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None))
            {
                if (posicion == -1)
                {
                    fs.Seek(0, SeekOrigin.End);
                    posicion = fs.Position;
                }
                else
                {
                    fs.Seek(posicion, SeekOrigin.Begin);
                }

                using (var writer = new BinaryWriter(fs))
                {
                    nodo.Escribir(writer);
                }
            }
            Console.WriteLine($"Escrito el nodo {nodo.valor} en la posición {posicion}");
            return posicion;
        }

        // Método para leer un nodo del archivo desde una posición específica
        private Nodo LeerNodoDeDisco(long posicion)
        {
            using (var fs = new FileStream(archivo, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                fs.Seek(posicion, SeekOrigin.Begin);
                using (var reader = new BinaryReader(fs))
                {
                    var nodo = Nodo.Leer(reader);
                    Console.WriteLine($"Leído el nodo en la posición {posicion}: valor={nodo.valor}, izquierda={nodo.posicionIzquierda}, derecha={nodo.posicionDerecha}");
                    return nodo;
                }
            }
        }
    }


}