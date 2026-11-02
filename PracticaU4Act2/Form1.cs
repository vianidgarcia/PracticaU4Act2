namespace PracticaU4Act2;

// El formulario solo atiende eventos y muestra resultados; la lógica está en ArbolBinarioBusqueda.
public partial class Form1 : Form
{
    private const int MaximoClavesEnRecorrido = 300;

    private readonly ArbolBinarioBusqueda _arbol = new();
    private Dictionary<NodoArbol, PointF> _posiciones = new();
    private Resaltado _resaltado = Resaltado.Ninguno;

    public Form1()
    {
        InitializeComponent();
    }

    private void Form1_Load(object? sender, EventArgs e)
    {
        btnEjemplo_Click(sender, e);
    }

    private void txtClave_KeyDown(object? sender, KeyEventArgs e)
    {
        if (e.KeyCode == Keys.Enter)
        {
            e.SuppressKeyPress = true;
            btnInsertar_Click(sender, e);
        }
    }

    private void btnInsertar_Click(object? sender, EventArgs e)
    {
        if (!IntentarLeerClave(out int clave))
            return;

        ResultadoOperacion r = _arbol.Insertar(clave);
        _resaltado = r.Exito ? new Resaltado(Array.Empty<int>(), clave, true) : Resaltado.Ninguno;
        MostrarEstado(r.Mensaje);
        txtClave.SelectAll();
        ActualizarVista();
    }

    private void btnBuscar_Click(object? sender, EventArgs e)
    {
        if (!IntentarLeerClave(out int clave))
            return;

        ResultadoBusqueda r = _arbol.Buscar(clave);
        // Si no se encontró, se destaca en rojo el último nodo revisado: ahí se detuvo la búsqueda.
        int? destacado = null;
        if (r.Encontrado)
            destacado = clave;
        else if (r.Camino.Count > 0)
            destacado = r.Camino[r.Camino.Count - 1];
        _resaltado = new Resaltado(r.Camino, destacado, r.Encontrado);
        MostrarEstado(r.Mensaje);
        picArbol.Invalidate();
    }

    private void btnEliminar_Click(object? sender, EventArgs e)
    {
        if (!IntentarLeerClave(out int clave))
            return;

        ResultadoOperacion r = _arbol.Eliminar(clave);
        _resaltado = Resaltado.Ninguno;
        MostrarEstado(r.Mensaje);
        ActualizarVista();
    }

    private void btnLimpiar_Click(object? sender, EventArgs e)
    {
        _arbol.Limpiar();
        _resaltado = Resaltado.Ninguno;
        MostrarEstado("Árbol vaciado.");
        ActualizarVista();
    }

    private void btnCargar_Click(object? sender, EventArgs e)
    {
        try
        {
            List<int> claves = EntradaClaves.LeerSecuencia(txtSecuencia.Text);
            CargarClaves(claves, "Secuencia cargada.");
        }
        catch (ArgumentException ex)
        {
            MostrarEstado(ex.Message);
        }
    }

    private void btnAleatorio_Click(object? sender, EventArgs e)
    {
        // 15 claves distintas entre 1 y 99 en orden aleatorio.
        int[] claves = Enumerable.Range(1, 99).OrderBy(_ => Random.Shared.Next()).Take(15).ToArray();
        txtSecuencia.Text = string.Join(", ", claves);
        CargarClaves(claves, "Claves aleatorias cargadas.");
    }

    private void btnEjemplo_Click(object? sender, EventArgs e)
    {
        txtSecuencia.Text = string.Join(", ", BancoDePruebas.ClavesEjemplo);
        CargarClaves(BancoDePruebas.ClavesEjemplo, "Árbol de ejemplo cargado.");
    }

    // Al hacer clic sobre un nodo, su clave pasa al cuadro de texto.
    private void picArbol_MouseClick(object? sender, MouseEventArgs e)
    {
        float radio = DibujoArbol.CalcularRadio(_arbol.Cantidad, picArbol.ClientSize);
        NodoArbol? nodo = DibujoArbol.NodoEn(e.Location, _posiciones, radio);
        if (nodo is null)
            return;

        txtClave.Text = nodo.Clave.ToString();
        string tipo = nodo.EsHoja ? "hoja" : (nodo.Izquierdo is not null && nodo.Derecho is not null ? "con dos hijos" : "con un hijo");
        MostrarEstado($"Nodo {nodo.Clave} seleccionado ({tipo}).");
    }

    private void picArbol_Paint(object? sender, PaintEventArgs e)
    {
        _posiciones = _arbol.Cantidad <= DibujoArbol.MaximoNodosDibujables
            ? DibujoArbol.CalcularPosiciones(_arbol.Raiz, picArbol.ClientSize)
            : new Dictionary<NodoArbol, PointF>();
        DibujoArbol.Dibujar(e.Graphics, picArbol.ClientSize, _arbol, _posiciones, _resaltado);
    }

    private void picArbol_Resize(object? sender, EventArgs e)
    {
        picArbol.Invalidate();
    }

    // Las pruebas y mediciones tardan, así que corren en otro hilo para no congelar la ventana.
    private async void btnPruebas_Click(object? sender, EventArgs e)
    {
        btnPruebas.Enabled = false;
        txtPruebas.Text = "Ejecutando pruebas y mediciones...";

        try
        {
            txtPruebas.Text = await Task.Run(ConstruirReportePruebas);
        }
        catch (Exception ex)
        {
            txtPruebas.Text = "Error inesperado durante las pruebas: " + ex.Message;
        }
        finally
        {
            btnPruebas.Enabled = true;
        }
    }

    private static string ConstruirReportePruebas()
    {
        List<PruebaResultado> casos = BancoDePruebas.EjecutarCasos();
        var lineas = new List<string>();

        foreach (PruebaResultado caso in casos)
            lineas.Add($"[{(caso.Paso ? "OK" : "FALLA")}] {caso.Caso}: {caso.Detalle}");

        lineas.Add(string.Empty);
        lineas.Add($"{casos.Count(c => c.Paso)} de {casos.Count} casos aprobados.");
        lineas.Add(string.Empty);
        lineas.Add(BancoDePruebas.EjecutarMediciones());

#if DEBUG
        lineas.Add("Aviso: el programa está compilado en Debug; los tiempos no son representativos. Usa Release.");
#endif
        return string.Join(Environment.NewLine, lineas);
    }

    private void CargarClaves(IEnumerable<int> claves, string mensaje)
    {
        _arbol.Limpiar();
        int omitidas = 0;
        foreach (int clave in claves)
        {
            if (!_arbol.Insertar(clave).Exito)
                omitidas++;
        }

        _resaltado = Resaltado.Ninguno;
        MostrarEstado(omitidas == 0 ? mensaje : $"{mensaje} Se omitieron {omitidas} claves duplicadas.");
        ActualizarVista();
    }

    private bool IntentarLeerClave(out int clave)
    {
        try
        {
            clave = EntradaClaves.LeerClave(txtClave.Text);
            return true;
        }
        catch (ArgumentException ex)
        {
            clave = 0;
            MostrarEstado(ex.Message);
            return false;
        }
    }

    private void ActualizarVista()
    {
        string altura = _arbol.EstaVacio ? "—" : _arbol.Altura().ToString();
        lblMetricas.Text = $"Nodos: {_arbol.Cantidad}   |   Altura: {altura}";
        txtRecorridos.Text =
            $"Preorden:  {Formatear(_arbol.Preorden())}{Environment.NewLine}" +
            $"Inorden:   {Formatear(_arbol.Inorden())}{Environment.NewLine}" +
            $"Postorden: {Formatear(_arbol.Postorden())}";
        picArbol.Invalidate();
    }

    private static string Formatear(List<int> claves)
    {
        if (claves.Count == 0)
            return "(vacío)";
        if (claves.Count <= MaximoClavesEnRecorrido)
            return string.Join(" ", claves);
        return string.Join(" ", claves.Take(MaximoClavesEnRecorrido)) + $" … ({claves.Count} claves)";
    }

    private void MostrarEstado(string mensaje)
    {
        lblEstado.Text = mensaje;
    }
}