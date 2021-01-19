using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetEquation
{
    partial class Node
    {
        public Node(Node parrent){
            this.parrent = parrent;
        }

        public Node(){
        }
        public Node left = null;
        public Node right = null;
        public Node parrent = null;
        public bool covered = false;

        /// covering all nodes 
        public static void TryCoverAll(ref Node curNode, ref Node pattern){
            if (dfs(ref curNode, ref pattern)){
                // Console.WriteLine("lol");
                coveredingDfs(ref curNode, ref pattern);
            }
            // Console.WriteLine("gogogo");
            if (curNode.left != null) {
                TryCoverAll(ref curNode.left, ref pattern);
                TryCoverAll(ref curNode.right, ref pattern);
            }
        }

        /// return true if trees is same 
        public static bool dfs(ref Node tree, ref Node pattern){
            // Console.WriteLine("1");
            if (pattern.left != null) {
                if (tree.left != null){
                    // Console.WriteLine("22");
                    return dfs(ref tree.left, ref pattern.left) && dfs(ref tree.right, ref pattern.right);
                }
                // Console.WriteLine("333");
                return false;
            }
            // Console.WriteLine(">>>> end");
            return true;
        }

        /// covering nodes by covering pattern one time
        public static void coveredingDfs(ref Node tree, ref Node pattern){
            if (pattern.left != null) {
                tree.covered = true;
                coveredingDfs(ref tree.left, ref pattern.left);
                coveredingDfs(ref tree.right, ref pattern.right);
            }
        }

        /// checking that all nodes are covered
        public static bool checkCovereding(ref Node tree){
            if (tree.left != null){
                if (!tree.covered)
                    return false;
                return checkCovereding(ref tree.left) && checkCovereding(ref tree.right);
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
            // tree[curr] = true;
            if (brackets == "")
                return;
            int index = close_bracket_index_for_first_break(brackets);
            curNode.left = new Node(curNode);
            curNode.right = new Node(curNode);
            
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
