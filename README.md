# VoronoiDiagram
Fortune's algorithm &amp; Bowyer–Watson algorithm

## macOS
If you're anything like me, you'll want to develop on your mac! To do so:

1. Install Visual Studio Community. Make sure you include .NET core, but feel free to remove the Xamarin install.

_Try running the project first, but if you encounter errors when creating the bitmap, you may have to do the following:_

2. Install the nuget packages for the project in order to get the `System.Drawing` library on mac. 
3. Run `brew install mono-libgdiplus` for graphics functionality. I think `System.Drawing` relies on it.

If you run the BruteForce project, you should get `img.bmp` saved to the `VoronoiBruteForce` folder of the sollution.
