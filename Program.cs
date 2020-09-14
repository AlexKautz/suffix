using System;
using System.Collections.Generic;
using System.Text;

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
            var innerNode = new InnerNode(0, 5, ref bigString);
            var innerNode2 = new InnerNode(1, 3, ref bigString);
            var leafNode = new LeafNode(0, ref bigString);
            var leafNode2 = new LeafNode(1, ref bigString);

            innerNode2.children.Add(leafNode2);

            innerNode.children.Add(innerNode2);
            innerNode.children.Add(leafNode);

            Console.WriteLine(innerNode.ToStringWithChildren());

        }
    }

    class Node
    {
        public Node(ref string _string){
            this._string = _string; // TODO does this work for memory?
        }
        protected string _string;
        public string ToStringWithChildren(){
            StringBuilder sb = new StringBuilder();
            this._ToStringWithChildren(0, sb);
            return sb.ToString();
        }
        private void _ToStringWithChildren(int indent, StringBuilder sb){
            for (int i = 0; i < indent; i++)
            {
                sb.Append(" "); // There has got to be an inline way to do this
            }
            sb.Append("└");
            sb.Append(this.ToString());
            sb.Append("\n");

            if(this is InnerNode){
                var kids = ((InnerNode) this).children; // one line way to do this?
                foreach (Node node in kids)
                {
                    node._ToStringWithChildren(indent+1, sb);
                }
            }
        }
    }

    class LeafNode : Node
    {
        public LeafNode(int value, ref string _string):base(ref _string){ // Am I double refing here
            Value = value;
        }

        public int Value {get;}

        public override string ToString()
        {
            return $"{_string.Substring(Value)} {Value}";
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
