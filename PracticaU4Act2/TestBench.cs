using System.Diagnostics;
using System.Globalization;
using System.Text;

namespace PracticaU4Act2;

public sealed record TestResult(string Case, bool Passed, string Detail);

// Test cases for the integrity of the traversals and operations, and measurements.
// It uses no controls: it returns data and text for the form to display.
public static class TestBench
{
    // Sample tree from the report; it is also used by the "Ejemplo" button.
    public static readonly int[] SampleKeys = { 50, 30, 70, 20, 40, 60, 80, 35, 45, 65 };

    public static List<TestResult> RunCases()
    {
        var cases = new List<TestResult>();

        // Expected values computed by hand and with an independent model.
        BinarySearchTree sample = CreateSample();
        cases.Add(Compare("Preorden del árbol de ejemplo", sample.Preorder(), 50, 30, 20, 40, 35, 45, 70, 60, 65, 80));
        cases.Add(Compare("Inorden del árbol de ejemplo", sample.Inorder(), 20, 30, 35, 40, 45, 50, 60, 65, 70, 80));
        cases.Add(Compare("Postorden del árbol de ejemplo", sample.Postorder(), 20, 35, 45, 40, 30, 65, 60, 80, 70, 50));
        cases.Add(new TestResult("Altura y cantidad del ejemplo",
            sample.Height() == 3 && sample.Count == 10, $"altura {sample.Height()} (camino 50-30-40-35), {sample.Count} nodos"));

        cases.Add(TestSearch(sample, 45, true, "50 → 30 → 40 → 45"));
        cases.Add(TestSearch(sample, 66, false, "50 → 70 → 60 → 65"));

        cases.Add(TestDeletion(20, "Caso 1 (hoja)", 50, 30, 40, 35, 45, 70, 60, 65, 80));
        cases.Add(TestDeletion(60, "Caso 2 (un hijo)", 50, 30, 20, 40, 35, 45, 70, 65, 80));
        cases.Add(TestDeletion(30, "Caso 3 (dos hijos)", 50, 35, 20, 40, 45, 70, 60, 65, 80));
        cases.Add(TestDeletion(50, "Caso 3 (dos hijos)", 60, 30, 20, 40, 35, 45, 70, 65, 80));

        cases.Add(TestEmptyTree());
        cases.Add(TestSingleNode());
        cases.Add(TestDuplicate());
        cases.Add(TestDeleteNonexistent());
        cases.Add(TestInput());
        cases.Add(TestDegenerate());
        cases.Add(TestReconstruction());
        cases.Add(TestRandomAgainstReference());
        return cases;
    }

    public static BinarySearchTree CreateSample()
    {
        var tree = new BinarySearchTree();
        foreach (int key in SampleKeys)
            tree.Insert(key);
        return tree;
    }

    private static TestResult Compare(string testCase, List<int> actual, params int[] expected) =>
        new(testCase, actual.SequenceEqual(expected), string.Join(" ", actual));

    private static TestResult TestSearch(BinarySearchTree tree, int key, bool shouldFind, string expectedPath)
    {
        SearchResult r = tree.Search(key);
        string path = string.Join(" → ", r.Path);
        return new TestResult($"Buscar {key} ({(shouldFind ? "existe" : "no existe")})",
            r.Found == shouldFind && path == expectedPath, $"camino {path}, {r.Path.Count} comparaciones");
    }

    // After deleting, the applied case, the expected preorder, the sorted inorder and the count are checked.
    private static TestResult TestDeletion(int key, string expectedCase, params int[] expectedPreorder)
    {
        BinarySearchTree tree = CreateSample();
        OperationResult r = tree.Delete(key);
        List<int> preorder = tree.Preorder();
        bool ok = r.Success && r.Message.StartsWith(expectedCase) && preorder.SequenceEqual(expectedPreorder)
                  && tree.IsValid() && tree.Count == 9 && !tree.Contains(key);
        return new TestResult($"Eliminar {key}: {expectedCase}", ok, $"preorden {string.Join(" ", preorder)}");
    }

    private static TestResult TestEmptyTree()
    {
        var t = new BinarySearchTree();
        bool ok = t.Preorder().Count == 0 && t.Inorder().Count == 0 && t.Postorder().Count == 0
                  && t.Height() == -1 && !t.Search(5).Found && !t.Delete(5).Success;
        return new TestResult("Árbol vacío", ok, "recorridos vacíos, altura -1, buscar y eliminar fallan sin error");
    }

    private static TestResult TestSingleNode()
    {
        var t = new BinarySearchTree();
        t.Insert(7);
        bool traversals = t.Preorder().SequenceEqual(new[] { 7 }) && t.Inorder().SequenceEqual(new[] { 7 })
                          && t.Postorder().SequenceEqual(new[] { 7 }) && t.Height() == 0;
        bool deleted = t.Delete(7).Success && t.IsEmpty && t.Count == 0;
        return new TestResult("Un solo nodo (insertar y eliminar la raíz)", traversals && deleted,
            "los tres recorridos dan 7; al eliminarlo el árbol queda vacío");
    }

    private static TestResult TestDuplicate()
    {
        BinarySearchTree t = CreateSample();
        OperationResult r = t.Insert(40);
        return new TestResult("Clave duplicada", !r.Success && t.Count == 10 && r.Message == ErrorMessages.DuplicateKey(40),
            $"mensaje: \"{r.Message}\"");
    }

    private static TestResult TestDeleteNonexistent()
    {
        BinarySearchTree t = CreateSample();
        OperationResult r = t.Delete(99);
        return new TestResult("Eliminar clave inexistente", !r.Success && t.Count == 10 && t.IsValid(),
            $"mensaje: \"{r.Message}\"");
    }

    private static TestResult TestInput()
    {
        bool sequence = KeyInput.ReadSequence(" 10, -5;20  7 ").SequenceEqual(new[] { 10, -5, 20, 7 });
        bool empty = ThrowsWithMessage(() => KeyInput.ReadSequence("  "), ErrorMessages.EmptySequence);
        bool letter = ThrowsWithMessage(() => KeyInput.ReadSequence("10, abc"), ErrorMessages.InvalidKey("abc"));
        bool emptyKey = ThrowsWithMessage(() => KeyInput.ReadKey(""), ErrorMessages.EmptyKey);
        return new TestResult("Validación de la entrada", sequence && empty && letter && emptyKey,
            "separadores mixtos, negativos, texto vacío y texto no numérico");
    }

    // With sorted keys the tree degenerates into a list; the iterative operations do not overflow the stack.
    private static TestResult TestDegenerate()
    {
        const int n = 10000;
        var t = new BinarySearchTree();
        for (int i = 1; i <= n; i++)
            t.Insert(i);

        bool ok = t.Height() == n - 1 && t.Inorder().SequenceEqual(Enumerable.Range(1, n))
                  && t.Preorder().SequenceEqual(Enumerable.Range(1, n))
                  && t.Postorder().SequenceEqual(Enumerable.Range(1, n).Reverse())
                  && t.Delete(1).Success && t.Delete(n).Success && t.IsValid();
        return new TestResult("Árbol degenerado de 10 000 nodos", ok, "altura 9 999; recorridos y eliminación sin desbordar la pila");
    }

    // Inserting the keys in preorder order rebuilds exactly the same tree.
    private static TestResult TestReconstruction()
    {
        BinarySearchTree original = RandomTree(new Random(3), 200, out _);
        var copy = new BinarySearchTree();
        foreach (int key in original.Preorder())
            copy.Insert(key);

        bool ok = copy.Preorder().SequenceEqual(original.Preorder())
                  && copy.Inorder().SequenceEqual(original.Inorder())
                  && copy.Postorder().SequenceEqual(original.Postorder());
        return new TestResult("Reconstrucción desde el preorden", ok, "200 nodos: los tres recorridos de la copia coinciden");
    }

    // 300 trees with random insertions and deletions against SortedSet and against recursive traversals.
    private static TestResult TestRandomAgainstReference()
    {
        var random = new Random(1);
        for (int test = 0; test < 300; test++)
        {
            BinarySearchTree t = RandomTree(random, random.Next(0, 80), out SortedSet<int> reference);

            bool ok = t.Inorder().SequenceEqual(reference)
                      && t.Preorder().SequenceEqual(RecursivePreorder(t.Root))
                      && t.Postorder().SequenceEqual(RecursivePostorder(t.Root))
                      && t.Count == reference.Count && t.IsValid();
            if (!ok)
                return new TestResult("300 árboles aleatorios", false, $"falla en la prueba {test + 1}");
        }
        return new TestResult("300 árboles aleatorios", true,
            "inorden = SortedSet; preorden y postorden = versiones recursivas");
    }

    // Inserts and deletes random keys in the tree and in a reference SortedSet.
    private static BinarySearchTree RandomTree(Random random, int operations, out SortedSet<int> reference)
    {
        var t = new BinarySearchTree();
        reference = new SortedSet<int>();
        for (int i = 0; i < operations; i++)
        {
            int key = random.Next(-500, 500);
            if (t.Insert(key).Success != reference.Add(key))
                throw new InvalidOperationException("Insertar no coincide con la referencia.");

            if (random.NextDouble() < 0.3 && reference.Count > 0)
            {
                int toDelete = reference.ElementAt(random.Next(reference.Count));
                t.Delete(toDelete);
                reference.Remove(toDelete);
            }
        }
        return t;
    }

    private static List<int> RecursivePreorder(TreeNode? node)
    {
        if (node is null) return new List<int>();
        var r = new List<int> { node.Key };
        r.AddRange(RecursivePreorder(node.Left));
        r.AddRange(RecursivePreorder(node.Right));
        return r;
    }

    private static List<int> RecursivePostorder(TreeNode? node)
    {
        if (node is null) return new List<int>();
        var r = RecursivePostorder(node.Left);
        r.AddRange(RecursivePostorder(node.Right));
        r.Add(node.Key);
        return r;
    }

    private static bool ThrowsWithMessage(Action action, string message)
    {
        try
        {
            action();
            return false;
        }
        catch (ArgumentException ex)
        {
            return ex.Message == message;
        }
    }

    // Compares a tree with keys in random order against one with sorted keys (degenerate).
    // Each time is the median of 5 runs, after one warm-up run.
    public static string RunMeasurements()
    {
        int[] sizes = { 1000, 5000, 10000, 20000 };
        const int searches = 1000;
        var sb = new StringBuilder();

        sb.AppendLine("Altura en aristas y construcción (insertar las n claves)");
        sb.AppendLine($"{"n",8}{"log2(n)",10}{"Altura aleat.",15}{"Altura orden.",15}{"Construir aleat. (ms)",23}{"Construir orden. (ms)",23}");
        var searchRows = new List<string>();

        foreach (int n in sizes)
        {
            int[] randomKeys = Shuffle(Enumerable.Range(1, n).ToArray(), new Random(42));
            int[] sortedKeys = Enumerable.Range(1, n).ToArray();

            BinarySearchTree randomTree = Build(randomKeys);
            BinarySearchTree sortedTree = Build(sortedKeys);

            double msBuildRandom = MedianMs(() => Build(randomKeys));
            double msBuildSorted = MedianMs(() => Build(sortedKeys));

            var random = new Random(7);
            int[] targets = Enumerable.Range(0, searches).Select(_ => random.Next(1, n + 1)).ToArray();
            double msSearchRandom = MedianMs(() => SearchAll(randomTree, targets));
            double msSearchSorted = MedianMs(() => SearchAll(sortedTree, targets));

            string log2 = Math.Log2(n).ToString("F1", CultureInfo.InvariantCulture);
            sb.AppendLine($"{n,8}{log2,10}{randomTree.Height(),15}{sortedTree.Height(),15}{Ms(msBuildRandom),23}{Ms(msBuildSorted),23}");
            searchRows.Add($"{n,8}{Ms(msSearchRandom),20}{Ms(msSearchSorted),20}{Factor(msSearchSorted, msSearchRandom),12}");
        }

        sb.AppendLine();
        sb.AppendLine($"{searches} búsquedas de claves existentes");
        sb.AppendLine($"{"n",8}{"Aleatorio (ms)",20}{"Ordenado (ms)",20}{"Ordenado / aleatorio",22}");
        foreach (string row in searchRows)
            sb.AppendLine(row);
        return sb.ToString();
    }

    private static BinarySearchTree Build(int[] keys)
    {
        var tree = new BinarySearchTree();
        foreach (int key in keys)
            tree.Insert(key);
        return tree;
    }

    private static int SearchAll(BinarySearchTree tree, int[] keys)
    {
        int found = 0;
        foreach (int key in keys)
            if (tree.Contains(key)) found++;
        return found;
    }

    // Fisher-Yates: uniform random permutation of the keys.
    private static int[] Shuffle(int[] keys, Random random)
    {
        for (int i = keys.Length - 1; i > 0; i--)
        {
            int j = random.Next(i + 1);
            (keys[i], keys[j]) = (keys[j], keys[i]);
        }
        return keys;
    }

    private static double MedianMs(Action action)
    {
        action();   // JIT warm-up
        var times = new double[5];
        for (int k = 0; k < times.Length; k++)
        {
            var clock = Stopwatch.StartNew();
            action();
            clock.Stop();
            times[k] = clock.Elapsed.TotalMilliseconds;
        }
        Array.Sort(times);
        return times[2];
    }

    private static string Ms(double value) => value.ToString("F2", CultureInfo.InvariantCulture);

    private static string Factor(double a, double b) =>
        b > 0 ? (a / b).ToString("F0", CultureInfo.InvariantCulture) + "x" : "-";
}
