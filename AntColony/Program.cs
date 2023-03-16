using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AntColony
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Path path = new Path(200, 1, 4, 0.36);
            path.Execute();
        }
    }
}
