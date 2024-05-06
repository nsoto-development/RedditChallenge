As a developer skill assessment, the task was to choose relevant languages, tools, libraries and utilities to integrate with Reddit API and track two things: 

For a given subreddit of my choice:
1) Get posts with the most upvotes  
2) Of those posts, get authors with the most posts
   
For the task, I chose asp.net core as it seemed a potentially relevant choice for the position.  

This project utilized dotenv (.env) for configuration/secrets. I'm used to using it with node.js, and was curious to see how easily it would be to also utilize it in this project.

As such, please refer to 'example.env' to supply your reddit credentials for use with the project. After supplying the credentials, please rename the file  '.env' after saving credentials. The application will utilize '.env' during runtime.
