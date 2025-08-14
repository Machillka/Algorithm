namespace DataStructures.Tree;

/// <summary>
/// 树的节点
/// </summary>
public interface ITreeNode<T>
{
    T Value { get; set; }
    IReadOnlyCollection<ITreeNode<T>> Children { get; }
}

/// <summary>
/// 通用操作的接口
/// </summary>
public interface ITree<T>
{
    ITreeNode<T> Root { get; }          // 定义根节点
    void Insert(T val);                 // 插入方法
    bool Search(T val);                 // 查找方法
    IEnumerable<T> Traverse();          // 遍历方法
}

/// <summary>
/// 提供遍历的默认实现
/// </summary>
public abstract class TreeBase<T> : ITree<T>
{
    public ITreeNode<T> Root { get; protected set; }

    public abstract void Insert(T val);
    public abstract bool Search(T val);

    // 提供遍历的默认方法, 前序遍历
    public virtual IEnumerable<T> Traverse()
    {
        if (Root == null)
            yield break;
        var stack = new Stack<ITreeNode<T>>();
        stack.Push(Root);

        while (stack.Count > 0)
        {
            var node = stack.Pop();
            yield return node.Value;
            foreach (var child in node.Children.Reverse())
                stack.Push(child);
        }
    }
}