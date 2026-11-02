namespace PracticaU4Act2;

public sealed record OperationResult(bool Success, string Message);

// Path: keys of the visited nodes, from the root; its length is the number of comparisons.
public sealed record SearchResult(bool Found, IReadOnlyList<int> Path, string Message);

// Binary search tree: to the left of each node, smaller keys; to the right, larger ones.
// All operations are iterative so a degenerate tree (height n) does not overflow the stack.
public sealed class BinarySearchTree
{
    private TreeNode? _root;
    private int _count;

    public TreeNode? Root => _root;
    public int Count => _count;
    public bool IsEmpty => _root is null;

    // The node is created only when it is linked, to avoid allocating memory if the key is repeated.
    public OperationResult Insert(int key)
    {
        if (_root is null)
        {
            _root = new TreeNode(key);
            _count++;
            return new OperationResult(true, $"Árbol vacío: {key} se inserta como raíz.");
        }

        TreeNode current = _root;
        while (true)
        {
            if (key < current.Key)
            {
                if (current.Left is null)
                {
                    current.Left = new TreeNode(key);
                    _count++;
                    return new OperationResult(true, $"{key} < {current.Key}: se inserta como hijo izquierdo de {current.Key}.");
                }
                current = current.Left;
            }
            else if (key > current.Key)
            {
                if (current.Right is null)
                {
                    current.Right = new TreeNode(key);
                    _count++;
                    return new OperationResult(true, $"{key} > {current.Key}: se inserta como hijo derecho de {current.Key}.");
                }
                current = current.Right;
            }
            else
            {
                return new OperationResult(false, ErrorMessages.DuplicateKey(key));
            }
        }
    }

    public SearchResult Search(int key)
    {
        var path = new List<int>();
        TreeNode? current = _root;

        while (current is not null)
        {
            path.Add(current.Key);
            if (key == current.Key)
                return new SearchResult(true, path,
                    $"{key} encontrado en {path.Count} comparaciones: {string.Join(" → ", path)}.");

            current = key < current.Key ? current.Left : current.Right;
        }

        string message = _root is null
            ? ErrorMessages.EmptyTree
            : $"{ErrorMessages.KeyNotFound(key)} Se revisaron {path.Count} nodos: {string.Join(" → ", path)}.";
        return new SearchResult(false, path, message);
    }

    // Version that does not record the path, for the measurements.
    public bool Contains(int key)
    {
        TreeNode? current = _root;
        while (current is not null)
        {
            if (key == current.Key)
                return true;
            current = key < current.Key ? current.Left : current.Right;
        }
        return false;
    }

    public OperationResult Delete(int key)
    {
        if (_root is null)
            return new OperationResult(false, ErrorMessages.EmptyTree);

        TreeNode? parent = null;
        TreeNode? current = _root;
        while (current is not null && current.Key != key)
        {
            parent = current;
            current = key < current.Key ? current.Left : current.Right;
        }
        if (current is null)
            return new OperationResult(false, ErrorMessages.KeyNotFound(key));

        string message;
        if (current.Left is not null && current.Right is not null)
        {
            // Case 3: the key of the inorder successor (the minimum of the right subtree) is copied
            // and the successor is deleted, which has no left child.
            TreeNode successorParent = current;
            TreeNode successor = current.Right;
            while (successor.Left is not null)
            {
                successorParent = successor;
                successor = successor.Left;
            }

            message = $"Caso 3 (dos hijos): {key} se sustituye por su sucesor inorden {successor.Key}.";
            current.Key = successor.Key;
            Replace(successorParent, successor, successor.Right);
        }
        else
        {
            TreeNode? child = current.Left ?? current.Right;
            message = child is null
                ? $"Caso 1 (hoja): {key} se elimina directamente."
                : $"Caso 2 (un hijo): {key} se sustituye por su hijo {child.Key}.";
            Replace(parent, current, child);
        }

        _count--;
        return new OperationResult(true, message);
    }

    public void Clear()
    {
        _root = null;   // nodes without references are freed by the garbage collector
        _count = 0;
    }

    // Root, left, right. The right child is pushed first so the left one is visited first.
    public List<int> Preorder()
    {
        var result = new List<int>(_count);
        if (_root is null)
            return result;

        var stack = new Stack<TreeNode>();
        stack.Push(_root);
        while (stack.Count > 0)
        {
            TreeNode node = stack.Pop();
            result.Add(node.Key);
            if (node.Right is not null) stack.Push(node.Right);
            if (node.Left is not null) stack.Push(node.Left);
        }
        return result;
    }

    // Left, root, right. It goes down the left side pushing and, when popping, visits and moves to the right.
    public List<int> Inorder()
    {
        var result = new List<int>(_count);
        var stack = new Stack<TreeNode>();
        TreeNode? current = _root;

        while (current is not null || stack.Count > 0)
        {
            while (current is not null)
            {
                stack.Push(current);
                current = current.Left;
            }
            current = stack.Pop();
            result.Add(current.Key);
            current = current.Right;
        }
        return result;
    }

    // Left, right, root. It traverses root-right-left and reverses the result.
    public List<int> Postorder()
    {
        var result = new List<int>(_count);
        if (_root is null)
            return result;

        var stack = new Stack<TreeNode>();
        stack.Push(_root);
        while (stack.Count > 0)
        {
            TreeNode node = stack.Pop();
            result.Add(node.Key);
            if (node.Left is not null) stack.Push(node.Left);
            if (node.Right is not null) stack.Push(node.Right);
        }
        result.Reverse();
        return result;
    }

    // Height = edges on the longest path from the root to a leaf: only the root = 0; empty tree = -1 by convention.
    // It is traversed level by level with a queue; the height is the number of levels minus one.
    public int Height()
    {
        if (_root is null)
            return -1;

        var queue = new Queue<TreeNode>();
        queue.Enqueue(_root);
        int levels = 0;

        while (queue.Count > 0)
        {
            levels++;
            for (int i = queue.Count; i > 0; i--)
            {
                TreeNode node = queue.Dequeue();
                if (node.Left is not null) queue.Enqueue(node.Left);
                if (node.Right is not null) queue.Enqueue(node.Right);
            }
        }
        return levels - 1;
    }

    // A BST is valid if its inorder traversal comes out strictly increasing.
    public bool IsValid()
    {
        List<int> inorder = Inorder();
        for (int i = 1; i < inorder.Count; i++)
        {
            if (inorder[i - 1] >= inorder[i])
                return false;
        }
        return inorder.Count == _count;
    }

    // Makes the parent point to 'replacement' instead of 'old'; with no parent, the root changes.
    private void Replace(TreeNode? parent, TreeNode old, TreeNode? replacement)
    {
        if (parent is null)
            _root = replacement;
        else if (parent.Left == old)
            parent.Left = replacement;
        else
            parent.Right = replacement;
    }
}
