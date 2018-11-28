# Voronoi Algorithm Analysis
The Bowyer–Watson sollution compared to a brute force implementation.

Once you have the program compiled and running, you can enter the following commands:

- `1` will run the entire test suite for all of the algorithms.
- `0` will generate a diagram for each algorithm. The colorful one (`brute_force_output.bmp`) is from the Brute Force algorithm. The grayscale one (`bowyer_output.bmp`) is from the Bowyer-Watson algorithm. They are both generated using the same 100 randomly generated input points in the range (0, 1000) inclusive.
- `-1` exits the program.

If you run the VoronoiAlgorithms project, you should get `img.bmp` saved to the `VoronoiBruteForce` folder of the solution.

## Windows

1. Install Visual Studio

_Try running the project first, but you may need to ensure that nuget packages are installed, and that you have .NET core 2.0+ installed._

## macOS
If you're anything like me, you'll want to develop on your mac! To do so:

1. Install Visual Studio Community. Make sure you include .NET core, but feel free to remove the Xamarin install.

_Try running the project first, but if you encounter errors when creating the bitmap, you may have to do the following:_

2. Install the nuget packages for the project in order to get the `System.Drawing` library on mac. 

3. Run `brew install mono-libgdiplus` for graphics functionality. I think `System.Drawing` relies on it.