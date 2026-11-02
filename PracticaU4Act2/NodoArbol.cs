namespace PracticaU4Act2;

// Cada nodo se crea con new en tiempo de ejecución (memoria dinámica, en el heap)
// y se enlaza con sus hijos mediante referencias.
public sealed class NodoArbol
{
    public NodoArbol(int clave)
    {
        Clave = clave;
    }

    // Los setters son internal: solo el árbol modifica claves y enlaces; la interfaz solo los lee.
    public int Clave { get; internal set; }
    public NodoArbol? Izquierdo { get; internal set; }
    public NodoArbol? Derecho { get; internal set; }

    public bool EsHoja => Izquierdo is null && Derecho is null;
}
