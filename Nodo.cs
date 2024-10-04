using System;
using System.IO;
namespace tarea3
{
    
    public class Nodo
    {
        public int valor;
        public long posicionIzquierda;
        public long posicionDerecha;

        public Nodo(int valor, long posicionIzquierda = -1, long posicionDerecha = -1)
        {
            this.valor = valor;
            this.posicionIzquierda = posicionIzquierda;
            this.posicionDerecha = posicionDerecha;
        }

        // Método para escribir el nodo en binario
        public void Escribir(BinaryWriter writer)
        {
            writer.Write(valor);
            writer.Write(posicionIzquierda);
            writer.Write(posicionDerecha);
        }

        // Método estático para leer un nodo desde binario
        public static Nodo Leer(BinaryReader reader)
        {
            int valor = reader.ReadInt32();
            long posicionIzquierda = reader.ReadInt64();
            long posicionDerecha = reader.ReadInt64();
            return new Nodo(valor, posicionIzquierda, posicionDerecha);
        }
    }

}
