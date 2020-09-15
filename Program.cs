using System;
using System.Collections.Generic;
using System.Text;

namespace suffix
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine( new SuffixTree("xabxa"));
            Console.WriteLine( new SuffixTree("aaaaaaaaaaaaa"));
            Console.WriteLine( new SuffixTree("abcdefghijklmnopqrstuvwxyz"));
            Console.WriteLine( new SuffixTree("ab12ab23ab34"));


        }
        public static readonly bool debug = false;
    }


    class SuffixTree
    {
        public SuffixTree(string input){
            _string = input + "$";
            n = _string.Length;
            _root = new InnerNode(0, n, _string);
            BuildSlow();
        }

        private readonly string _string;
        private readonly int n;
        private readonly InnerNode _root;

        public void BuildSlow(){
            // Console.WriteLine("Build Slow is running...");
            _root.children.Add(new LeafNode(0, n, 0, _string));
            InnerNode branchingPoint;
            int lastIndex;
            for (int i = 1; i < n; i++)
            {
                (branchingPoint, lastIndex) = _root.Traverse(i);
                branchingPoint.children.Add(
                    new LeafNode(lastIndex, n, i, _string)
                );
            }
            // Console.WriteLine("Build Slow is done.");
        }

        public override string ToString()
        {
            return _root.ToStringWithChildren();
        }
    }

    class Node
    {
        public Node(int startIndex, int endIndex, string _string){
            this._string = _string; //TODO does this work for memory?
            LabelStartIndex = startIndex;
            LabelEndIndex = endIndex;
        }
        protected readonly string _string;
        public int LabelStartIndex;
        public int LabelEndIndex;
        public string ToStringWithChildren(){
            StringBuilder sb = new StringBuilder();
            this._ToStringWithChildren(0, sb);
            return sb.ToString();
        }
        // Splits an edge at an index
        public InnerNode Split(int indexToSplitOn){
            if(LabelStartIndex >= indexToSplitOn || LabelEndIndex <= indexToSplitOn){
                Console.WriteLine("ERROR: Split index is outside edge label"); // TODO use real error handling here
                return null;
            }
            var newInnerNode = new InnerNode(this.LabelStartIndex, indexToSplitOn, this._string);
            this.LabelStartIndex = indexToSplitOn;
            newInnerNode.children.Add(this);
            return newInnerNode;
        }
        private void _ToStringWithChildren(int indent, StringBuilder sb){
            for (int i = 0; i < indent; i++)
            {
                sb.Append("  "); //TODO There has got to be an inline way to do this
            }
            sb.Append("└ ");
            sb.Append(this.ToString());
            sb.Append("\n");

            if(this is InnerNode thisInnerNode){
                foreach (Node node in thisInnerNode.children)
                {
                    node._ToStringWithChildren(indent+1, sb);
                }
            }
        }
    }

    class LeafNode : Node
    {
        public LeafNode(int startIndex, int endIndex, int value, string _string):base(startIndex, endIndex, _string){
            Value = value;
        }

        public int Value {get;}

        public override string ToString()
        {
            if (Program.debug){
                return $"{this.LabelStartIndex} {this.LabelEndIndex} {Value}";
            } else {
                return $"{_string.Substring(LabelStartIndex, LabelEndIndex - LabelStartIndex)} {Value+1}";
            }
        }
    }

    class InnerNode : Node
    {
        public InnerNode(int startIndex, int endIndex, string _string):base(startIndex, endIndex, _string){
            children = new List<Node>();
        }
        public List<Node> children {get;}

        public override string ToString()
        {
            if(Program.debug){
                return $"{this.LabelStartIndex} {this.LabelEndIndex}";
            } else {
                return _string.Substring(LabelStartIndex, LabelEndIndex-LabelStartIndex);
            }
        }


        // Traverses the tree with the added string and returns the final node and index.
        public (InnerNode, int) Traverse(int startIndexOfSuffix){
            
            foreach (Node child in this.children)
            {
                if( _string[startIndexOfSuffix] == _string[child.LabelStartIndex]){
                    // if the kid's label starts with the same letter as the traversal, then...
                    for (int i = 0; i < child.LabelEndIndex - child.LabelStartIndex; i++)
                    {
                        if(_string[startIndexOfSuffix+i] != _string[child.LabelStartIndex+i]){
                            // we have arrived at the difference
                            // so we split the kid
                            var newChild = child.Split(child.LabelStartIndex+i);
                            this.children.Remove(child);
                            this.children.Add(newChild);
                            return (newChild, startIndexOfSuffix+i);
                        }
                    }
                    // We never got a difference. Since the last character is unique, we are not at the end of the input suffix.
                    // Thus we are at the end of the edge label.
                    // so we recursively call Traverse
                    // we can not be at a Leaf Node Right?
                    var childInnerNode = child as InnerNode;
                    if (childInnerNode == null){
                        Console.WriteLine("ERROR: we somhow landed at a LeafNode while calling Traverse");
                    }
                    return childInnerNode.Traverse(startIndexOfSuffix + (childInnerNode.LabelEndIndex - childInnerNode.LabelStartIndex));
                }
            }
            // none of the kids had the right starting letter
            // so we return ourself
            return (this, startIndexOfSuffix);
        }

    }
}
