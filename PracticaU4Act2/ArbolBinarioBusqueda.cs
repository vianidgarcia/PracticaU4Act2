namespace PracticaU4Act2;

public sealed record ResultadoOperacion(bool Exito, string Mensaje);

// Camino: claves de los nodos visitados, desde la raíz; su longitud es el número de comparaciones.
public sealed record ResultadoBusqueda(bool Encontrado, IReadOnlyList<int> Camino, string Mensaje);

// Árbol binario de búsqueda: a la izquierda de cada nodo, claves menores; a la derecha, mayores.
// Todas las operaciones son iterativas para que un árbol degenerado (altura n) no desborde la pila.
public sealed class ArbolBinarioBusqueda
{
    private NodoArbol? _raiz;
    private int _cantidad;

    public NodoArbol? Raiz => _raiz;
    public int Cantidad => _cantidad;
    public bool EstaVacio => _raiz is null;

    // El nodo se crea solo al enlazarlo, para no reservar memoria si la clave está repetida.
    public ResultadoOperacion Insertar(int clave)
    {
        if (_raiz is null)
        {
            _raiz = new NodoArbol(clave);
            _cantidad++;
            return new ResultadoOperacion(true, $"Árbol vacío: {clave} se inserta como raíz.");
        }

        NodoArbol actual = _raiz;
        while (true)
        {
            if (clave < actual.Clave)
            {
                if (actual.Izquierdo is null)
                {
                    actual.Izquierdo = new NodoArbol(clave);
                    _cantidad++;
                    return new ResultadoOperacion(true, $"{clave} < {actual.Clave}: se inserta como hijo izquierdo de {actual.Clave}.");
                }
                actual = actual.Izquierdo;
            }
            else if (clave > actual.Clave)
            {
                if (actual.Derecho is null)
                {
                    actual.Derecho = new NodoArbol(clave);
                    _cantidad++;
                    return new ResultadoOperacion(true, $"{clave} > {actual.Clave}: se inserta como hijo derecho de {actual.Clave}.");
                }
                actual = actual.Derecho;
            }
            else
            {
                return new ResultadoOperacion(false, MensajesError.ClaveDuplicada(clave));
            }
        }
    }

    public ResultadoBusqueda Buscar(int clave)
    {
        var camino = new List<int>();
        NodoArbol? actual = _raiz;

        while (actual is not null)
        {
            camino.Add(actual.Clave);
            if (clave == actual.Clave)
                return new ResultadoBusqueda(true, camino,
                    $"{clave} encontrado en {camino.Count} comparaciones: {string.Join(" → ", camino)}.");

            actual = clave < actual.Clave ? actual.Izquierdo : actual.Derecho;
        }

        string mensaje = _raiz is null
            ? MensajesError.ArbolVacio
            : $"{MensajesError.ClaveInexistente(clave)} Se revisaron {camino.Count} nodos: {string.Join(" → ", camino)}.";
        return new ResultadoBusqueda(false, camino, mensaje);
    }

    // Versión sin registrar el camino, para las mediciones.
    public bool Contiene(int clave)
    {
        NodoArbol? actual = _raiz;
        while (actual is not null)
        {
            if (clave == actual.Clave)
                return true;
            actual = clave < actual.Clave ? actual.Izquierdo : actual.Derecho;
        }
        return false;
    }

    public ResultadoOperacion Eliminar(int clave)
    {
        if (_raiz is null)
            return new ResultadoOperacion(false, MensajesError.ArbolVacio);

        NodoArbol? padre = null;
        NodoArbol? actual = _raiz;
        while (actual is not null && actual.Clave != clave)
        {
            padre = actual;
            actual = clave < actual.Clave ? actual.Izquierdo : actual.Derecho;
        }
        if (actual is null)
            return new ResultadoOperacion(false, MensajesError.ClaveInexistente(clave));

        string mensaje;
        if (actual.Izquierdo is not null && actual.Derecho is not null)
        {
            // Caso 3: se copia la clave del sucesor inorden (el mínimo del subárbol derecho)
            // y se elimina el sucesor, que no tiene hijo izquierdo.
            NodoArbol padreSucesor = actual;
            NodoArbol sucesor = actual.Derecho;
            while (sucesor.Izquierdo is not null)
            {
                padreSucesor = sucesor;
                sucesor = sucesor.Izquierdo;
            }

            mensaje = $"Caso 3 (dos hijos): {clave} se sustituye por su sucesor inorden {sucesor.Clave}.";
            actual.Clave = sucesor.Clave;
            Reemplazar(padreSucesor, sucesor, sucesor.Derecho);
        }
        else
        {
            NodoArbol? hijo = actual.Izquierdo ?? actual.Derecho;
            mensaje = hijo is null
                ? $"Caso 1 (hoja): {clave} se elimina directamente."
                : $"Caso 2 (un hijo): {clave} se sustituye por su hijo {hijo.Clave}.";
            Reemplazar(padre, actual, hijo);
        }

        _cantidad--;
        return new ResultadoOperacion(true, mensaje);
    }

    public void Limpiar()
    {
        _raiz = null;   // los nodos sin referencias los libera el recolector de basura
        _cantidad = 0;
    }

    // Raíz, izquierdo, derecho. Se apila primero el derecho para visitar antes el izquierdo.
    public List<int> Preorden()
    {
        var resultado = new List<int>(_cantidad);
        if (_raiz is null)
            return resultado;

        var pila = new Stack<NodoArbol>();
        pila.Push(_raiz);
        while (pila.Count > 0)
        {
            NodoArbol nodo = pila.Pop();
            resultado.Add(nodo.Clave);
            if (nodo.Derecho is not null) pila.Push(nodo.Derecho);
            if (nodo.Izquierdo is not null) pila.Push(nodo.Izquierdo);
        }
        return resultado;
    }

    // Izquierdo, raíz, derecho. Se baja por la izquierda apilando y, al desapilar, se visita y se pasa a la derecha.
    public List<int> Inorden()
    {
        var resultado = new List<int>(_cantidad);
        var pila = new Stack<NodoArbol>();
        NodoArbol? actual = _raiz;

        while (actual is not null || pila.Count > 0)
        {
            while (actual is not null)
            {
                pila.Push(actual);
                actual = actual.Izquierdo;
            }
            actual = pila.Pop();
            resultado.Add(actual.Clave);
            actual = actual.Derecho;
        }
        return resultado;
    }

    // Izquierdo, derecho, raíz. Se recorre raíz-derecho-izquierdo y se invierte el resultado.
    public List<int> Postorden()
    {
        var resultado = new List<int>(_cantidad);
        if (_raiz is null)
            return resultado;

        var pila = new Stack<NodoArbol>();
        pila.Push(_raiz);
        while (pila.Count > 0)
        {
            NodoArbol nodo = pila.Pop();
            resultado.Add(nodo.Clave);
            if (nodo.Izquierdo is not null) pila.Push(nodo.Izquierdo);
            if (nodo.Derecho is not null) pila.Push(nodo.Derecho);
        }
        resultado.Reverse();
        return resultado;
    }

    // Altura = aristas del camino más largo de la raíz a una hoja: solo la raíz = 0; árbol vacío = -1 por convención.
    // Se recorre nivel por nivel con una cola; la altura es el número de niveles menos uno.
    public int Altura()
    {
        if (_raiz is null)
            return -1;

        var cola = new Queue<NodoArbol>();
        cola.Enqueue(_raiz);
        int niveles = 0;

        while (cola.Count > 0)
        {
            niveles++;
            for (int i = cola.Count; i > 0; i--)
            {
                NodoArbol nodo = cola.Dequeue();
                if (nodo.Izquierdo is not null) cola.Enqueue(nodo.Izquierdo);
                if (nodo.Derecho is not null) cola.Enqueue(nodo.Derecho);
            }
        }
        return niveles - 1;
    }

    // Un ABB es válido si su recorrido inorden sale estrictamente creciente.
    public bool EsValido()
    {
        List<int> inorden = Inorden();
        for (int i = 1; i < inorden.Count; i++)
        {
            if (inorden[i - 1] >= inorden[i])
                return false;
        }
        return inorden.Count == _cantidad;
    }

    // Hace que el padre apunte a 'nuevo' en lugar de 'viejo'; sin padre, cambia la raíz.
    private void Reemplazar(NodoArbol? padre, NodoArbol viejo, NodoArbol? nuevo)
    {
        if (padre is null)
            _raiz = nuevo;
        else if (padre.Izquierdo == viejo)
            padre.Izquierdo = nuevo;
        else
            padre.Derecho = nuevo;
    }
}