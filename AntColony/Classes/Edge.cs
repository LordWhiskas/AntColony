using System;
using System.Text.RegularExpressions;
using System.Threading;

namespace AntColony.Classes
{
    public class Edge
    {
        private double pheromone_amount; //Количество феромонов
        private int firstPoint;          //Первый город
        private int secondPoint;         //Второй город
        private double length;         //Расстояние
        private double Aratio;           //альфа
        private double Bratio;           //бета
        private double Qratio;           //Q
        public Edge(int firstEdge, int secondEdge, double length, double pheromone_amount, double Aratio, double Bratio, double Qratio)
        { 
            this.pheromone_amount = pheromone_amount/100; //Делим на 100 просто чтобы перевести в десятичную дробь. Потом переделаем это, чтобы сразу в файле писать дробное.
            this.firstPoint = firstEdge;
            this.secondPoint = secondEdge;
            this.length = length;
            this.Aratio = Aratio;
            this.Bratio = Bratio;
            this.Qratio = Qratio;
        }

        public double Count()
        {
            /*            Console.WriteLine(this.Qratio / this.lenth);
            */
            Console.WriteLine($"Bratio: {Math.Pow(this.Qratio / this.length, this.Aratio) * Math.Pow(this.pheromone_amount, this.Bratio)}");
            return Math.Pow(this.pheromone_amount, this.Aratio) * Math.Pow(this.Qratio/this.length, this.Bratio); //Получаем по формуле вероятность перехода.    
        }

        public bool FindEdge(int point)
        {
            if (this.firstPoint == point || this.secondPoint == point)//Есть ли в грани город point. К примеру путь между(1 2). Является ли 3 одним из этих городов.
                return true;
            return false;
        }

        public string Name()
        {
            return this.firstPoint.ToString() + ' ' + this.secondPoint.ToString();
        }
        public int CityChange(int city)
        {
            if (city == this.firstPoint)
            {
                Console.WriteLine($"this.secondPoint: {this.secondPoint}");
                return this.secondPoint;
            }
            else
            {
                return this.firstPoint;
            }
        }
        public void DeecreasePheramones(double decrease)
        {
            if (this.pheromone_amount > 0)
            {
                this.pheromone_amount = decrease;
                if(this.pheromone_amount < 0)
                {
                    this.pheromone_amount = 0;
                }
            }
        }
        public void AddPheramones(double add)
        {
            this.pheromone_amount += add;
            if (this.pheromone_amount < 0)
            {
                this.pheromone_amount = 0;
            }
        }
        public double GetPreramones()
        {
            return this.pheromone_amount;
        }
        public int GetFirst()
        {
            return this.firstPoint;
        }
        public int GetSecond()
        {
            return this.secondPoint;
        }
        public double GetLength()
        {
            return this.length;
        }       
        public bool FindEdge(int first, int second)
        {
            if (this.GetFirst() == first && this.GetSecond() == second)
                return true;
            return false;
        }
    }
}
