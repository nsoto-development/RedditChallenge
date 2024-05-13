As a developer skill assessment, the task was to choose relevant languages, tools, libraries and utilities to integrate with Reddit API and track two things: 

For a given subreddit of my choice:
1) Get posts with the most upvotes  
2) Of those posts, get authors with the most posts
   
For the task, I chose asp.net core as it seemed a potentially relevant choice for the position.  

This project utilized dotenv (.env) for configuration/secrets. I'm used to using it with node.js, and was curious to see how easy it would be to also utilize it in this project.

As such, please refer to 'example.env' to supply your Reddit credentials for use with the project. After supplying the credentials, please "save as..." and set the new file name to be simply '.env' after saving credentials. 

The end result should be a file named '.env' with the Reddit API credentials saved to the root of the project folder: 

`/.env`:
``` ## reddit api credentials
REDDIT_CLIENT_ID="YOUR CLIENT ID"
REDDIT_CLIENT_SECRET="YOUR CLIENT SECRET"
REDDIT_USER_AGENT="ASP.NET Core:DevChallenge:v0.0.1 (by /u/DevChallenge)"
```
