using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using SCIMAPI.Models;

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
            var user = db.Users.SqlQuery("select * from User where userName = {0}", userName).First<User>();
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
        ///          "Id":"null / GUID",
        ///          "userName":"user4@contoso.com",
        ///          "displayName":"User Four",
        ///          "active": false
        ///      }
        ///}
        /// </param>
        /// <returns>Response: responds with the URI that has the Location of the new resource. E.g. /api/users/{id-of-new-resource}</returns>

        [HttpPost]
        public HttpResponseMessage PostUser(SCIMAPI.Models.User user)
        {
            if (!ModelState.IsValid)
            {
                var existingUsers = from u in db.Users
                            where (u.userName == user.userName)
                            select u;

                List<User> users = existingUsers.ToList<User>();

                if (users.Count == 0)
                {
                    //user doesnt exist already, create the new user
                    user.Id = Guid.NewGuid();
                    db.Users.Add(user);
                    db.SaveChanges();

                    var response = Request.CreateResponse<User>(HttpStatusCode.Created, user);

                    string uri = Url.Link("DefaultApi", new { id = user.Id });
                    response.Headers.Location = new Uri(uri);
                    return response;
                }

                else
                {
                    //user already exists. Return the user existing user object.
                    User firstUser = users.First<User>();
                    var response = Request.CreateResponse<User>(HttpStatusCode.Found, firstUser);

                    string uri = Url.Link("DefaultApi", new { id = firstUser.Id });
                    response.Headers.Location = new Uri(uri);
                    return response;
                }                             
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
