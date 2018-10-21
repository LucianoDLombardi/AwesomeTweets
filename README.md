# AwesomeTweets
Web-based solution to IQVIA bad api problem.

This solution retrieves all tweets from the IQVIA bad api from 2016 to 2017 - there is no option to select dates.
The solution displays the the tweets on a single page. The home page also includes a "random tweet" button that randomly selects a
tweet.

The solution uses the MVC pattern where the model data is loaded into an in-memory database.  The data is retrieved from the IQVIA bad 
api at application startup in the configure method (startup.cs).

This solution also includes docker support for linux-based OS.
