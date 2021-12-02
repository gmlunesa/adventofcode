//using Day01SonarSweep;

using Day02Dive;

string depthDataFile = @"Data\PositionDepthMeasurements.dat";
PositionDepthCalculator calculator = new PositionDepthCalculator(depthDataFile);
calculator.Run();