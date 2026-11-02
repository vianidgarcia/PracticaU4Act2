namespace PracticaU4Act2;

// Each node is created with new at run time (dynamic memory, on the heap)
// and linked to its children through references.
public sealed class TreeNode
{
    public TreeNode(int key)
    {
        Key = key;
    }

    // The setters are internal: only the tree modifies keys and links; the interface only reads them.
    public int Key { get; internal set; }
    public TreeNode? Left { get; internal set; }
    public TreeNode? Right { get; internal set; }

    public bool IsLeaf => Left is null && Right is null;
}
