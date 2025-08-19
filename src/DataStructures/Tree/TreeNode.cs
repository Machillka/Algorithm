namespace DataStructures.Tree;

/// <summary>
/// 定义多叉树通用节点
/// </summary>
public class TreeNode<T> : ITreeNode<T>
{
    public T Value { get; set; }
    private readonly List<ITreeNode<T>> _children = new();
    public IReadOnlyCollection<ITreeNode<T>> Children => _children;

    // 默认构造函数, 可以提供空值
    public TreeNode(T val = default) => Value = val;
    public void AddChild(ITreeNode<T> childNode) => _children.Add(childNode);
}

public class BinaryTreeNode<T> : ITreeNode<T>
{
    public T Value { get; set; }
    public BinaryTreeNode<T> Left { get; set; }
    public BinaryTreeNode<T> Right { get; set; }

    public IReadOnlyCollection<ITreeNode<T>> Children =>
        new[] { Left as ITreeNode<T>, Right as ITreeNode<T> }
        .Where(x => x != null)
        .ToList()
        .AsReadOnly();

    public BinaryTreeNode(T val = default) => Value = val;
}
public enum Colors { Red, Black }
public class RedBlackTreeNode<T> : ITreeNode<T>
{
    public T Value { get; set; }
    public Colors Color;
    public RedBlackTreeNode<T> Left { get; set; }
    public RedBlackTreeNode<T> Right { get; set; }
    public RedBlackTreeNode<T> Parent { get; set; }

    public IReadOnlyCollection<ITreeNode<T>> Children =>
        new[] { Left as ITreeNode<T>, Right as ITreeNode<T> }
        .Where(x => x != null)
        .ToList()
        .AsReadOnly();

    public RedBlackTreeNode(T val = default, Colors col = Colors.Red, RedBlackTreeNode<T> nil = null)
    {
        Value = val;
        Color = col;
        Left = Right = Parent = nil;
    }
}