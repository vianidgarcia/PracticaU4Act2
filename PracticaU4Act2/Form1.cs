using System.DirectoryServices;

namespace PracticaU4Act2;

// The form only handles events and displays results; the logic is in BinarySearchTree.
public partial class Form1 : Form
{
    private const int MaxKeysInTraversal = 300;

    private readonly BinarySearchTree _tree = new();
    private Dictionary<TreeNode, PointF> _positions = new();
    private Highlight _highlight = Highlight.None;

    public Form1()
    {
        InitializeComponent();
    }

    private void Form1_Load(object? sender, EventArgs e)
    {
        btnSample_Click(sender, e);
    }

    private void txtKey_KeyDown(object? sender, KeyEventArgs e)
    {
        if (e.KeyCode == Keys.Enter)
        {
            e.SuppressKeyPress = true;
            btnInsert_Click(sender, e);
        }
    }

    private void btnInsert_Click(object? sender, EventArgs e)
    {
        if (!TryReadKey(out int key))
            return;

        OperationResult r = _tree.Insert(key);
        _highlight = r.Success ? new Highlight(Array.Empty<int>(), key, true) : Highlight.None;
        ShowStatus(r.Message);
        txtKey.SelectAll();
        UpdateView();
    }

    private void btnSearch_Click(object? sender, EventArgs e)
    {
        if (!TryReadKey(out int key))
            return;

        SearchResult r = _tree.Search(key);
        // If it was not found, the last node checked is highlighted in red: that is where the search stopped.
        int? featured = null;
        if (r.Found)
            featured = key;
        else if (r.Path.Count > 0)
            featured = r.Path[r.Path.Count - 1];
        _highlight = new Highlight(r.Path, featured, r.Found);
        ShowStatus(r.Message);
        picTree.Invalidate();
    }

    private void btnDelete_Click(object? sender, EventArgs e)
    {
        if (!TryReadKey(out int key))
            return;

        OperationResult r = _tree.Delete(key);
        _highlight = Highlight.None;
        ShowStatus(r.Message);
        UpdateView();
    }

    private void btnClear_Click(object? sender, EventArgs e)
    {
        _tree.Clear();
        _highlight = Highlight.None;
        ShowStatus("Árbol vaciado.");
        UpdateView();
    }

    private void btnLoad_Click(object? sender, EventArgs e)
    {
        try
        {
            List<int> keys = KeyInput.ReadSequence(txtSequence.Text);
            LoadKeys(keys, "Secuencia cargada.");
        }
        catch (ArgumentException ex)
        {
            ShowStatus(ex.Message);
        }
    }

    private void btnRandom_Click(object? sender, EventArgs e)
    {
        // 15 distinct keys between 1 and 99 in random order.
        int[] keys = Enumerable.Range(1, 99).OrderBy(_ => Random.Shared.Next()).Take(15).ToArray();
        txtSequence.Text = string.Join(", ", keys);
        LoadKeys(keys, "Claves aleatorias cargadas.");
    }

    private void btnSample_Click(object? sender, EventArgs e)
    {
        txtSequence.Text = string.Join(", ", TestBench.SampleKeys);
        LoadKeys(TestBench.SampleKeys, "Árbol de ejemplo cargado.");
    }

    // Clicking a node copies its key into the text box.
    private void picTree_MouseClick(object? sender, MouseEventArgs e)
    {
        float radius = TreeDrawing.CalculateRadius(_tree.Count, picTree.ClientSize);
        TreeNode? node = TreeDrawing.NodeAt(e.Location, _positions, radius);
        if (node is null)
            return;

        txtKey.Text = node.Key.ToString();
        string kind = node.IsLeaf ? "hoja" : (node.Left is not null && node.Right is not null ? "con dos hijos" : "con un hijo");
        ShowStatus($"Nodo {node.Key} seleccionado ({kind}).");
    }

    private void picTree_Paint(object? sender, PaintEventArgs e)
    {
        _positions = _tree.Count <= TreeDrawing.MaxDrawableNodes
            ? TreeDrawing.CalculatePositions(_tree.Root, picTree.ClientSize)
            : new Dictionary<TreeNode, PointF>();
        TreeDrawing.Draw(e.Graphics, picTree.ClientSize, _tree, _positions, _highlight);
    }

    private void picTree_Resize(object? sender, EventArgs e)
    {
        picTree.Invalidate();
    }

    // The tests and measurements take a while, so they run on another thread to avoid freezing the window.
    private async void btnTests_Click(object? sender, EventArgs e)
    {
        btnTests.Enabled = false;
        txtTests.Text = "Ejecutando pruebas y mediciones...";

        try
        {
            txtTests.Text = await Task.Run(BuildTestReport);
        }
        catch (Exception ex)
        {
            txtTests.Text = "Error inesperado durante las pruebas: " + ex.Message;
        }
        finally
        {
            btnTests.Enabled = true;
        }
    }

    private static string BuildTestReport()
    {
        List<TestResult> cases = TestBench.RunCases();
        var lines = new List<string>();

        foreach (TestResult testCase in cases)
            lines.Add($"[{(testCase.Passed ? "OK" : "FALLA")}] {testCase.Case}: {testCase.Detail}");

        lines.Add(string.Empty);
        lines.Add($"{cases.Count(c => c.Passed)} de {cases.Count} casos aprobados.");
        lines.Add(string.Empty);
        lines.Add(TestBench.RunMeasurements());

#if DEBUG
        lines.Add("Aviso: el programa está compilado en Debug; los tiempos no son representativos. Usa Release.");
#endif
        return string.Join(Environment.NewLine, lines);
    }

    private void LoadKeys(IEnumerable<int> keys, string message)
    {
        _tree.Clear();
        int skipped = 0;
        foreach (int key in keys)
        {
            if (!_tree.Insert(key).Success)
                skipped++;
        }

        _highlight = Highlight.None;
        ShowStatus(skipped == 0 ? message : $"{message} Se omitieron {skipped} claves duplicadas.");
        UpdateView();
    }

    private bool TryReadKey(out int key)
    {
        try
        {
            key = KeyInput.ReadKey(txtKey.Text);
            return true;
        }
        catch (ArgumentException ex)
        {
            key = 0;
            ShowStatus(ex.Message);
            return false;
        }
    }

    private void UpdateView()
    {
        string height = _tree.IsEmpty ? "—" : _tree.Height().ToString();
        lblMetrics.Text = $"Nodos: {_tree.Count}   |   Altura: {height}";
        txtTraversals.Text =
            $"Preorden:  {FormatKeys(_tree.Preorder())}{Environment.NewLine}" +
            $"Inorden:   {FormatKeys(_tree.Inorder())}{Environment.NewLine}" +
            $"Postorden: {FormatKeys(_tree.Postorder())}";
        picTree.Invalidate();
    }

    private static string FormatKeys(List<int> keys)
    {
        if (keys.Count == 0)
            return "(vacío)";
        if (keys.Count <= MaxKeysInTraversal)
            return string.Join(" ", keys);
        return string.Join(" ", keys.Take(MaxKeysInTraversal)) + $" … ({keys.Count} claves)";
    }

    private void ShowStatus(string message)
    {
        lblStatus.Text = message;
    }
}