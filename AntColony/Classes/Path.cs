using AntColony.Classes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace AntColony
{
    public class Path
    {
        private double Qratio; //Коэффициент Q
        private double Aratio; //Коэффициент альфа
        private double Bratio; //Коэффициент бэта
        private double evaporation; // Процент испарения
        private int city = 1; //Данный город, в котором муравей находится
        private List<Edge> edges = new List<Edge>(); //Список дорог(граней)
        private List<int[]> fileList = new List<int[]>(); //Временное хранилище для граней
        private Ant ant; //Муравей
        private List<int> cityList = new List<int>();
        private List<double> results = new List<double>();
        private List<Edge> edgesCopy;
        private double result = 0;
        Random rng = new Random();
        public Path(int qratio, int aratio, int bratio, double evaporation) //Получаем коэффициенты из Program.cs
        {
            this.Qratio = qratio;
            this.Aratio = aratio;
            this.Bratio = bratio;
            this.ReadFile(); //Читаем input.txt
            this.GenerateEdges(); //Создаем грани как класс для удобств
            this.evaporation = evaporation;
        }
        public void Execute()
        {
            for (int j = 0; j < 10; j++)
            {
                for (int i = 1; i != this.fileList[this.fileList.Count - 1][1] + 1; i++)
                {
                    this.city = i;
                    Console.WriteLine("New path");
                    System.Threading.Thread.Sleep(200);
                    Console.Write(".");
                    System.Threading.Thread.Sleep(200);
                    Console.Write(".");
                    System.Threading.Thread.Sleep(200);
                    Console.Write(".");
                    this.Move();//Скорее всего тут будем двигать муравья               
                    this.AddLenghtToResult();//посчитать размер пройденного муравьем пути
                    this.results.Add(result);//добавить результат в список результатов
                                             //обновить значения для новой итерации
                    using (StreamWriter strw = new StreamWriter("resources/output.txt", true))
                    {
                        foreach (int cities in this.cityList)
                        {
                            strw.Write(cities);
                            strw.Write(' ');
                        }
                        strw.Write(this.result);
                        strw.WriteLine();
                    }
                    this.result = 0;
                    this.cityList.Clear();
                }
                int k = 1;
                foreach (double result in this.results)
                {
                    Console.WriteLine($" result {k} je {result}");
                    k++;
                }
                this.UpdateFeramonesAll();// ->Сперва идет испарение на всех дорогах
                this.UpdateEdge();
                Console.WriteLine($"Edge: {this.edges[0].Name()} {this.edges[0].GetPreramones()}");
            }
            for (int c = 0; c < this.edges.Count; c++)
            {
                Console.WriteLine($"{this.edges[c].Name()} {this.edges[c].GetPreramones()}");
            }
        }
        private void UpdateEdge()
        {
            for (int i = 0; i < this.edgesCopy.Count; i++)
            {
                this.edges[i].AddPheramones(this.edgesCopy[i].GetPreramones() - this.edges[i].GetPreramones());
            }
        }

        private List<Edge> FindEdges() //Ищем все грани 
        {
            List<Edge> necessaryEdges = new List<Edge>();
            Console.WriteLine("FindEdges():");
            for (int i = 0; i < this.edges.Count; i++)
            {
                if (this.edges[i].FindEdge(this.city) == true)
                {
                    necessaryEdges.Add(this.edges[i]);
                    Console.WriteLine($"First cykle: {necessaryEdges[necessaryEdges.Count - 1].Name()}");
                }
            }
            for (int i = 0; i < this.cityList.Count; i++)
            {
                for (int j = 0; j < necessaryEdges.Count; j++)
                {
                    if (necessaryEdges[j].FindEdge(this.cityList[i]) == true && this.cityList[i] != this.city)
                    {
                        Console.WriteLine(necessaryEdges[j].Name());
                        necessaryEdges.Remove(necessaryEdges[j]);
                    }
                }
            }
            Console.WriteLine("//FindEdges()//");
            return necessaryEdges;
        }
        private void Move()
        {
            this.cityList.Add(this.city);
            while (this.cityList.Count != this.fileList[this.fileList.Count - 1][1])
            {
                ant = new Ant(this.FindEdges(), this.rng);
                int result_city = ant.Move();
                this.city = this.FindEdges()[result_city].CityChange(this.city);
                Console.WriteLine($"City: {this.city} Result: {result_city}");
                if (this.cityList.Contains(this.city) == false)
                    this.cityList.Add(this.city);
                for (int i = 0; i < this.cityList.Count; i++)
                {
                    Console.WriteLine($"CityList: {this.cityList[i]}");
                }
            }
            this.cityList.Add(this.cityList[0]);
            // В конце move нужно обновить дороги           
            this.UpdateFeramonesAnt();// ->Потом добавлю ферамоны на те дорожки, где ходил муравей

        }
        private void GenerateEdges()
        {
            for (int i = 0; i < fileList.Count; i++)
            {
                this.edges.Add(new Edge(fileList[i][0], fileList[i][1], fileList[i][2], fileList[i][3], this.Aratio, this.Bratio, this.Qratio));
            }
            this.edgesCopy = this.edges;
        }
        private void ReadFile()
        {
            FileStream file1 = new FileStream("resources/input.txt", FileMode.Open); //новый FileStream
            StreamReader reader = new StreamReader(file1); //Reader который будет считывать FileStream
            for (int i = 0; reader.Peek() > -1; i++) //Считываем каждый элемент пока reader.Peek() > -1, то есть не закончился файл 
            {
                string line = reader.ReadLine();//Считываем каждую строку файла
                fileList.Add(new int[4]);//Четыре отдельных переменных в файле вида (0(город 1) 1(город 2) 300(длина пути) 200(количество феромонов, которое затем поделим на 100))
                for (int j = 0; j < 4; j++)
                {
                    fileList[i][j] = Convert.ToInt32(line.Split()[j]);//получаем строку из файла и преобразуем ее с помощью Split() который удаляет пробелы и получаем 4 независимых строки которые преобразуем в int и затем записываем в одну из переменных
                }
            }
            reader.Close(); //close reader
        }
        private void UpdateFeramonesAll()
        {
            //уменшаем на каждой дорожке значение ферамона по формуле
            //если значение ферамона менше нуля, то я зделаю его нулем 
            for (int i = 0; i < this.edges.Count; i++)
            {
                this.edges[i].DeecreasePheramones(this.edges[i].GetPreramones() * (1 - this.evaporation));
            }
        }
        private void UpdateFeramonesAnt()
        {
            double increase = 0;
            for (int j = 0; j < this.cityList.Count - 1; j++)
            {
                foreach (Edge edge in this.edgesCopy)
                {
                    if (edge.GetFirst() == this.cityList[j] && edge.GetSecond() == this.cityList[j + 1] || edge.GetFirst() == this.cityList[j + 1] && edge.GetSecond() == this.cityList[j])
                    {
                        increase += edge.GetLength();
                        Console.WriteLine($"Increase: {increase}");
                    }
                }
            }
            increase = this.Qratio / increase;
            for (int j = 0; j < this.cityList.Count - 1; j++)
            {
                foreach (Edge edge in this.edgesCopy)
                {
                    if (edge.GetFirst() == this.cityList[j] && edge.GetSecond() == this.cityList[j + 1] || edge.GetFirst() == this.cityList[j + 1] && edge.GetSecond() == this.cityList[j])
                    {
                        edge.AddPheramones(increase);
                    }
                }
            }
        }
        private void AddLenghtToResult()
        {
            for (int j = 0; j < this.cityList.Count - 1; j++)
            {
                foreach (Edge edge in this.edges)
                {
                    if (edge.GetFirst() == this.cityList[j] && edge.GetSecond() == this.cityList[j + 1] || edge.GetFirst() == this.cityList[j + 1] && edge.GetSecond() == this.cityList[j])
                    {
                        this.result += edge.GetLength();
                    }
                }

                //код на то, что бы сделать замкнутый маршрут, последний город сооединяеться с первым
                //foreach (Edge edge in this.edges)
                //{
                // if (edge.GetFirst() == this.cityList[j] && edge.GetSecond() == this.cityList[j + 1])
                // {
                //    this.result += edge.GetLength();
                // }                    
                //}
            }
        }
    }
}
