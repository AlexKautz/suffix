using System;
using System.Collections.Generic;

namespace suffix
{
    class Program
    {
        static void Main(string[] args)
        {
            // Console.WriteLine("\nWhat is your name? ");
            // var name = Console.ReadLine();
            // var date = DateTime.Now;
            // Console.WriteLine($"\nHello, {name}, on {date:d} at {date:t}!");
            // Console.Write("\nPress any key to exit...");
            // Console.ReadKey(true);
            string bigString = "HelloWorld";
            LeafNode leafNode = new LeafNode(2, ref bigString);
            InnerNode innerNode = new InnerNode(1, 5, ref bigString);
            Console.WriteLine($"leafNode {leafNode}");
            Console.WriteLine($"innerNode {innerNode}");
            innerNode.children.Add(leafNode);
            Console.WriteLine($"innerNode {innerNode} kids {innerNode.children[0]}");

        }
    }

    class Node
    {
        public Node(ref string _string){
            this._string = _string; // TODO does this work for memory?
        }
        protected string _string;
    }

    class LeafNode : Node
    {
        public LeafNode(int value, ref string _string):base(ref _string){ // Am I double refing here
            Value = value;
        }

        public int Value {get;}

        public override string ToString()
        {
            return $"Value: {Value} Suffix: {_string.Substring(Value)}";
        }
    }

    class InnerNode : Node
    {
        public InnerNode(int startIndex, int endIndex, ref string _string):base(ref _string){
            LabelStartIndex = startIndex;
            LabelEndIndex = endIndex;
            children = new List<Node>();
        }
        public int LabelStartIndex;
        public int LabelEndIndex;
        public List<Node> children {get;}

        public override string ToString()
        {
            return _string.Substring(LabelStartIndex, LabelEndIndex-LabelStartIndex);
        }
    }
}
