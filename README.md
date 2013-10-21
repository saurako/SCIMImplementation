SCIMImplementation
==================
This is prototype implementation of the SCIM API (http://www.simplecloud.info/), in ASP.NET MVC4. 
Currently the prototype defines the following REST APIs. 

REST METHOD       URL                 DATA PASSED / RESULT
-----------       -------------       -------------------------------------------
GET               /api/users/         Returns JSON array of users in the database
GET               /api/users/{id}     Returns JSON object of user with Id == {id}
PUT               /api/users/         Expects a JSON object represnting user information. Responds with URI of newly created user
DELETE            /api/users/{id}     Deletes the user with Id == {id}

More to be added:
1. Get OAuth 2.0 Bearer token
2. APIs defined in the complete spec here: http://tools.ietf.org/html/draft-ietf-scim-api-02
