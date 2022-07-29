# BedrockLivePlayerTrackingAPI

This API is to receive* bedrock minecraft players name, coordinates, dimension and writes it to a Markers file.

the Database used is in memory, so only alive while the app is running

* Receives info from https://github.com/TheDogg/BedrockLivePlayerTracking

Currently supporting:
- PapyrusCS Marker file (playersData.js)
- Unmined Marker File (custom.marker.js)

GET /playerpositions -> Returns all player positions in db

GET /playerpositions/{name}  -> Returns specified player position in db

POST /playerposition  -> Add or Update the specified player position in db

POST /playerpositions  -> Adds an array of Player Positions (**Deletes all existing before**)
