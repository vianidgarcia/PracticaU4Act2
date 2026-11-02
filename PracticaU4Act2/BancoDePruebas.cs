using System.Diagnostics;
using System.Globalization;
using System.Text;

namespace PracticaU4Act2;

public sealed record PruebaResultado(string Caso, bool Paso, string Detalle);

// Casos de prueba de la integridad de los recorridos y de las operaciones, y mediciones.
// No usa controles: devuelve datos y texto para que el formulario los muestre.
public static class BancoDePruebas
{
    // Árbol de ejemplo del reporte; se usa también en el botón "Ejemplo".
    public static readonly int[] ClavesEjemplo = { 50, 30, 70, 20, 40, 60, 80, 35, 45, 65 };

    public static List<PruebaResultado> EjecutarCasos()
    {
        var casos = new List<PruebaResultado>();

        // Valores esperados calculados a mano y con un modelo independiente.
        ArbolBinarioBusqueda ejemplo = CrearEjemplo();
        casos.Add(Comparar("Preorden del árbol de ejemplo", ejemplo.Preorden(), 50, 30, 20, 40, 35, 45, 70, 60, 65, 80));
        casos.Add(Comparar("Inorden del árbol de ejemplo", ejemplo.Inorden(), 20, 30, 35, 40, 45, 50, 60, 65, 70, 80));
        casos.Add(Comparar("Postorden del árbol de ejemplo", ejemplo.Postorden(), 20, 35, 45, 40, 30, 65, 60, 80, 70, 50));
        casos.Add(new PruebaResultado("Altura y cantidad del ejemplo",
            ejemplo.Altura() == 3 && ejemplo.Cantidad == 10, $"altura {ejemplo.Altura()} (camino 50-30-40-35), {ejemplo.Cantidad} nodos"));

        casos.Add(ProbarBusqueda(ejemplo, 45, true, "50 → 30 → 40 → 45"));
        casos.Add(ProbarBusqueda(ejemplo, 66, false, "50 → 70 → 60 → 65"));

        casos.Add(ProbarEliminacion(20, "Caso 1 (hoja)", 50, 30, 40, 35, 45, 70, 60, 65, 80));
        casos.Add(ProbarEliminacion(60, "Caso 2 (un hijo)", 50, 30, 20, 40, 35, 45, 70, 65, 80));
        casos.Add(ProbarEliminacion(30, "Caso 3 (dos hijos)", 50, 35, 20, 40, 45, 70, 60, 65, 80));
        casos.Add(ProbarEliminacion(50, "Caso 3 (dos hijos)", 60, 30, 20, 40, 35, 45, 70, 65, 80));

        casos.Add(ProbarArbolVacio());
        casos.Add(ProbarUnSoloNodo());
        casos.Add(ProbarDuplicado());
        casos.Add(ProbarEliminarInexistente());
        casos.Add(ProbarEntrada());
        casos.Add(ProbarDegenerado());
        casos.Add(ProbarReconstruccion());
        casos.Add(ProbarAleatoriosContraReferencia());
        return casos;
    }

    public static ArbolBinarioBusqueda CrearEjemplo()
    {
        var arbol = new ArbolBinarioBusqueda();
        foreach (int clave in ClavesEjemplo)
            arbol.Insertar(clave);
        return arbol;
    }

    private static PruebaResultado Comparar(string caso, List<int> obtenido, params int[] esperado) =>
        new(caso, obtenido.SequenceEqual(esperado), string.Join(" ", obtenido));

    private static PruebaResultado ProbarBusqueda(ArbolBinarioBusqueda arbol, int clave, bool debeEncontrar, string caminoEsperado)
    {
        ResultadoBusqueda r = arbol.Buscar(clave);
        string camino = string.Join(" → ", r.Camino);
        return new PruebaResultado($"Buscar {clave} ({(debeEncontrar ? "existe" : "no existe")})",
            r.Encontrado == debeEncontrar && camino == caminoEsperado, $"camino {camino}, {r.Camino.Count} comparaciones");
    }

    // Después de eliminar se revisan el caso aplicado, el preorden esperado, el inorden ordenado y la cantidad.
    private static PruebaResultado ProbarEliminacion(int clave, string casoEsperado, params int[] preordenEsperado)
    {
        ArbolBinarioBusqueda arbol = CrearEjemplo();
        ResultadoOperacion r = arbol.Eliminar(clave);
        List<int> preorden = arbol.Preorden();
        bool ok = r.Exito && r.Mensaje.StartsWith(casoEsperado) && preorden.SequenceEqual(preordenEsperado)
                  && arbol.EsValido() && arbol.Cantidad == 9 && !arbol.Contiene(clave);
        return new PruebaResultado($"Eliminar {clave}: {casoEsperado}", ok, $"preorden {string.Join(" ", preorden)}");
    }

    private static PruebaResultado ProbarArbolVacio()
    {
        var a = new ArbolBinarioBusqueda();
        bool ok = a.Preorden().Count == 0 && a.Inorden().Count == 0 && a.Postorden().Count == 0
                  && a.Altura() == -1 && !a.Buscar(5).Encontrado && !a.Eliminar(5).Exito;
        return new PruebaResultado("Árbol vacío", ok, "recorridos vacíos, altura -1, buscar y eliminar fallan sin error");
    }

    private static PruebaResultado ProbarUnSoloNodo()
    {
        var a = new ArbolBinarioBusqueda();
        a.Insertar(7);
        bool recorridos = a.Preorden().SequenceEqual(new[] { 7 }) && a.Inorden().SequenceEqual(new[] { 7 })
                          && a.Postorden().SequenceEqual(new[] { 7 }) && a.Altura() == 0;
        bool eliminado = a.Eliminar(7).Exito && a.EstaVacio && a.Cantidad == 0;
        return new PruebaResultado("Un solo nodo (insertar y eliminar la raíz)", recorridos && eliminado,
            "los tres recorridos dan 7; al eliminarlo el árbol queda vacío");
    }

    private static PruebaResultado ProbarDuplicado()
    {
        ArbolBinarioBusqueda a = CrearEjemplo();
        ResultadoOperacion r = a.Insertar(40);
        return new PruebaResultado("Clave duplicada", !r.Exito && a.Cantidad == 10 && r.Mensaje == MensajesError.ClaveDuplicada(40),
            $"mensaje: \"{r.Mensaje}\"");
    }

    private static PruebaResultado ProbarEliminarInexistente()
    {
        ArbolBinarioBusqueda a = CrearEjemplo();
        ResultadoOperacion r = a.Eliminar(99);
        return new PruebaResultado("Eliminar clave inexistente", !r.Exito && a.Cantidad == 10 && a.EsValido(),
            $"mensaje: \"{r.Mensaje}\"");
    }

    private static PruebaResultado ProbarEntrada()
    {
        bool secuencia = EntradaClaves.LeerSecuencia(" 10, -5;20  7 ").SequenceEqual(new[] { 10, -5, 20, 7 });
        bool vacia = LanzaConMensaje(() => EntradaClaves.LeerSecuencia("  "), MensajesError.SecuenciaVacia);
        bool letra = LanzaConMensaje(() => EntradaClaves.LeerSecuencia("10, abc"), MensajesError.ClaveInvalida("abc"));
        bool claveVacia = LanzaConMensaje(() => EntradaClaves.LeerClave(""), MensajesError.ClaveVacia);
        return new PruebaResultado("Validación de la entrada", secuencia && vacia && letra && claveVacia,
            "separadores mixtos, negativos, texto vacío y texto no numérico");
    }

    // Con claves ordenadas el árbol degenera en una lista; las operaciones iterativas no desbordan la pila.
    private static PruebaResultado ProbarDegenerado()
    {
        const int n = 10000;
        var a = new ArbolBinarioBusqueda();
        for (int i = 1; i <= n; i++)
            a.Insertar(i);

        bool ok = a.Altura() == n - 1 && a.Inorden().SequenceEqual(Enumerable.Range(1, n))
                  && a.Preorden().SequenceEqual(Enumerable.Range(1, n))
                  && a.Postorden().SequenceEqual(Enumerable.Range(1, n).Reverse())
                  && a.Eliminar(1).Exito && a.Eliminar(n).Exito && a.EsValido();
        return new PruebaResultado("Árbol degenerado de 10 000 nodos", ok, "altura 9 999; recorridos y eliminación sin desbordar la pila");
    }

    // Insertar las claves en el orden del preorden reconstruye exactamente el mismo árbol.
    private static PruebaResultado ProbarReconstruccion()
    {
        ArbolBinarioBusqueda original = ArbolAleatorio(new Random(3), 200, out _);
        var copia = new ArbolBinarioBusqueda();
        foreach (int clave in original.Preorden())
            copia.Insertar(clave);

        bool ok = copia.Preorden().SequenceEqual(original.Preorden())
                  && copia.Inorden().SequenceEqual(original.Inorden())
                  && copia.Postorden().SequenceEqual(original.Postorden());
        return new PruebaResultado("Reconstrucción desde el preorden", ok, "200 nodos: los tres recorridos de la copia coinciden");
    }

    // 300 árboles con inserciones y eliminaciones al azar contra SortedSet y contra recorridos recursivos.
    private static PruebaResultado ProbarAleatoriosContraReferencia()
    {
        var azar = new Random(1);
        for (int prueba = 0; prueba < 300; prueba++)
        {
            ArbolBinarioBusqueda a = ArbolAleatorio(azar, azar.Next(0, 80), out SortedSet<int> referencia);

            bool ok = a.Inorden().SequenceEqual(referencia)
                      && a.Preorden().SequenceEqual(PreordenRecursivo(a.Raiz))
                      && a.Postorden().SequenceEqual(PostordenRecursivo(a.Raiz))
                      && a.Cantidad == referencia.Count && a.EsValido();
            if (!ok)
                return new PruebaResultado("300 árboles aleatorios", false, $"falla en la prueba {prueba + 1}");
        }
        return new PruebaResultado("300 árboles aleatorios", true,
            "inorden = SortedSet; preorden y postorden = versiones recursivas");
    }

    // Inserta y elimina claves al azar en el árbol y en un SortedSet de referencia.
    private static ArbolBinarioBusqueda ArbolAleatorio(Random azar, int operaciones, out SortedSet<int> referencia)
    {
        var a = new ArbolBinarioBusqueda();
        referencia = new SortedSet<int>();
        for (int i = 0; i < operaciones; i++)
        {
            int clave = azar.Next(-500, 500);
            if (a.Insertar(clave).Exito != referencia.Add(clave))
                throw new InvalidOperationException("Insertar no coincide con la referencia.");

            if (azar.NextDouble() < 0.3 && referencia.Count > 0)
            {
                int eliminar = referencia.ElementAt(azar.Next(referencia.Count));
                a.Eliminar(eliminar);
                referencia.Remove(eliminar);
            }
        }
        return a;
    }

    private static List<int> PreordenRecursivo(NodoArbol? nodo)
    {
        if (nodo is null) return new List<int>();
        var r = new List<int> { nodo.Clave };
        r.AddRange(PreordenRecursivo(nodo.Izquierdo));
        r.AddRange(PreordenRecursivo(nodo.Derecho));
        return r;
    }

    private static List<int> PostordenRecursivo(NodoArbol? nodo)
    {
        if (nodo is null) return new List<int>();
        var r = PostordenRecursivo(nodo.Izquierdo);
        r.AddRange(PostordenRecursivo(nodo.Derecho));
        r.Add(nodo.Clave);
        return r;
    }

    private static bool LanzaConMensaje(Action accion, string mensaje)
    {
        try
        {
            accion();
            return false;
        }
        catch (ArgumentException ex)
        {
            return ex.Message == mensaje;
        }
    }

    // Compara un árbol con claves en orden aleatorio contra uno con claves ordenadas (degenerado).
    // Cada tiempo es la mediana de 5 corridas, después de una de calentamiento.
    public static string EjecutarMediciones()
    {
        int[] tamanos = { 1000, 5000, 10000, 20000 };
        const int busquedas = 1000;
        var sb = new StringBuilder();

        sb.AppendLine("Altura en aristas y construcción (insertar las n claves)");
        sb.AppendLine($"{"n",8}{"log2(n)",10}{"Altura aleat.",15}{"Altura orden.",15}{"Construir aleat. (ms)",23}{"Construir orden. (ms)",23}");
        var filasBusqueda = new List<string>();

        foreach (int n in tamanos)
        {
            int[] aleatorias = Barajar(Enumerable.Range(1, n).ToArray(), new Random(42));
            int[] ordenadas = Enumerable.Range(1, n).ToArray();

            ArbolBinarioBusqueda arbolAleatorio = Construir(aleatorias);
            ArbolBinarioBusqueda arbolOrdenado = Construir(ordenadas);

            double msConstruirAleatorio = MedianaMs(() => Construir(aleatorias));
            double msConstruirOrdenado = MedianaMs(() => Construir(ordenadas));

            var azar = new Random(7);
            int[] objetivos = Enumerable.Range(0, busquedas).Select(_ => azar.Next(1, n + 1)).ToArray();
            double msBuscarAleatorio = MedianaMs(() => BuscarTodas(arbolAleatorio, objetivos));
            double msBuscarOrdenado = MedianaMs(() => BuscarTodas(arbolOrdenado, objetivos));

            string log2 = Math.Log2(n).ToString("F1", CultureInfo.InvariantCulture);
            sb.AppendLine($"{n,8}{log2,10}{arbolAleatorio.Altura(),15}{arbolOrdenado.Altura(),15}{Ms(msConstruirAleatorio),23}{Ms(msConstruirOrdenado),23}");
            filasBusqueda.Add($"{n,8}{Ms(msBuscarAleatorio),20}{Ms(msBuscarOrdenado),20}{Factor(msBuscarOrdenado, msBuscarAleatorio),12}");
        }

        sb.AppendLine();
        sb.AppendLine($"{busquedas} búsquedas de claves existentes");
        sb.AppendLine($"{"n",8}{"Aleatorio (ms)",20}{"Ordenado (ms)",20}{"Ordenado / aleatorio",22}");
        foreach (string fila in filasBusqueda)
            sb.AppendLine(fila);
        return sb.ToString();
    }

    private static ArbolBinarioBusqueda Construir(int[] claves)
    {
        var arbol = new ArbolBinarioBusqueda();
        foreach (int clave in claves)
            arbol.Insertar(clave);
        return arbol;
    }

    private static int BuscarTodas(ArbolBinarioBusqueda arbol, int[] claves)
    {
        int encontradas = 0;
        foreach (int clave in claves)
            if (arbol.Contiene(clave)) encontradas++;
        return encontradas;
    }

    // Fisher-Yates: permutación aleatoria uniforme de las claves.
    private static int[] Barajar(int[] claves, Random azar)
    {
        for (int i = claves.Length - 1; i > 0; i--)
        {
            int j = azar.Next(i + 1);
            (claves[i], claves[j]) = (claves[j], claves[i]);
        }
        return claves;
    }

    private static double MedianaMs(Action accion)
    {
        accion();   // calentamiento del JIT
        var tiempos = new double[5];
        for (int k = 0; k < tiempos.Length; k++)
        {
            var reloj = Stopwatch.StartNew();
            accion();
            reloj.Stop();
            tiempos[k] = reloj.Elapsed.TotalMilliseconds;
        }
        Array.Sort(tiempos);
        return tiempos[2];
    }

    private static string Ms(double valor) => valor.ToString("F2", CultureInfo.InvariantCulture);

    private static string Factor(double a, double b) =>
        b > 0 ? (a / b).ToString("F0", CultureInfo.InvariantCulture) + "x" : "-";
}