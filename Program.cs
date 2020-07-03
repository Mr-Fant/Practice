using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Practice
{
    class Program
    {
        //Создание функции принимает граф, начало и конец
        static List<KeyValuePair<int, int>> dijkstra(Dictionary<int, List<KeyValuePair<int, int>>> gr, int begin, int end)
        {
            List<int> cost = new List<int>();
            List<int> p = new List<int>();
            List<bool> u = new List<bool>();
            List<KeyValuePair<int, int>> path = new List<KeyValuePair<int, int>>();
            // Инийиализация массивов
            for (int i = 0; i <= gr.Count; i++)
            {
                cost.Add(int.MaxValue);
                u.Add(false);
                //Не из какой нет путей
                p.Add(-1);
            }
            // Алгоритм Дейкстры
            cost[begin] = 0;
            //Идем по вершинам
            for (int i = 0; i < gr.Count; i++)
            {
                int v = -1;
                //Ищем с мин. стоимостью
                for (int j = 0; j < gr.Count; j++)
                {
                    if (/*не посещали*/!u[j] && (v == -1 || cost[j] < cost[v]))
                        v = j;
                }
                if (cost[v] == int.MaxValue)
                    break;
                u[v] = true;
                //Проверка есть ли v в графе
                if (gr.ContainsKey(v))
                {
                    //Идем по всем соседним
                    for (int j = 0; j < gr[v].Count; j++)
                    {
                        //Релаксация путей
                        int to = gr[v][j].Key;
                        int c = gr[v][j].Value;
                        if (cost[v] + c < cost[to])
                        {
                            cost[to] = cost[v] + c;
                            p[to] = v;
                        }
                    }
                }
            }

            // Восстановление пути
            // Записываем все в один массив
            for (int i = end; i != begin; i = p[i])
            {
                path.Add(new KeyValuePair<int, int>(i, cost[i]));
            }
            //Добавляем первую вершину
            path.Add(new KeyValuePair<int, int>(begin, cost[begin]));
            path.Reverse();
            return path;
        }

        static void Main(string[] args)
        {
            Dictionary<int, List<KeyValuePair<int, int>>> gr = new Dictionary<int, List<KeyValuePair<int, int>>>();
            string[] lines = File.ReadAllLines("test.csv");

            for (int i = 0; i < lines.Length; i++)
            {
                string[] item = lines[i].Split(new char[] { ';' });
                // Если есть ключ то мы добавляем смежную вершину иначе добавляем вершину со смежной
                if (gr.ContainsKey(int.Parse(item[0])))
                {
                    gr[int.Parse(item[0])].Add(new KeyValuePair<int, int>(int.Parse(item[1]), int.Parse(item[2])));
                }
                else
                {
                    gr.Add(int.Parse(item[0]), new List<KeyValuePair<int, int>>());
                    gr[int.Parse(item[0])].Add(new KeyValuePair<int, int>(int.Parse(item[1]), int.Parse(item[2])));
                }
                // Добавляем вершины в граф из которых исходят пути
                if (!gr.ContainsKey(int.Parse(item[1])))
                {
                    gr.Add(int.Parse(item[1]), new List<KeyValuePair<int, int>>());
                }
            }
            // первое значение(номер вершины) второе стоимость
            List<KeyValuePair<int, int>> path = dijkstra(gr, 1, 20);
            Console.Write(string.Join(" -> ", path.Select(o => string.Format($"{o.Key}({o.Value})").ToString()).ToArray()));
            Console.ReadKey();
        }
    }
}