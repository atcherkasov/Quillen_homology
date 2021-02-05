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
        static void Main(string[] args)
        {
            // считываем данные
            Console.Write("Введите число листьев у шаблонов: ");
            int m;
            int.TryParse(Console.ReadLine(), out m);

            Console.Write("Введите число листьев у деревьев: ");
            int n;
            int.TryParse(Console.ReadLine(), out n);
            
            string curr_pattern = getFirstSeq(m);
            string curr_tree;

            // начало цикла по шаблонам 
            while (curr_pattern != "no")
            {
                // переводим шаблон в класс Node
                Node pattern = new Node();
                Node.Transfer(ref pattern, curr_pattern);

                // отрисовка дерева 
                bool[] patternLit = new bool[(int)Math.Pow(2, m) - 1];
                Display(ref patternLit, curr_pattern, 0);
                curr_pattern = Gen_next(curr_pattern);
                Tree lit = new Tree(patternLit);
                //    
                
                Console.WriteLine("\nНОВЫЙ ШАБЛОН ");
                Console.WriteLine(lit.WolframForm());
                // Console.WriteLine("Деревья с покрытиями:");

                int[,] line = new int[n + 1 - m, n + 1]; // размер цепи х листья
                int rows = line.GetUpperBound(0) + 1;
                int columns = line.GetUpperBound(1) + 1;
                
                for (int i = 0; i < columns; i++) line[0, i] = i;
                for (int i = 0; i < rows; i++) line[i, 0] = i;

                for (int i = 1; i < rows; i++) {
                    for (int j = 1; j < columns; j++)
                        line[i, j] = 0;
                }

                for (int numLeafes = m; numLeafes <= n; numLeafes++)
                {
                    curr_tree = getFirstSeq(numLeafes);
                    // начало цикла по деревьям
                    while (curr_tree != "no")
                    {
                        // переводим дерево в класс Node
                        Node tree = new Node();
                        Node.Transfer(ref tree, curr_tree);

                        int size;
                        if (IsGomology(tree, pattern, numLeafes, out size))
                        {
                            // значит, что наш pattern образовывает цепь размера size
                            // для данного дерева у которого numLeafes листьев
                            line[size, numLeafes]++;
                        }

                        curr_tree = Gen_next(curr_tree);
                    }
                }

                string ans = "";
                for (int i = 1; i < rows; i++) {
                    for (int j = 1; j < columns; j++)
                    {
                        if (line[i, j] != 0)
                        {
                            ans += $"{line[i, j]}*X^[{j}]*Y^[{i}] + ";
                        }
                    }
                }
                Console.WriteLine(ans + " ...");
            }
        }
    }
}
