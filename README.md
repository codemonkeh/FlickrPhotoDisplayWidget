# FlickrPhotoDisplayWidget
Simple project to display a flickr photo as a widget within a website using AWS Lambda and S3

The widget will comprise of two parts:
	• A simple HTML/JS Web App to display the photo and link to my photostream
	• An AWS Lambda to connect to flickr, download and resize the photo and store it in an S3 bucket.

Web App
The website will be a simple bootstrap web app to display the widget.

Widget
The widget will be an element in the web app. 
It will be comprised of a single photo display and a link to my profile. 
The link to my profile will be statically linked directly to my photostream.
The photo will be statically linked to a publicly hosted photo in an AWS S3 bucket.
The photo is presumed to be the correct size and will be displayed at original size.
The photo will be statically linked to my photostream.

AWS Lambda
The lambda will be written in C#.
The lambda will be scheduled on an infrequent schedule, such as once a day.
The lambda will download the last photo in my photostream, sorted by uploaded date descendingly, and store it in a temporary directory.
The photo will be processed, resizing it but preserving EXIF data (e.g. copyright), and saved to the temporary directory.
The photo will be stored at a configured location in an S3 bucket once processed.
All temporary files will use unique filenames.
The date of the last execution will be stored. This date will be compared to the last file upload date, if later the file will not be downloaded and processed