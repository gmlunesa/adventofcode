using Day01SonarSweep;

string depthDataFile = @"Data\DepthMeasurements.dat";
DepthCalculator calculator = new DepthCalculator(depthDataFile);
calculator.Run();