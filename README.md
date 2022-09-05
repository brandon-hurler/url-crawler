# url-crawler
This repository contains a URL crawler to display images in a carousel and analytics on contained text. The crawler allows a user to type a URL into an input box, from which images and text will be fetched.

All images on the URL will be shown in a carousel.
All text on the URL will be counted with the total displayed and the top 10 (default) occuring words and their respective totals will be shown, with the capability to increase/decrease number of top occurring words.

## Running the Application
1. Download and install .NET 6.0 LTS runtime (ASP.NET Core Runtime 6.0.8+) for your OS: https://dotnet.microsoft.com/en-us/download/dotnet/6.0.
2. Open your local repositories directory or create one.
3. Using your terminal of choice (PowerShell, CMD, Terminal, etc.), navigate to your repository folder.
4. Clone this repo using 'git clone https://github.com/brandon-hurler/url-crawler.git'
5. Navigate to %YOUR_REPOSITORY_PATH%\url-crawler\URL-Crawler in your terminal.
6. Run the command 'dotnet run URL-Crawler.dll'
7. Browse the URL on localhost with the port provided in the terminal, e.g. if it says "Now listening on: https://localhost:7110", copy this URL and paste it in your browser.
