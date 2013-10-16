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
        //fake data
        public static List<User> users = new List<User> {
            new User {Id = Guid.NewGuid(), userName = "user1@contoso.com", displayName = "User One", active = true},
            new User {Id = Guid.NewGuid(), userName = "user2@contoso.com", displayName = "User Two", active = true},
            new User {Id = Guid.NewGuid(), userName = "user3@contoso.com", displayName = "User Three", active = false}
        };


        [HttpGet]        
        public IEnumerable<User> GetAllUsers()
        {
            return users;
        }

        //GET /users/{id}
        [HttpGet]   
        public User GetUserById(Guid id)
        {
            var user = users.FirstOrDefault((u) => u.Id == id);
            if (user == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            return user;
        }

        //GET /users/?username=email@contoso.com
        [HttpGet]   
        public User GetUserByUserName(String userName)
        {
            var user = users.FirstOrDefault((u) => u.userName == userName);
            if (user == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            return user;

        }  


        //POST /users/
        /*
         * Accepts a JSON object of the form:
         * {
         *      {
         *           "Id":"null / GUID",
         *           "userName":"user4@contoso.com",
         *           "displayName":"User Four",
         *           "active": false
         *       }
         * }
         * if the Id sent in is null, then this will create a new user. If the ID sent in is that of an exisiting user, then the user will be udpated.
         */

        [HttpPost]
        public HttpResponseMessage PostUser(SCIMAPI.Models.User user)
        {
            User newUser;

            if (user.Id.ToString() == "null")
            {
                //create the user
                newUser = new User { Id = Guid.NewGuid(), userName = user.userName, active = user.active, displayName = user.displayName };
                users.Add(newUser);                
            }
            else {
                //Get the existing user form teh data store
                


                newUser = user;
            }

            
            var response = Request.CreateResponse<User>(HttpStatusCode.Created, newUser);

            string uri = Url.Link("DefaultApi", new { id = newUser.Id });
            response.Headers.Location = new Uri(uri);
            return response;
        }
        
    }
}
