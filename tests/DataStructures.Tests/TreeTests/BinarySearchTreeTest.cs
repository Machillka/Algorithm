namespace Tests.Tree;

using DataStructures.Tree;
public class BinarySearchTreeTests
{
    private readonly BinarySearchTree<int> _bst = new();

    [Fact]
    public void Insert_And_InOrder_ShouldReturnSortedSequence()
    {
        var data = new[] { 7, 1, 5, 3, 9 };
        foreach (var val in data)
            _bst.Insert(val);
        var res = _bst.Traverse().ToArray();
        Assert.Equal([1, 3, 5, 7, 9], res);
    }

    [Theory]
    [InlineData(5, true)]
    [InlineData(10, false)]
    public void Search_ReturnsExpected(int value, bool expected)
    {
        var data = new[] { 2, 4, 5, 6, 8 };
        foreach (var v in data) _bst.Insert(v);

        Assert.Equal(expected, _bst.Search(value));
    }
}