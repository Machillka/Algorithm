namespace DataStructures.Tree;

public class BinarySearchTree<T> : TreeBase<T> where T : IComparable<T>
{
    public new BinaryTreeNode<T> Root
    {
        get => (BinaryTreeNode<T>)base.Root;
        private set => base.Root = value;
    }

    public override void Insert(T value)
        => Root = InsertRec(Root, value);

    private BinaryTreeNode<T> InsertRec(BinaryTreeNode<T> node, T value)
    {
        if (node == null) return new BinaryTreeNode<T>(value);
        int cmp = value.CompareTo(node.Value);
        if (cmp < 0) node.Left  = InsertRec(node.Left,  value);
        else if (cmp > 0) node.Right = InsertRec(node.Right, value);
        return node;
    }

    public override bool Search(T value)
        => SearchRec(Root, value);

    private bool SearchRec(BinaryTreeNode<T> node, T value)
    {
        if (node == null) return false;
        int cmp = value.CompareTo(node.Value);
        if (cmp == 0) return true;
        return cmp < 0
            ? SearchRec(node.Left,  value)
            : SearchRec(node.Right, value);
    }
}