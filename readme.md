Shocked Isaac mod
----------

OpenShock/Shocklink support for isaac.

Shocks when isaac gets hit

Intentional damage (curse, sacrefice rooms) shock aswell, but shorter


## Installation

1. Copy the client folder to "C:\Program Files (x86)\Steam\steamapps\common\The Binding of Isaac Rebirth\mods"
2. Rename the folder to shockedisaac.
3. Start the server using ```dotnet run```.
4. Insert the API token genertated from https://shocklink.net/#/dashboard/tokens into the server console app.
5. Go to the game settings of Isaac in Steam.
6. Add ```--luadebug``` to the launch options
7. Start Isaac and get shocked!

## Remarks

- This mod is the bare minimum.
- The isaac modding API is sandboxed by default. In order to communicate with shocklink / the proxy the sandbox needs to be turned off.
- There arent any settings yet. (Except the API key)
- On hit translates to 1 second 20% shock strength * (hearts lost)
- On intentional hit translates to 0.3 seconds with 100% shock strength