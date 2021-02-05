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
        public Node(Node parrent, int high, int number){
            this.parrent = parrent;
            this.high = high;
            this.number = number;
        }
        public Node(){
        }
        public Node left = null;
        public Node right = null;
        public Node parrent = null;
        public int covered = 0;
        public int high = 0;
        public int isRoot = 0;
        public int isLeaf = 0;
        public int number = 0;
        public int rootNumder = -1;
        public HashSet<int> roots = new HashSet<int>();

        /// покрывает все деревья шаблонами 
        public static void TryCoverAll(ref Node curNode, ref Node pattern, ref int patternNum){
            if (dfs(ref curNode, ref pattern)){
                curNode.isRoot++;
                curNode.rootNumder = curNode.number;
                coveredingDfs(curNode, ref pattern, curNode.rootNumder);
                // patternNum = curNode.number;
            }
            if (curNode.left != null) {
                TryCoverAll(ref curNode.left, ref pattern, ref patternNum);
                TryCoverAll(ref curNode.right, ref pattern, ref patternNum);
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
        public static void coveredingDfs( Node tree, ref Node pattern, int patternNum){
            tree.covered++;
            tree.roots.Add(patternNum);
            if (pattern.left != null) {
                coveredingDfs( tree.left, ref pattern.left, patternNum);
                coveredingDfs( tree.right, ref pattern.right, patternNum);
            } else 
                tree.isLeaf++;
        }

        /// проверяет, что все вершины покрыты 
        public static bool checkCovereding(ref Node tree, ref List<Node> roots){
            if (tree.isRoot > 0)
                roots.Add(tree);
            if (tree.covered == 0)
                return false;
            if (tree.left == null)
                return true;
            if (tree.parrent != null &&  tree.isLeaf + tree.isRoot == tree.covered)
                return false;
            if (tree.left != null){
                return checkCovereding(ref tree.left, ref roots) &&
                       checkCovereding(ref tree.right, ref roots);
            }
            return true;
        }

        /// проверяем, что можем снять шаблон и условия продолжат выполняться 
        public static bool couldDelete(Node subTree, ref Node pattern) {
            if (subTree.covered <= 1)
                return false;
            if (subTree.isLeaf + subTree.isRoot == subTree.covered - 1) {
                if (pattern.parrent != null && pattern.left != null) {
                    return false;
                }
            }
            if (pattern.left != null)
                return couldDelete(subTree.left, ref pattern.left) &&
                       couldDelete(subTree.right, ref pattern.right);
            return true;
        }
        
        /// снимаем шаблон с дерева
        public static void putOff(Node subTree, ref Node pattern, int rootNumber){
            subTree.covered--;
            subTree.roots.Remove(rootNumber);
            if (pattern.left != null) {
                    putOff(subTree.left, ref pattern.left, rootNumber);
                    putOff(subTree.right, ref pattern.right, rootNumber);
            } else {
                subTree.isLeaf--;
            }
            return;
        }

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

        /// переводит скобочную последовательность в класс Node
        public static void Transfer(ref Node curNode, string brackets)
        {
            if (brackets == "")
                return;
            int index = close_bracket_index_for_first_break(brackets);
            curNode.left = new Node(curNode, curNode.high + 1, 2 * curNode.number + 1);
            curNode.right = new Node(curNode, curNode.high + 1, 2 * curNode.number + 2);
            
            Transfer(ref curNode.left, brackets.Substring(1, index - 1));
            Transfer(ref curNode.right, brackets.Substring(index + 1, brackets.Length - index - 1));
        }

        public static Node findRoot(Node curNode, int rootNum)
        {
            if (curNode.rootNumder == rootNum)
                return curNode;
            if (curNode.parrent != null)
                return findRoot(curNode.parrent, rootNum);
            throw new IndexOutOfRangeException("don't find root");
        }
    
        // проверяем, что зелёная зона не распадётся без covRoot
        public static bool checkGreen(Node covRoot, Node delRoot, Node pattern, out Node intetsept) {
            if (covRoot == delRoot) {
                intetsept = pattern;
                return true;
            } if (covRoot.covered <= 1) {
                intetsept = new Node();
                return false;
            } if (covRoot.isLeaf + covRoot.isRoot == covRoot.covered - 1) {
                if (pattern.parrent != null && pattern.left != null) {
                    intetsept = new Node();
                    return false;
                }
            } if (pattern.left != null)
                return checkGreen(covRoot.left, delRoot, pattern.left, out  intetsept) &&
                       checkGreen(covRoot.right, delRoot, pattern.right, out  intetsept);
            intetsept = new Node();
            return true;
        }


        // public static bool couldChange(Node covRoot, Node delRoot, Node pattern, bool onDel) {
        //     if (!onDel && covRoot != delRoot)
        //     {
        //         if (covRoot.covered <= 1)
        //             return false;
        //         if (covRoot.isLeaf + covRoot.isRoot == covRoot.covered - 1) {
        //             if (pattern.parrent != null && pattern.left != null) {
        //                 return false;
        //             }
        //         }
        //         if (pattern.left != null)
        //             return couldChange(covRoot.left, delRoot, pattern.left, onDel) &&
        //                    couldChange(covRoot.right, delRoot, pattern.right, onDel);
        //         return true;
        //     }
        //
        //     if (covRoot == delRoot)
        //     {
        //         
        //     }
        //     
        // }
        
    }    
}
