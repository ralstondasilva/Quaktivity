# Quaktivity

This is a desktop application which monitors and informs the user of the latest global earthquake activity, as well as potentially affected cities.

## Summary

When the app launches, it first fetches a list of cities from OpenGeoCode: [World Cities](http://www.opengeocode.org/download/worldcities.zip). 
This zip file is uncompressed, and then parsed to create an array of Cities, which is stored in memory.
Only unique Cities are stored in the array. If there are multiple entries for the same city (names in different languages), we store the first entry encountered.

Next, the app makes a [call](http://earthquake.usgs.gov/earthquakes/feed/v1.0/summary/all_hour.geojson) to fetch the earthquake activity in the last hour.
The app displays the list of earthquakes occured. This feed is refreshed every 5 minutes, and so the app checks for updates every 5 minutes. 
You can manually refresh the list of earthquakes by clicking the refresh button. 
At every refresh, new earthquake activity is added, and if the existing earthquakes have updated activity, the feed is updated.

## Implementation Details

### List of Cities
The app reads the csv file line by line, and adds cities to a Dictionary if the same city hasnt been added before. 
The coordinates are used to uniquely identify the cities. 
For multiple entries for the same city, we use the first one we encountered - This can be extended to give english names priority over other languags if needed)
(Didn't use the NGA GNS Unique Feature Identifier (UFI) from the csv because some cities had the UFI field empty).

### Nearby Cities
The app lists the 3 nearest cities to the earthquake epicenter. 
This is done by iterating through the City[] array and replacing the largest item in a SortedDictionary if a nearer city is found.
This Operation completes in **O(log3 * n)** or  **O(n)** (where n is the total number of cities). 
This can be optimized further, but it seemed to be quick enough.

The distances between two cities was calculated using [GeoCoordinate.GetDistanceTo](https://msdn.microsoft.com/en-us/library/system.device.location.geocoordinate.getdistanceto(v=vs.110).aspx) method.


