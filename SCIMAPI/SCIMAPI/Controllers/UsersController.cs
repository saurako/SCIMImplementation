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

        [HttpGet]
        public IEnumerable<User> GetAllUsers()
        {
            return db.Users.ToList();
        }

        //GET /users/{id}
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

        //GET /users/?username=email@contoso.com
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
            if (ModelState.IsValid)
            {
                //create the new user
                user.Id = Guid.NewGuid();
                db.Users.Add(user);
                db.SaveChanges();
            }    
            
            //TODO: update user if ID is found in table.

                       
            var response = Request.CreateResponse<User>(HttpStatusCode.Created, user);

            string uri = Url.Link("DefaultApi", new { id = user.Id });
            response.Headers.Location = new Uri(uri);
            return response;            
        }
        
    }
}
