using System;
using System.IO;
namespace tarea3
{
    public class Nodo

    {
        public int valor;
        public long posicionIzquierda;  // Posición en el archivo del hijo izquierdo
        public long posicionDerecha;    // Posición en el archivo del hijo derecho

        public Nodo(int valor, long posicionIzquierda = -1, long posicionDerecha = -1)
        {
            this.valor = valor;
            this.posicionIzquierda = posicionIzquierda;
            this.posicionDerecha = posicionDerecha;
        }

        // Convertir el nodo a una cadena que se pueda almacenar en disco
        public override string ToString()
        {
            return $"{valor},{posicionIzquierda},{posicionDerecha}";
        }

        // Método estático para construir un nodo a partir de una cadena leída del disco
        public static Nodo DesdeString(string datos)
        {
            if (string.IsNullOrWhiteSpace(datos))
            {
                throw new ArgumentException("Los datos no son validos");

            }
            var partes = datos.Split(',');
            if (partes.Length != 3)
            {
                throw new FormatException("Los datos del nodo no estan en el formato correcto");
            }
            try 
            {
                int valor = int.Parse(partes[0]);
                long posicionIzquierda = long.Parse(partes[1]);
                long posicionDerecha = long.Parse(partes[2]);
                return new Nodo(valor,posicionIzquierda,posicionDerecha);
            }
            catch (Exception ex)
            {
                throw new FormatException($"Error al convertir los datos del nodo: '{datos}'. Detalles: {ex.Message}");
            }
           
        }
    }
}
