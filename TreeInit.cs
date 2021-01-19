using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetEquation
{
    partial class Tree
    {
        /// <summary>
        /// форма хранения дерева в массиве
        /// </summary>
        public bool[] ArrayForm;


        /// <summary>
        /// конструктор класса Tree от одного параметра (массива True и False)
        /// </summary>
        /// <param name="ArrayForm"></param>
        public Tree(bool[] ArrayForm)
        {
            this.ArrayForm = ArrayForm;
        }


        /// <summary>
        /// индексатор для удобства работы с полем массива ArrayForm
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public bool this[int i]
        {
            get => ArrayForm[i];
            set { ArrayForm[i] = value; }
        }


        ///// <summary>
        ///// массив доступных имён переменных (побочных деревьев)
        ///// </summary>
        //public static string[] XYVarialables = new string[200];

        /// <summary>
        /// массив доступных имён переменных (побочных деревьев)
        /// </summary>
        public static string[] XVarialables = new string[200];


        /// <summary>
        /// возвращает строку, которая при запуске в Wolfram Mathematica рисует заданное дерево
        /// </summary>
        /// <param name="ArrayForm"></param>
        /// <returns></returns>
        public string WolframForm()
        {
            string ans = "Graph[{";
            for (int i = 1; i < this.ArrayForm.Length; i++)
            {
                if (this[i])
                {
                    ans += $"{(i - 1) / 2}->{i},";
                }
            }
            if (ans == "Graph[{")
            {
                ans += "0->0}]";
            }
            else
            {
                ans = ans.Substring(0, ans.Length - 1) + "}]";
            }
            return ans;
        }
    }
}
