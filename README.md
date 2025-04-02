This is a code challange I did for a job interview.

The console program will parse and sort comma, pipe and space delimited text files.
Specify "comma", "pipe" or "space" in the name of your text files to tell the parser service to use the corresponding characters as delimiters.

Run the following in the package manager console to install Swagger.

Install-Package NSwag.AspNetCore

The following uri will bring up the Swagger interface:
http://localhost:{port}/swagger

Use the Swagger interface to execute POST and GET commands to DBWebAPI.

Example jsons to POST:
{ "delimiter":"comma", "line":"Gibbe,Candace,Female,Crimson,3/28/2010" }   
{ "delimiter":"space", "line":"Eltringham Nelia Female Crimson 9/5/1962" }   
{ "delimiter":"pipe", "line":"Fudd|Elmer|Male|Green|10/8/1954" }

Execute GET with the following parameters (don't use quotes in the Swagger interface).
gender
birthdate
name 
