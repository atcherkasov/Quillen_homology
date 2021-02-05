using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace GetEquation
{
    partial class Program
    {
        static string Gen_next(string s)
        {
            int n = (int)s.Length;
            string ans = "no";
            for (int i = n - 1, depth = 0; i >= 0; --i)
            {
                if (s[i] == '(')
                    --depth;
                else
                    ++depth;
                if (s[i] == '(' && depth > 0)
                {
                    --depth;
                    int open = (n - i - 1 - depth) / 2;
                    int close = n - i - 1 - open;
                    ans = s.Substring(0, i) + ')' + new string('(', open) + new string(')', close);
                    break;
                }
            }          
            return ans;
        }
        
        static void Display(ref bool[] tree, string brackets, int curr)
        {
            tree[curr] = true;
            if (brackets == "")
                return;
            int index = Node.close_bracket_index_for_first_break(brackets);

            Display(ref tree, brackets.Substring(1, index - 1), 2 * curr + 1);
            Display(ref tree, brackets.Substring(index + 1, brackets.Length - index - 1), 2 * curr + 2);

        }
        public static string getFirstSeq(int m) {
            string res = "";
            for (int i = 0; i < m - 1; i++)
                res += '(';
            for (int i = 0; i < m - 1; i++)
                res += ')';
            return res;
        }

        public static bool IsGomology(Node tree, Node pattern, int n, out int size)
        {
            size = 0;
            // пытаемся покрыть дерево полностью шаблоном
            int patternNum = 0;         // номер текущего шаблона
            Node.TryCoverAll(ref tree, ref pattern, ref patternNum);
            // проверяем, что дерево удалось покрыть шаблоном
            List<Node> roots = new List<Node>();
            if (!Node.checkCovereding(ref tree, ref roots))
            {
                return false;
            }
            roots = roots.OrderBy(o=>-o.high).ToList();
            
            // отрисовка дерева 
            // bool[] treeLit = new bool[(int)Math.Pow(2, n) - 1];
            // Display(ref treeLit, curr_tree, 0);
            // Tree lit2 = new Tree(treeLit);
            // Console.WriteLine(lit2.WolframForm());
            //

            // выкидываем лишнее для построения цепи 
            List<Node> chain = new List<Node>();
            HashSet<Node> allRoots = new HashSet<Node>(roots);
            for (int i = 0; i < roots.Count; i++){
                // можем ли выкинуть шаблон?
                if (Node.couldDelete(roots[i], ref pattern)){
                    roots[i].isRoot = 0;
                    Node.putOff(roots[i], ref pattern, roots[i].rootNumder);
                    roots[i].rootNumder = -1;
                } else
                    chain.Add(roots[i]);
            }
            
            // проверка минимального покрытия но то, что оно цепь
            List<Node> delitedRoots = new List<Node>(allRoots.Except(new HashSet<Node>(chain)));
            bool isChain = true;
            foreach (Node delRoot in delitedRoots) {
                isChain = false;
                List<int> delRootRoots = new List<int>(delRoot.roots);
                foreach (int numCovRoot in delRootRoots) {
                    Node covRoot = Node.findRoot(delRoot, numCovRoot);

                    // int covRootNum = covRoot.rootNumder;
                    covRoot.isRoot = 0;
                    Node.putOff(covRoot, ref pattern, covRoot.rootNumder);
                    covRoot.rootNumder = -1;

                    delRoot.isRoot++;
                    delRoot.rootNumder = delRoot.number;
                    Node.coveredingDfs( delRoot, ref pattern, delRoot.rootNumder);

                    List<Node> _ = new List<Node>();
                    if (Node.checkCovereding(ref tree, ref _))
                    {
                        isChain = true;
                        break;
                    }
                    delRoot.isRoot = 0;
                    Node.putOff(delRoot, ref pattern, delRoot.rootNumder);
                    delRoot.rootNumder = -1;

                    covRoot.isRoot++;
                    covRoot.rootNumder = covRoot.number;
                    Node.coveredingDfs(covRoot, ref pattern, covRoot.rootNumder);

                    if (!Node.checkCovereding(ref tree, ref _))
                        throw new IndexOutOfRangeException("bug in putOf and coveringDfs !");
                }
                if (!isChain)
                    break;
            }

            if (isChain)
            {
                size = chain.Count;
                return true;
            }

            return false;

            // вывод массива с вершинами номеров шаблоном 
            // Console.Write("номера корней шаблонов: ");
            // foreach (var root in chain)
            //     Console.Write($"{root.number} ");
            // Console.WriteLine();
            // todo: как-нибудь созранить цепь в файл 
        }
        
    }
}