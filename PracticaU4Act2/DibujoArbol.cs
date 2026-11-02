using System.Drawing.Drawing2D;

namespace PracticaU4Act2;

// Qué resaltar al dibujar: el camino de la última búsqueda y un nodo destacado,
// en verde si se encontró o insertó, en rojo si es donde se detuvo una búsqueda sin éxito.
public sealed record Resaltado(IReadOnlyCollection<int> Camino, int? Clave, bool ClaveEncontrada)
{
    public static readonly Resaltado Ninguno = new(Array.Empty<int>(), null, false);
}

// Dibuja el árbol sobre cualquier Graphics. La posición horizontal de cada nodo es su lugar
// en el recorrido inorden y la vertical es su profundidad, así los nodos nunca se enciman.
public static class DibujoArbol
{
    // Con más nodos, el espacio horizontal por nodo es menor que el círculo más pequeño y se enciman.
    public const int MaximoNodosDibujables = 60;

    private const int Margen = 30;
    private const float RadioMaximo = 18F;
    private const float RadioMinimo = 7F;

    private static readonly Color ColorCamino = Color.FromArgb(255, 236, 150);
    private static readonly Color ColorEncontrado = Color.FromArgb(120, 200, 120);
    private static readonly Color ColorNoEncontrado = Color.FromArgb(240, 140, 140);

    public static Dictionary<NodoArbol, PointF> CalcularPosiciones(NodoArbol? raiz, Size area)
    {
        var lista = RecorrerConProfundidad(raiz);
        var posiciones = new Dictionary<NodoArbol, PointF>(lista.Count);
        if (lista.Count == 0)
            return posiciones;

        int niveles = lista.Max(x => x.Profundidad) + 1;
        float pasoX = (area.Width - 2f * Margen) / lista.Count;
        float pasoY = niveles > 1 ? Math.Min(70f, (area.Height - 2f * Margen) / (niveles - 1)) : 0f;

        for (int i = 0; i < lista.Count; i++)
        {
            float x = Margen + pasoX * (i + 0.5f);
            float y = Margen + pasoY * lista[i].Profundidad;
            posiciones[lista[i].Nodo] = new PointF(x, y);
        }
        return posiciones;
    }

    public static float CalcularRadio(int cantidad, Size area)
    {
        if (cantidad == 0)
            return RadioMaximo;
        float porAncho = (area.Width - 2f * Margen) / cantidad / 2.3f;
        return Math.Clamp(porAncho, RadioMinimo, RadioMaximo);
    }

    public static void Dibujar(Graphics g, Size area, ArbolBinarioBusqueda arbol,
                               Dictionary<NodoArbol, PointF> posiciones, Resaltado resaltado)
    {
        g.SmoothingMode = SmoothingMode.AntiAlias;
        g.Clear(Color.White);

        using var fuenteAviso = new Font("Segoe UI", 10F);
        if (arbol.EstaVacio)
        {
            g.DrawString("El árbol está vacío. Inserta una clave o carga una secuencia.", fuenteAviso, Brushes.Black, Margen, Margen);
            return;
        }
        if (arbol.Cantidad > MaximoNodosDibujables)
        {
            g.DrawString($"El árbol tiene {arbol.Cantidad} nodos; solo se dibujan árboles de hasta {MaximoNodosDibujables}.",
                         fuenteAviso, Brushes.Black, Margen, Margen);
            return;
        }

        float radio = CalcularRadio(arbol.Cantidad, area);
        using var fuente = new Font("Segoe UI", Math.Max(6F, radio * 0.55F), FontStyle.Bold);
        using var lapizArista = new Pen(Color.FromArgb(110, 110, 110), 1.5F);
        using var lapizCamino = new Pen(Color.FromArgb(220, 160, 0), 3F);
        var enCamino = new HashSet<int>(resaltado.Camino);

        // Primero las aristas, para que los nodos queden encima.
        foreach (var (nodo, punto) in posiciones)
        {
            foreach (NodoArbol? hijo in new[] { nodo.Izquierdo, nodo.Derecho })
            {
                if (hijo is null || !posiciones.TryGetValue(hijo, out PointF puntoHijo))
                    continue;
                bool aristaEnCamino = enCamino.Contains(nodo.Clave) && enCamino.Contains(hijo.Clave);
                g.DrawLine(aristaEnCamino ? lapizCamino : lapizArista, punto, puntoHijo);
            }
        }

        foreach (var (nodo, punto) in posiciones)
        {
            Color relleno = Color.White;
            if (resaltado.Clave == nodo.Clave)
                relleno = resaltado.ClaveEncontrada ? ColorEncontrado : ColorNoEncontrado;
            else if (enCamino.Contains(nodo.Clave))
                relleno = ColorCamino;

            var circulo = new RectangleF(punto.X - radio, punto.Y - radio, 2 * radio, 2 * radio);
            using (var pincel = new SolidBrush(relleno))
                g.FillEllipse(pincel, circulo);
            g.DrawEllipse(Pens.Black, circulo);

            string texto = nodo.Clave.ToString();
            SizeF tamano = g.MeasureString(texto, fuente);
            g.DrawString(texto, fuente, Brushes.Black, punto.X - tamano.Width / 2, punto.Y - tamano.Height / 2);
        }
    }

    // Devuelve el nodo cuyo círculo contiene el punto, o null.
    public static NodoArbol? NodoEn(Point punto, Dictionary<NodoArbol, PointF> posiciones, float radio)
    {
        foreach (var (nodo, centro) in posiciones)
        {
            float dx = punto.X - centro.X;
            float dy = punto.Y - centro.Y;
            if (dx * dx + dy * dy <= radio * radio)
                return nodo;
        }
        return null;
    }

    // Inorden iterativo que además guarda la profundidad de cada nodo.
    private static List<(NodoArbol Nodo, int Profundidad)> RecorrerConProfundidad(NodoArbol? raiz)
    {
        var resultado = new List<(NodoArbol, int)>();
        var pila = new Stack<(NodoArbol Nodo, int Profundidad)>();
        NodoArbol? actual = raiz;
        int profundidad = 0;

        while (actual is not null || pila.Count > 0)
        {
            while (actual is not null)
            {
                pila.Push((actual, profundidad));
                actual = actual.Izquierdo;
                profundidad++;
            }
            var (nodo, p) = pila.Pop();
            resultado.Add((nodo, p));
            actual = nodo.Derecho;
            profundidad = p + 1;
        }
        return resultado;
    }
}