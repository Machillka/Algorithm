using DataStructures.Tree;
using DataStructures.LinkedList;
namespace Playground
{
    class Program
    {
        static void Main(string[] args)
        {
            var list = new SingleLinkedList<int>();

            list.PushBack(1);
            list.PushBack(2);
            list.PushBack(3);
            list.PushFront(0);

            foreach (var val in list)
                Console.WriteLine(val);

            Console.WriteLine(list.Contains(1));
            Console.WriteLine(list.Contains(999));

            list.Remove(2);
            foreach (var val in list)
                Console.WriteLine(val);
        }
    }
}
