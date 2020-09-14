using System;
using System.Collections.Generic;
using System.Text;

namespace suffix
{
    class Program
    {
        static void Main(string[] args)
        {
            var suffixTree = new SuffixTree("abcde");

            Console.WriteLine(suffixTree);

        }
    }

    class SuffixTree
    {
        public SuffixTree(string input){
            _string = input;
            n = _string.Length;
            _root = new InnerNode(0, n, _string);
            BuildSlow();
        }

        private readonly string _string;
        private readonly int n;
        private readonly InnerNode _root;

        private void BuildSlow(){
            Console.WriteLine("Build Slow is running...");
            for (int i = 0; i < n; i++)
            {
                _root.children.Add(
                    new InnerNode(i, n, _string)
                );
            }
            Console.WriteLine("Build Slow is done.");
        }

        public override string ToString()
        {
            return _root.ToStringWithChildren();
        }
    }

    class Node
    {
        public Node(string _string){
            this._string = _string; //TODO does this work for memory?
        }
        protected readonly string _string;
        public string ToStringWithChildren(){
            StringBuilder sb = new StringBuilder();
            this._ToStringWithChildren(0, sb);
            return sb.ToString();
        }
        private void _ToStringWithChildren(int indent, StringBuilder sb){
            for (int i = 0; i < indent; i++)
            {
                sb.Append("  "); //TODO There has got to be an inline way to do this
            }
            sb.Append("└ ");
            sb.Append(this.ToString());
            sb.Append("\n");

            if(this is InnerNode){
                var kids = ((InnerNode) this).children; //TODO one line way to do this?
                foreach (Node node in kids)
                {
                    node._ToStringWithChildren(indent+1, sb);
                }
            }
        }
    }

    class LeafNode : Node
    {
        public LeafNode(int value, string _string):base(_string){
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
        public InnerNode(int startIndex, int endIndex, string _string):base(_string){
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

        // Splits an edge at an index
        public InnerNode Split(int indexToSplitOn){
            if(LabelStartIndex >= indexToSplitOn || LabelEndIndex <= indexToSplitOn){
                Console.WriteLine("ERROR: Split index is outside edge label"); // TODO use real error handling here
                return this;
            }
            var newInnerNode = new InnerNode(this.LabelStartIndex, indexToSplitOn, this._string);
            this.LabelStartIndex = indexToSplitOn;
            newInnerNode.children.Add(this);
            return newInnerNode;
        }
    }
}
