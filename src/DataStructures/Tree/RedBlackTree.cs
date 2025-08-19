using System.Collections;
using System.Transactions;

namespace DataStructures.Tree;

public class RedBlackTree<T> : TreeBase<T> where T : IComparable
{
    // 叶子节点
    private readonly RedBlackTreeNode<T> _nil;
    // 根节点
    private RedBlackTreeNode<T> _root;
    public new RedBlackTreeNode<T> Root => _root;

    public RedBlackTree()
    {
        _nil = new RedBlackTreeNode<T>();
        _nil.Left = _nil.Right = _nil.Parent = _nil;
        _root = _nil;
    }

    public bool IsEmptyTree => Root == _nil;

    /// <summary>
    /// 模拟左旋操作，提升 pivot.Right 位置
    /// </summary>
    /// <param name="pivot">需要左旋的节点位置 (支点)</param>
    private void LeftRotate(RedBlackTreeNode<T> pivot)
    {
        var rightChild = pivot.Right;
        pivot.Right = rightChild.Left;
        // 先修改父节点, 如果都是 nil 就没有修改指针的必要
        if (rightChild.Left != _nil)
            rightChild.Left.Parent = pivot;
        rightChild.Parent = pivot.Parent;

        // 假设对于根节点操作, 需要把根设置成 y
        if (pivot.Parent == _nil)
            _root = rightChild;
        // x 是左孩子
        else if (pivot == pivot.Parent.Left)
            pivot.Parent.Left = rightChild;
        // x 是右孩子
        else
            pivot.Parent.Right = rightChild;

        rightChild.Left = pivot;
        pivot.Parent = rightChild;
    }

    /// <summary>
    /// 右旋, 提升 pivot.Left 位置
    /// </summary>
    /// <param name="pivot">支点位置</param>
    private void RightRotate(RedBlackTreeNode<T> pivot)
    {
        var leftChild = pivot.Left;
        pivot.Left = leftChild.Right;
        if (leftChild.Right != _nil)
            leftChild.Right.Parent = pivot;

        leftChild.Parent = pivot.Parent;

        if (pivot.Parent == _nil)
            _root = leftChild;
        else if (pivot == pivot.Parent.Right)
            pivot.Parent.Right = leftChild;
        else
            pivot.Parent.Left = leftChild;

        leftChild.Right = pivot;
        pivot.Parent = leftChild;
    }

    public override void Insert(T val)
    {
        // 先进行普通BST的插入
        var newNode = new RedBlackTreeNode<T>(val);
        var parentNode = _nil;
        var currentNode = _root;

        while (currentNode != _nil)
        {
            parentNode = currentNode;
            currentNode = val.CompareTo(currentNode.Value) < 0 ? currentNode.Left : currentNode.Right;
        }

        newNode.Parent = parentNode;

        // 说明 _root 也是空的
        if (parentNode == _nil)
            _root = newNode;
        else if (val.CompareTo(parentNode.Value) < 0)
            parentNode.Left = newNode;
        else
            parentNode.Right = newNode;

        // 维护红黑树性质, 从 newNode 开始修复
        // newNode 已经插入了树中, 所以虽然是引用传递, 但是不会影响新插入的节点信息本身
        RepairAfterInsert(newNode);
    }

    /// <summary>
    /// 维护插入后的红黑树性质
    /// </summary>
    /// <param name="node">新插入的节点位置</param>
    private void RepairAfterInsert(RedBlackTreeNode<T> node)
    {
        // 自低往上修复
        while (node.Parent.Color == Colors.Red)
        {
            var parent = node.Parent;
            var grandPa = parent.Parent;

            if (parent == grandPa.Left)
            {
                var uncle = grandPa.Right;

                if (uncle.Color == Colors.Red)
                {
                    parent.Color = Colors.Black;
                    uncle.Color = Colors.Black;
                    grandPa.Color = Colors.Red;
                    node = grandPa;
                }
                else
                {
                    if (node == parent.Right)
                    {
                        node = parent;
                        LeftRotate(node);
                        parent = node.Parent;
                        grandPa = parent.Parent;
                    }
                    parent.Color = Colors.Black;
                    grandPa.Color = Colors.Red;
                    RightRotate(grandPa);
                }
            }
            // 镜像操作
            else
            {
                var uncle = parent.Left;

                if (uncle.Color == Colors.Red)
                {
                    parent.Color = Colors.Black;
                    uncle.Color = Colors.Black;
                    grandPa.Color = Colors.Red;
                    node = grandPa;
                }
                else
                {
                    if (node == parent.Left)
                    {
                        node = parent;
                        RightRotate(node);
                        parent = node.Parent;
                        grandPa = parent.Parent;
                    }
                    parent.Color = Colors.Black;
                    grandPa.Color = Colors.Red;
                    LeftRotate(grandPa);
                }
            }
        }

        // 根节点黑色
        _root.Color = Colors.Black;
    }

    /// <summary>
    /// 根据数值查找是否存在对应节点
    /// </summary>
    /// <param name="val"></param>
    /// <returns></returns>
    public RedBlackTreeNode<T> FindNode(T val)
    {
        var currentNode = _root;

        while (currentNode != _nil)
        {
            int cmp = val.CompareTo(currentNode.Value);
            if (cmp == 0)
                return currentNode;
            currentNode = cmp < 0 ? currentNode.Left : currentNode.Right;
        }

        return _nil;
    }

    public override bool Search(T val) => FindNode(val) != _nil;

    /// <summary>
    /// 插入数值
    /// </summary>
    /// <param name="val">需要插入的数值</param>
    /// <returns>返回是否插入成功</returns>
    public bool Delete(T val)
    {
        var nodeToDelete = FindNode(val);

        if (nodeToDelete == _nil)
            return false;

        var nodeMoved = nodeToDelete;
        var originColor = nodeMoved.Color;
        RedBlackTreeNode<T> fixNode;

        if (nodeToDelete.Left == _nil)
        {
            fixNode = nodeToDelete.Right;
            ReplacePlant(nodeToDelete, nodeToDelete.Right);
        }
        else if (nodeToDelete.Right == _nil)
        {
            fixNode = nodeToDelete.Left;
            ReplacePlant(nodeToDelete, nodeToDelete.Left);
        }
        else
        {
            // 左右节点都非空, 寻找后继点
            nodeMoved = MininumNode(nodeToDelete.Right);
            originColor = nodeMoved.Color;
            fixNode = nodeMoved.Right;

            if (nodeMoved.Parent == nodeToDelete)
            {
                fixNode.Parent = nodeMoved;
            }
            else
            {
                ReplacePlant(nodeMoved, nodeMoved.Right);
                nodeMoved.Right = nodeToDelete.Right;
                nodeMoved.Right.Parent = nodeMoved;
            }

            ReplacePlant(nodeMoved, nodeMoved.Right);
            nodeMoved.Left = nodeToDelete.Left;
            nodeMoved.Left.Parent = nodeMoved;
            nodeMoved.Color = nodeToDelete.Color;
        }

        if (originColor == Colors.Black)
            RepairAfterDelete(fixNode);

        return true;
    }

    private void RepairAfterDelete(RedBlackTreeNode<T> node)
    {
        while (node != _root && node.Color == Colors.Black)
        {
            if (node == node.Parent.Left)
            {
                var sibling = node.Parent.Right;

                if (sibling.Color == Colors.Red)
                {
                    sibling.Color = Colors.Black;
                    node.Parent.Color = Colors.Red;
                    LeftRotate(node.Parent);
                    sibling = node.Parent.Right;
                }

                if (sibling.Left.Color == Colors.Black && sibling.Right.Color == Colors.Black)
                {
                    sibling.Color = Colors.Red;
                    node = node.Parent;
                }
                else
                {
                    if (sibling.Right.Color == Colors.Black)
                    {
                        sibling.Left.Color = Colors.Black;
                        sibling.Color = Colors.Red;
                        RightRotate(sibling);
                        sibling = node.Parent.Right;
                    }
                    sibling.Color = node.Parent.Color;
                    node.Parent.Color = Colors.Black;
                    sibling.Right.Color = Colors.Black;
                    LeftRotate(node.Parent);
                    node = _root;
                }
            }
            else
            {
                var sibling = node.Parent.Left;

                if (sibling.Color == Colors.Red)
                {
                    sibling.Color = Colors.Black;
                    node.Parent.Color = Colors.Red;
                    RightRotate(node.Parent);
                    sibling = node.Parent.Left;
                }

                if (sibling.Left.Color == Colors.Black && sibling.Right.Color == Colors.Black)
                {
                    sibling.Color = Colors.Red;
                    node = node.Parent;
                }
                else
                {
                    if (sibling.Left.Color == Colors.Black)
                    {
                        sibling.Right.Color = Colors.Black;
                        sibling.Color = Colors.Red;
                        LeftRotate(sibling);
                        sibling = node.Parent.Left;
                    }
                    sibling.Color = node.Parent.Color;
                    node.Parent.Color = Colors.Black;
                    sibling.Left.Color = Colors.Black;
                    RightRotate(node.Parent);
                    node = _root;
                }
            }
        }
    }
    /// <summary>
    /// 使用 v 替换 u 的位置
    /// </summary>
    /// <param name="u"></param>
    /// <param name="v"></param>
    private void ReplacePlant(RedBlackTreeNode<T> u, RedBlackTreeNode<T> v)
    {
        if (u.Parent == _nil)
            _root = v;
        else if (u.Parent == u.Parent.Left)
            u.Parent.Left = v;
        else
            u.Parent.Right = v;
    }

    /// <summary>
    /// 寻找以 start 为根的最小节点
    /// </summary>
    /// <param name="start">根节点</param>
    /// <returns>返回寻找到的节点</returns>
    private RedBlackTreeNode<T> MininumNode(RedBlackTreeNode<T> start)
    {
        var currentNode = start;

        // 因为左边节点的数值小于父节点, 所以一直往左边走
        while (currentNode != _nil)
            currentNode = currentNode.Left;

        return currentNode;
    }

    public override IEnumerable<T> Traverse() => InOrderRec(_root);
    // IEnumerator IEnumerable<T>.GetEnumerator() => Traverse().GetEnumerator();

    private IEnumerable<T> InOrderRec(RedBlackTreeNode<T> node)
    {
        if (node == _nil)
            yield break;
        foreach (var childVal in InOrderRec(node.Left))
            yield return childVal;
        yield return node.Value;
        foreach (var childVal in InOrderRec(node.Right))
            yield return childVal;
    }
}