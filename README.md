# primes
========
Author: Bogdan Nica (bogdan.nica.van@gmail.com)

Server
----------------
used Java and developed with Intelij
- Server starts by running com.bjet.primesserver.Main 
- default port open 8090
- default port can be overriden if the server is open in command line with desired port number as argument.

To check if the server started call from browser: http://<server url>:<port>/getInt 

Example calls:
http://<server url>:<port>/getInt					-> the default min is 1 the default max is 100000
you can specify min and max:
http://<server url>:<port>/getInt/min=1&max=999  	-> the response is a randoom number between 1 and 999

in case the server is not on the localhost the delay param cause the server to keep the connection open for a bit longer, allowing the client to receive the response:
http://<server url>:<port>/getInt/min=55&max=89&delay=500	-> the response is a randoom number between [55,89] and will delay the connection closing with 500 milliseconds
 
Client
----------------

