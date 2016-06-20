# Quaktivity

This is a desktop application which monitors and informs the user of the latest global earthquake activity, as well as the potentially affected cities.

## Summary

When the app launches, it first fetches a list of [cities](http://www.opengeocode.org/download/worldcities.zip) from OpenGeoCode. 
This zip file is uncompressed, and then parsed to create an array of Cities, which is stored in memory.
Only unique cities are stored in the array. If there are multiple entries for the same city (names in different languages), the first entry encountered is stored.

Next, the app makes a [call](http://earthquake.usgs.gov/earthquakes/feed/v1.0/summary/all_hour.geojson) to fetch the earthquake activity in the last hour.
The app displays the list of earthquakes occured. The endpoint is refreshed every 5 minutes, so the app checks for updates every 5 minutes.
At every refresh, new earthquake activity is added, and if the existing earthquakes have updated activity, the list is updated. 
You can manually refresh the list of earthquakes by clicking the refresh button. 


## Settings

The app has some configurable settings. These are located in Properties\Settings:
- The number of nearby cities to display
- The auto refresh time interval
- The urls for the earthquake feed and the list of cities

## Instructions

To run the project within Visual Studio, open Quakitivity\Quakitivity.sln and click Start(F5).
Alternately, build the solution and launch Quakitivity.exe from the bin\Debug or bin\Release folder.

## Dependencies/References

All the references are part of the .NET framework

| Reference                         | Description										  |
| --------------------------------- | --------------------------------------------------- |
| System.Net.Http                   | To make a REST call to the earthquake.usgs.gov API  |
| System.IO.Compression.FileSystem	| To unzip the list of cities returned by OpenGeoCode |
| System.Runtime.Serialization		| To parse the JSON response to C# objects			  |
| System.Windows.Interactivity		| To add a Loaded event to the ViewModel			  |
| System.Device						| To get the distance between two GeoCoordinates	  |

## Implementation Details

### List of Cities

The app reads the csv file line by line, and adds the city to a Dictionary if it hasn't been added before. 
The coordinates are used to uniquely identify the cities (The depth parameter is ignored, since distances on the surface would give us the same result).  
For multiple entries for the same city, we store the first one we encountered - This can be changed to store the english names if needed)
(Didn't use the NGA GNS Unique Feature Identifier (UFI) from the csv because some cities had this field empty).
This operation completes in **O(n) time** and uses **O(n) space** (Where n is the total number of world cities).

### Nearby Cities

The app lists the 3 nearest cities to the earthquake epicenter. 
This is done by iterating through the City[] array and replacing the largest item in a SortedDictionary if a nearer city is found.
This Operation completes in **O(log3 * n)** or  **O(n) time** (where n is the number of unique world cities). And  **O(3) or constant space**
This can be optimized further, but for now it seemed to be quick enough.

The distance between two cities was calculated using the [GeoCoordinate.GetDistanceTo](https://msdn.microsoft.com/en-us/library/system.device.location.geocoordinate.getdistanceto(v=vs.110).aspx) method.


