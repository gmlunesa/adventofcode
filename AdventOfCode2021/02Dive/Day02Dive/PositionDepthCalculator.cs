using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day02Dive
{
    public class PositionDepthCalculator
    {
        IEnumerable<(string direction, long distance)> _directionDistance;

        public PositionDepthCalculator(string dataFile)
        {
            var depthValues = File.ReadAllLines(dataFile);
            _directionDistance = depthValues.Select(line => line.Split()).Select(x => (direction: x[0], distance: long.Parse(x[1]))).ToArray();
        }
        public void Run()
        {
            Console.WriteLine("First task: {0}", CalculatePositionByDepth());
            Console.WriteLine("Second task: {0}", CalculatePositionByDepthWithAim());
        }
        public long CalculatePositionByDepth()
        {
            var sum = _directionDistance
                        .GroupBy(x => x.direction, x => x.distance)
                        .ToDictionary(y => y.Key, y => y.Sum());
            return sum["forward"] * (sum["down"] - sum["up"]);
        }
        public long CalculatePositionByDepthWithAim()
        {
            var calculated = _directionDistance
                        .Aggregate((distance: 0L, depth: 0L, aim: 0L), (sum, curr) =>
                        curr.direction switch
                        {
                            "up" => (sum.distance, sum.depth, sum.aim - curr.distance),
                            "down" => (sum.distance, sum.depth, sum.aim + curr.distance),
                            "forward" => (sum.distance + curr.distance, sum.depth + sum.aim * curr.distance, sum.aim),
                            _ => throw new NotImplementedException()
                        });

            return calculated.distance * calculated.depth;
        }
    }
}
