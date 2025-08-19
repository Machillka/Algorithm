using System.Collections;

namespace DataStructures.LinkedList;

public class SingleLinkedListNode<T>
{
    public T Value { get; set; }
    public SingleLinkedListNode<T>? Next { get; set; }

    public SingleLinkedListNode(T val)
    {
        Value = val;
        Next = null;
    }
}

public class SingleLinkedList<T> : IEnumerable<T> where T : IComparable<T>
{
    private SingleLinkedListNode<T>? _head;
    public SingleLinkedListNode<T>? Head => _head;

    public void PushBack(T val)
    {
        var newNode = new SingleLinkedListNode<T>(val);

        if (_head == null)
        {
            _head = newNode;
            return;
        }

        var current = _head;

        while (current.Next != null)
            current = current.Next;

        current.Next = newNode;
    }

    public void PushFront(T val)
    {
        var newNode = new SingleLinkedListNode<T>(val);
        if (_head == null)
        {
            _head = newNode;
            return;
        }

        newNode.Next = _head;
        _head = newNode;
    }

    public bool Contains(T val) => SearchNode(val) != null;

    public SingleLinkedListNode<T>? SearchNode(T val)
    {
        var current = _head;

        while (current != null)
        {
            if (val.CompareTo(current.Value) == 0)
                return current;
            current = current.Next;
        }
        return null;
    }

    /// <summary>
    /// 删除第一个匹配到数值相同的节点
    /// </summary>
    /// <param name="val"></param>
    /// <returns></returns>
    public bool Remove(T val)
    {
        if (_head == null)
            return false;

        var current = _head.Next;
        var prev = _head;

        while (current != null)
        {
            // 查找到了
            if (val.CompareTo(current.Value) == 0)
                break;

            prev = current;
            current = current.Next;
        }

        if (current == null)
            return false;

        prev.Next = current.Next;

        return true;
    }

    public IEnumerator<T> GetEnumerator()
    {
        var current = _head;

        while (current != null)
        {
            yield return current.Value;
            current = current.Next;
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}