using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetEquation
{
    partial class Node //: IComparable<Node>
    {
        public Node(Node parrent){
            this.parrent = parrent;
        }
        public Node(Node parrent, int high){
            this.parrent = parrent;
            this.high = high;
        }

        public Node(){
        }
        public Node left = null;
        public Node right = null;
        public Node parrent = null;
        public int covered = 0;
        public int high = 0;
        public bool isRoot = false;

        // public int CompareTo(Node other) 
        // {
        //     if (null == other)
        //         return 1;
        //     return int.Compare(this.high, other.high);
        // }

        /// покрывает все деревья шаблонами 
        public static void TryCoverAll(ref Node curNode, ref Node pattern){
            if (dfs(ref curNode, ref pattern)){
                curNode.isRoot = true;
                coveredingDfs(ref curNode, ref pattern);
            }
            if (curNode.left != null) {
                TryCoverAll(ref curNode.left, ref pattern);
                TryCoverAll(ref curNode.right, ref pattern);
            }
        }

        /// проверяет (с корня), что деревья идентичны (возвращает true)
        public static bool dfs(ref Node tree, ref Node pattern){
            if (pattern.left != null) {
                if (tree.left != null){
                    return dfs(ref tree.left, ref pattern.left) && dfs(ref tree.right, ref pattern.right);
                }
                return false;
            }
            return true;
        }

        /// делает одно наложение шаблона и помечает вершины покрытыми 
        public static void coveredingDfs(ref Node tree, ref Node pattern){
            tree.covered++;
            if (pattern.left != null) {
                coveredingDfs(ref tree.left, ref pattern.left);
                coveredingDfs(ref tree.right, ref pattern.right);
            }
        }

        /// проверяет, что все вершины покрыты 
        public static bool checkCovereding(ref Node tree, ref List<Node> roots){
            if (tree.covered == 0)
                return false;
            
            if (tree.left != null && tree.parrent != null){
                if (tree.covered < 2)
                    return false;
            }
            if (tree.left != null){
                if (tree.isRoot)
                    roots.Add(tree);
                return checkCovereding(ref tree.left, ref roots) &&
                       checkCovereding(ref tree.right, ref roots);

            }
            return true;
        }

        // public static void Transfer(ref string brackets, ref Node curNode, int ind){
        //     if (ind == brackets.Length - 1)
        //         return;
        //     if (brackets[ind] == '(' && brackets[ind + 1] == '(' ) {
        //         curNode.left = new Node(curNode);
        //         curNode.right = new Node(curNode);
        //         Transfer(ref brackets, ref curNode.left, ind + 1);
        //     }
        //     if (brackets[ind] == '(' && brackets[ind + 1] == ')' ) {
        //         Transfer(ref brackets, ref curNode, ind + 1);
        //     }
        //     if (brackets[ind] == ')' && brackets[ind + 1] == ')' ){  // todo 
        //         Transfer(ref brackets, ref curNode.parrent, ind + 1);
        //     }
        //     if (brackets[ind] == ')' && brackets[ind + 1] == '(' ){
        //         Transfer(ref brackets, ref curNode.parrent.right, ind + 1);
        //     }
        // }

        public static int close_bracket_index_for_first_break(string brackets)
        {
            int count_opened = 1;
            for (int i = 1; i < brackets.Length; i++)
            {
                if (brackets[i] == '(')
                    count_opened++;
                else
                    count_opened--;
                if (count_opened == 0)
                    return i;
            }
            return -2;
        }

        public static void Transfer(ref Node curNode, string brackets)
        {
            Console.WriteLine(curNode.high);
            // tree[curr] = true;
            if (brackets == "")
                return;
            int index = close_bracket_index_for_first_break(brackets);
            curNode.left = new Node(curNode, curNode.high + 1);
            curNode.right = new Node(curNode, curNode.high + 1);
            
            Transfer(ref curNode.left, brackets.Substring(1, index - 1));
            Transfer(ref curNode.right, brackets.Substring(index + 1, brackets.Length - index - 1));

        }

    }
    // public class kek{
        // static void Main(string[] args)
        // {
        //     Node tree = new Node();
        //     tree.left = new Node();
        //     tree.right = new Node();
        //     tree.right.left = new Node();
        //     tree.right.right = new Node();

        //     Node pattern = new Node();
        //     pattern.left = new Node();
        //     pattern.right = new Node();

        //     Console.WriteLine(Node.dfs(ref tree, ref pattern));
        //     Console.WriteLine(Node.dfs(ref tree, ref pattern.left));
        //     Console.WriteLine(Node.dfs(ref tree, ref pattern.right));

        //     Console.WriteLine(Node.dfs(ref tree.left, ref pattern));
        //     Console.WriteLine(Node.dfs(ref tree.right, ref pattern));


        //     Console.WriteLine("kekekek");

        //     Node tranf = new Node();
        //     string brac = "((()())())";

        //     Node.Transfer(ref tranf, brac);

        //     Node kek = tranf.left;
        //     Node lol = tranf.right;
        //     Console.WriteLine(kek.left);
        //     Console.WriteLine(kek.right);
        //     Console.WriteLine(lol.left);
        //     Console.WriteLine(lol.right);

        //     int keke = 1;
        // }
    // }
    
}
