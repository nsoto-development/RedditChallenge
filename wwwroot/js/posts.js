"use strict";

const connection = new signalR.HubConnectionBuilder()
    .withUrl("/postsHub")
    .build();


connection.start()
    .then(() => {
        console.log("Connection started. Listening for new posts...");
        // Start streaming new posts from the server

        // get the subreddit name from the URL
        const subreddit = window.location.pathname.split("/")[2];

        connection.stream("StreamNewPostsAsync", subreddit).subscribe({
            next: post => {
                console.log("Received new post:", post);
                const postList = document.getElementsByClassName("post-list")[0];

                const postInnerHtml = `
                  <li class="post-item">
                      <h3 class="post-title">${post.title}</h3>
                      <div class="post-details">
                          <span class="post-author">Author: ${post.author}</span>
                          <span class="post-score">Score: ${post.score}</span>
                      </div>
                  </li>
                `;

                postList.innerHTML = postInnerHtml + postList.innerHTML;
            },
            complete: () => {
                console.log("Streaming of new posts completed.");
            },
            error: error => {
                console.error("Error streaming new posts:", error);
            }
        });
    })
    .catch(err => console.error(err));