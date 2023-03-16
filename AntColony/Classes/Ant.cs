using AntColony.Classes;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace AntColony
{
    public class Ant
    {
        private List<Edge> edges;
        private List<double> wishes = new List<double>(); //Все вероятности перехода
        Random rng;
        public Ant(List<Edge> edges, Random rng)
        { 
            this.edges = edges;
            this.rng = rng;
        }
        public int Move()
        {
            double allSum = 0;
            foreach (Edge e in edges)
            {               
                allSum += e.Count();
            }
            for (int i = 0; i < this.edges.Count; i++)
            {
                this.wishes.Add(this.edges[i].Count() / allSum);
                Console.WriteLine($"{this.edges[i].Count() / allSum} {this.edges[i].Name()}");
            }
            double random = rng.NextDouble();
            double wishSum = 0;
            int j = 0;
            for (j = 0; j < this.wishes.Count; j++)
            {
                wishSum += this.wishes[j];
                if (wishSum >= random)
                {
                    break;
                }
            }
            Console.WriteLine($"Random: {random} allSum: {allSum}");
            return j; //Вернули город в Path.cs
        }
    }
}
