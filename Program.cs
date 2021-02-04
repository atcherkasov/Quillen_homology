using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace GetEquation
{
    class Program
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


        static void Main(string[] args)
        {
            // считываем данные
            Console.Write("Введите число листьев у шаблонов: ");
            int m;
            int.TryParse(Console.ReadLine(), out m);

            Console.Write("Введите число листьев у деревьев: ");
            int n;
            int.TryParse(Console.ReadLine(), out n);

            // начало цикла по шаблонам 
            string curr_pattern = "";
            string curr_tree;
            for (int i = 0; i < m - 1; i++)
                curr_pattern += '(';
            for (int i = 0; i < m - 1; i++)
                curr_pattern += ')';
            
            while (curr_pattern != "no")
            {
                // переводим шаблон в класс Node
                Node pattern = new Node();
                Node.Transfer(ref pattern, curr_pattern);

                //
                bool[] patternLit = new bool[(int)Math.Pow(2, m) - 1];
                Display(ref patternLit, curr_pattern, 0);
                curr_pattern = Gen_next(curr_pattern);
                Tree lit = new Tree(patternLit);
                // if (lit.WolframForm() != "Graph[{0->1,0->2,2->5,2->6,6->13,6->14,14->29,14->30,30->61,30->62}]")
                //     continue;
                Console.WriteLine("\nНОВЫЙ ШАБЛОН ");
                Console.WriteLine(lit.WolframForm());
                //
                Console.WriteLine("Деревья с цепями:");
                
                // начало цикла по деревьям
                int cnt = 0;
                curr_tree = "";
                for (int i = 0; i < n - 1; i++)
                    curr_tree += '(';
                for (int i = 0; i < n - 1; i++)
                    curr_tree += ')';
                while (curr_tree != "no")
                {
                    
                    // bool[] treeLit = new bool[(int)Math.Pow(2, n) - 1];
                    // Display(ref treeLit, curr_tree, 0);
                    // Tree lit2 = new Tree(treeLit);
                    
                    // if (lit2.WolframForm() != "Graph[{0->1,0->2,2->5,2->6,6->13,6->14,14->29,14->30,30->61,30->62,62->125,62->126,126->253,126->254}]")
                    // {curr_tree = Gen_next(curr_tree);
                    //     continue;}
                    // Console.WriteLine(lit2.WolframForm());
                    //
                    
                    // переводим дерево в класс Node
                    Node tree = new Node();
                    Node.Transfer(ref tree, curr_tree);
                    // пытаемся покрыть дерево полностью шаблоном
                    Node.TryCoverAll(ref tree, ref pattern);
                    // проверяем, что дерево удалось покрыть шаблоном
                    List<Node> roots = new List<Node>();
                    if (!Node.checkCovereding(ref tree, ref roots)){
                        curr_tree = Gen_next(curr_tree);
                        continue;
                    }

                    foreach (var root in roots)
                        Console.Write($"{root.number} ");
                    Console.WriteLine();
                    roots = roots.OrderBy(o=>-o.high).ToList();
                    
                    //
                    bool[] treeLit = new bool[(int)Math.Pow(2, n) - 1];
                    Display(ref treeLit, curr_tree, 0);
                    Tree lit2 = new Tree(treeLit);
                    Console.WriteLine(lit2.WolframForm());
                    //
 
                    // выкидываем лишнее для построения цепи 
                    List<Node> chain = new List<Node>();
                    for (int i = 0; i < roots.Count; i++){
                        // можем ли выкинуть шаблон?
                        if (Node.couldDelete(roots[i], ref pattern)){
                            roots[i].isRoot = 0;
                            Node.putOff(roots[i], ref pattern);
                        } else
                            chain.Add(roots[i]);
                    }
                    curr_tree = Gen_next(curr_tree);
                    foreach (var root in chain)
                        Console.Write($"{root.number} ");
                    Console.WriteLine();
                    Console.WriteLine("Размер покрытия: " + chain.Count.ToString());

                    // todo: как-нибудь созранить цепь в файл 
                }
            }
        }
    }
}
