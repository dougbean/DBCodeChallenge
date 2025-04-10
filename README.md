 This is a code challange I did for a job interview.

The requirements were to write a console program that will parse text files containing records that use different delimiters - "comma", "pipe" or "space".

I used the Stategy design pattern to implement the Open Closed Principle - "software entities (classes, modules, functions, etc.) should be open for extension, but closed for modification". https://en.wikipedia.org/wiki/Open%E2%80%93closed_principle 

I use "comma", "pipe" or "space" in the name of the text files to tell the parser service which strategy to use. 

The StreamReaderWrapper and FileSystemWrapper in /DBLibrary/Wrappers implement the Decorator design pattern. They allow me to abstract the StreamReader and the FileSystem so I can mock those dependencies in unit tests. https://en.wikipedia.org/wiki/Decorator_pattern

I also used the Singleton design pattern in the web api. https://en.wikipedia.org/wiki/Singleton_pattern. Some people claim that singleton is really an anti-pattern. I suppose that depends on how it is used and it is a matter for reflection. It serves my purpose here.

The requirements also specified a web api where one can post one record at a time as a json. The post request puts a record in a cache and the get request gets those cached records.

The following uri will bring up the Swagger interface:
https://localhost:{port}/swagger/index.html

Use the Swagger interface to execute POST and GET commands to DBWebAPI.

Example jsons to POST:
{ "delimiter":"comma", "line":"Gibbe,Candace,Female,Crimson,3/28/2010" }   
{ "delimiter":"space", "line":"Eltringham Nelia Female Crimson 9/5/1962" }   
{ "delimiter":"pipe", "line":"Fudd|Elmer|Male|Green|10/8/1954" }

Execute GET with the following sort parameters (don't use quotes in the Swagger interface) - 
gender,
birthdate or 
name 
