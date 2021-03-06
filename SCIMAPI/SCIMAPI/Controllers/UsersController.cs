﻿using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using SCIMAPI.Models;

using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;

namespace SCIMAPI.Controllers
{
    public class UsersController : ApiController
    {
        private UsersContext db = new UsersContext();

        /// <summary>
        /// GET /api/users 
        /// </summary>
        /// <returns>
        /// JSON array with each member being a user from the database.
        /// </returns>        
        [HttpGet]
        public IEnumerable<User> GetAllUsers()
        {
            return db.Users.ToList();
        }

        /// <summary>
        /// GET /api/users/{id}
        /// </summary>
        /// <param name="id"> Guid of the form xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx</param>
        /// <returns>JSON representation of User with id == supplied id</returns>
        [HttpGet]   
        public User GetUserById(Guid id)
        {
            var user = db.Users.Find(id);
            if (user == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound); 
            }
            return user;
        }

        
        /// <summary>
        /// GET /api/users/?username=email@contoso.com
        /// </summary>
        /// <param name="userName">UserName of the form email@contoso.com</param>
        /// <returns>JSON representation of User with userName == email@contoso.com</returns>
        [HttpGet]   
        public User GetUserByUserName(String userName)
        {
            var find = from u in db.Users
                       where (u.userName == userName)
                       select u;

            User user = find.First<User>();

            if (user == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            return user;

        }   
        
         
        /// <summary>
        /// create resource using POST
        /// POST /api/users/        
        /// </summary>
        /// <param name="user"> The JSON in the following format is bound to the parameter "user"
        ///Accepts a JSON object of the form:
        ///{
        ///     {
        ///          "Id":"null",
        ///          "userName":"user4@contoso.com",
        ///          "displayName":"User Four",
        ///          "active": false
        ///      }
        ///}
        /// </param>
        /// <returns>Response: responds with the URI that has the Location of the new resource. E.g. /api/users/{id-of-new-resource}. 
        /// If the user already exists, then the URI of the existing user is returned. 
        /// No updates are performed, if the user already exists.
        /// 
        /// </returns>
        [HttpPost]
        public HttpResponseMessage PostUser(SCIMAPI.Models.User user)
        {
            if (user != null)
            { 
                //user is not null, as expected. Check if we already have an existing user with the same user name. If yes, return the existing user.
                var existingUser = (from u in db.Users
                                    where (u.userName == user.userName)
                                    select u).ToList<User>();

                //did not find user with the user name.
                if (existingUser.Count == 0)
                {
                    try
                    {
                        user.Id = Guid.NewGuid();
                        db.Users.Add(user);
                        db.SaveChanges();

                        var response = Request.CreateResponse<User>(HttpStatusCode.Created, user);

                        string uri = Url.Link("DefaultApi", new { id = user.Id });
                        response.Headers.Location = new Uri(uri);
                        return response;
                    }
                    catch (Exception e)
                    {
                        var response = Request.CreateResponse(HttpStatusCode.BadRequest);
                        return response;
                    }
                }

                else
                {
                    var response = Request.CreateResponse<User>(HttpStatusCode.Found, existingUser.First<User>());

                    string uri = Url.Link("DefaultApi", new { id = existingUser.First<User>().Id });
                    response.Headers.Location = new Uri(uri);
                    return response;
                }              
                
            }

            //the passed in user is null
            else
            {
                var response = Request.CreateResponse(HttpStatusCode.BadRequest);                    
                return response;
                
            
            }
        }


        /// <summary>
        /// update user
        /// PUT api/users/
        /// </summary>
        /// <param name="user"> 
        /// The JSON in the following format is bound to the parameter "user"
        ///Accepts a JSON object of the form:
        ///{
        ///     {
        ///          "Id":"Guid",
        ///          "userName":"user4@contoso.com",
        ///          "displayName":"User Four",
        ///          "active": false/true
        ///      }
        ///}
        /// </param>
        /// <returns>
        /// URI to user if user exists and is updated. Else "user not found".
        /// </returns>
        [HttpPut]
        public HttpResponseMessage PutUser(User user)
        {
            //Requires a valid user.Id to be sent in.
            if (user != null)
            {
                var usersToUpdate = (from u in db.Users
                                    where (u.Id == user.Id)
                                    select u).ToList<User>();
                
                //if user is found
                if (usersToUpdate.Count > 0)                
                {
                    try
                    {
                        User userToUpdate = usersToUpdate.First<User>();

                        userToUpdate.userName = user.userName;
                        userToUpdate.displayName = user.displayName;
                        userToUpdate.active = user.active;

                        db.Entry(userToUpdate).State = EntityState.Modified;
                        db.SaveChanges();

                        var response = Request.CreateResponse<User>(HttpStatusCode.Found, userToUpdate);

                        string uri = Url.Link("DefaultApi", new { id = userToUpdate.Id });
                        response.Headers.Location = new Uri(uri);
                        return response;
                    }
                    catch (Exception e)
                    {
                        var response = Request.CreateResponse(HttpStatusCode.BadRequest);
                        return response;
                    }
                }

                //if user not found
                else
                {
                    var response = Request.CreateResponse<String>(HttpStatusCode.NotFound, "User not found.");
                    return response;
                }              

            }

            //invalid user object
            else
            {
                var response = Request.CreateResponse<String>(HttpStatusCode.NotFound, "Invalid user object. Cannot update user");
                return response;
            }

        }

        /// <summary>
        /// DELETE /api/users/{id}
        /// </summary>
        /// <param name="id">Guid of the user to be deleted</param>
        /// <returns>200 ok on successful delete. Else 404 resource not found.</returns>

        [HttpDelete]
        public HttpResponseMessage DeleteUser(Guid id)
        {
            var response = Request.CreateResponse<Guid>(HttpStatusCode.NotFound, id);

            var user = db.Users.Find(id);            
            if (user != null)
            {
                db.Users.Remove(user);
                db.SaveChanges();

                //return OK if found and deleted successfully
                response = Request.CreateResponse<Guid>(HttpStatusCode.OK, id);              
            }
            
            return response;
        }
        
    }
}
