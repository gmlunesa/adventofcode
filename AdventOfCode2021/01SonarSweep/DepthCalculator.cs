namespace Day01SonarSweep
{
    public class DepthCalculator
    {
        int[] _depthArray;
        public DepthCalculator(string dataFile)
        {
            var depthValues = File.ReadAllLines(dataFile);
            _depthArray = depthValues.Select(int.Parse).ToArray();
        }

        public void Run()
        {
            Console.WriteLine("First task: {0}", CalculateDepth(1));
            Console.WriteLine("Second task: {0}", CalculateDepth(3));
        }

        private int CalculateDepth(int windowSize)
        {
            return _depthArray
                    .Where((current, index) => index >= windowSize && current > _depthArray[index - windowSize])
                    .Count();
        }
    }
}
