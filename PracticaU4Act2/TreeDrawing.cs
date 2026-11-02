using System.Drawing.Drawing2D;

namespace PracticaU4Act2;

// What to highlight when drawing: the path of the last search and one featured node,
// green if it was found or inserted, red if it is where an unsuccessful search stopped.
public sealed record Highlight(IReadOnlyCollection<int> Path, int? Key, bool KeyFound)
{
    public static readonly Highlight None = new(Array.Empty<int>(), null, false);
}

// Draws the tree on any Graphics. The horizontal position of each node is its place
// in the inorder traversal and the vertical one is its depth, so nodes never overlap.
public static class TreeDrawing
{
    // With more nodes, the horizontal space per node is smaller than the smallest circle and they overlap.
    public const int MaxDrawableNodes = 60;

    private const int Margin = 30;
    private const float MaxRadius = 18F;
    private const float MinRadius = 7F;

    private static readonly Color PathColor = Color.FromArgb(255, 236, 150);
    private static readonly Color FoundColor = Color.FromArgb(120, 200, 120);
    private static readonly Color NotFoundColor = Color.FromArgb(240, 140, 140);

    public static Dictionary<TreeNode, PointF> CalculatePositions(TreeNode? root, Size area)
    {
        var list = TraverseWithDepth(root);
        var positions = new Dictionary<TreeNode, PointF>(list.Count);
        if (list.Count == 0)
            return positions;

        int levels = list.Max(x => x.Depth) + 1;
        float stepX = (area.Width - 2f * Margin) / list.Count;
        float stepY = levels > 1 ? Math.Min(70f, (area.Height - 2f * Margin) / (levels - 1)) : 0f;

        for (int i = 0; i < list.Count; i++)
        {
            float x = Margin + stepX * (i + 0.5f);
            float y = Margin + stepY * list[i].Depth;
            positions[list[i].Node] = new PointF(x, y);
        }
        return positions;
    }

    public static float CalculateRadius(int count, Size area)
    {
        if (count == 0)
            return MaxRadius;
        float byWidth = (area.Width - 2f * Margin) / count / 2.3f;
        return Math.Clamp(byWidth, MinRadius, MaxRadius);
    }

    public static void Draw(Graphics g, Size area, BinarySearchTree tree,
                            Dictionary<TreeNode, PointF> positions, Highlight highlight)
    {
        g.SmoothingMode = SmoothingMode.AntiAlias;
        g.Clear(Color.White);

        using var noticeFont = new Font("Segoe UI", 10F);
        if (tree.IsEmpty)
        {
            g.DrawString("El árbol está vacío. Inserta una clave o carga una secuencia.", noticeFont, Brushes.Black, Margin, Margin);
            return;
        }
        if (tree.Count > MaxDrawableNodes)
        {
            g.DrawString($"El árbol tiene {tree.Count} nodos; solo se dibujan árboles de hasta {MaxDrawableNodes}.",
                         noticeFont, Brushes.Black, Margin, Margin);
            return;
        }

        float radius = CalculateRadius(tree.Count, area);
        using var font = new Font("Segoe UI", Math.Max(6F, radius * 0.55F), FontStyle.Bold);
        using var edgePen = new Pen(Color.FromArgb(110, 110, 110), 1.5F);
        using var pathPen = new Pen(Color.FromArgb(220, 160, 0), 3F);
        var inPath = new HashSet<int>(highlight.Path);

        // Edges first, so the nodes end up on top.
        foreach (var (node, point) in positions)
        {
            foreach (TreeNode? child in new[] { node.Left, node.Right })
            {
                if (child is null || !positions.TryGetValue(child, out PointF childPoint))
                    continue;
                bool edgeInPath = inPath.Contains(node.Key) && inPath.Contains(child.Key);
                g.DrawLine(edgeInPath ? pathPen : edgePen, point, childPoint);
            }
        }

        foreach (var (node, point) in positions)
        {
            Color fill = Color.White;
            if (highlight.Key == node.Key)
                fill = highlight.KeyFound ? FoundColor : NotFoundColor;
            else if (inPath.Contains(node.Key))
                fill = PathColor;

            var circle = new RectangleF(point.X - radius, point.Y - radius, 2 * radius, 2 * radius);
            using (var brush = new SolidBrush(fill))
                g.FillEllipse(brush, circle);
            g.DrawEllipse(Pens.Black, circle);

            string text = node.Key.ToString();
            SizeF size = g.MeasureString(text, font);
            g.DrawString(text, font, Brushes.Black, point.X - size.Width / 2, point.Y - size.Height / 2);
        }
    }

    // Returns the node whose circle contains the point, or null.
    public static TreeNode? NodeAt(Point point, Dictionary<TreeNode, PointF> positions, float radius)
    {
        foreach (var (node, center) in positions)
        {
            float dx = point.X - center.X;
            float dy = point.Y - center.Y;
            if (dx * dx + dy * dy <= radius * radius)
                return node;
        }
        return null;
    }

    // Iterative inorder that also stores the depth of each node.
    private static List<(TreeNode Node, int Depth)> TraverseWithDepth(TreeNode? root)
    {
        var result = new List<(TreeNode, int)>();
        var stack = new Stack<(TreeNode Node, int Depth)>();
        TreeNode? current = root;
        int depth = 0;

        while (current is not null || stack.Count > 0)
        {
            while (current is not null)
            {
                stack.Push((current, depth));
                current = current.Left;
                depth++;
            }
            var (node, d) = stack.Pop();
            result.Add((node, d));
            current = node.Right;
            depth = d + 1;
        }
        return result;
    }
}
