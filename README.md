# YoutubeFetch
Fetch videos from a given channel

# Usage
```
YoutubeAppConsole.exe ChannelName
```
eg. <br/>
```
YoutubeAppConsole.exe Imperial
```

# Notes
1. It will fetch 32 latest videos from the given channel and display their titles in console. <br/>
2. This is the prototype of Youtube App. For further improvement, I need to integrate it with GDO. <br/>

# Problems
1.There is a limitation of number of videos that can be fetched every request (<=50), and I still haven't found an API that supports fetching videos starting from a specific videoId. Thus, I cannot get more than 50 different videos currently. <br/>
2. Integration with GDO after the SDK is ready. <br/>