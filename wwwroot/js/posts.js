// get the subreddit from the path in the url
const subreddit = window.location.pathname.split("/")[2];

if (subreddit) {
  // Create a WebSocket connection
  const webSocket = new WebSocket("ws://localhost:5051/redditUpdates?subreddit=" + encodeURIComponent(subreddit));

  // When the WebSocket connection is opened
  webSocket.onopen = function () {
    console.log("WebSocket connection established.");
  };

  // When a message is received from the server
  webSocket.onmessage = function (event) {
    const updatedPosts = JSON.parse(event.data);

    // Handle updated posts and update the UI
    const postList = document.getElementsByClassName("post-list")[0];

    // prepend the new posts to the top of the list
    updatedPosts.forEach(function (post) {
      const postInnerHtml = `
                <li class="post-item">
                    <h3 class="post-title">${post.title}</h3>
                    <div class="post-details">
                        <span class="post-author">Author: ${post.author}</span>
                        <span class="post-score">Score: ${post.score}</span>
                    </div>
                </li>
                `;
      postList.insertAdjacentHTML("afterbegin", postInnerHtml);
    });
  };

  // When an error occurs with the WebSocket connection
  webSocket.onerror = function (event) {
    console.error("WebSocket error:", event);
  };

  // When the WebSocket connection is closed
  webSocket.onclose = function (event) {
    console.log("WebSocket connection closed:", event);
  };
}
