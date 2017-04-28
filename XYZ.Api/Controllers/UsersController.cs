using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using XYZ.Api.Models;
using XYZ.Api.Services;

namespace XYZ.Api.Controllers
{
    public class UsersController : ApiController
    {
        private readonly UserService _userService;
        public UsersController()
        {
            _userService = new UserService();
        }
        public IEnumerable<User> Get()
        {
            return _userService.GetUsers();
        }

        public IHttpActionResult Post([FromBody] User user)
        {
            if(user == null)
                return BadRequest("Need to pass object with Name and Points.");

            if(string.IsNullOrEmpty(user.Name))
                return BadRequest("Name cannot be null or empty.");

           
            var result = _userService.AddUser(user);
            if (result > 0)
            {
                var response = Helper.CreateResponseMessage(HttpStatusCode.Created,
                    $"User with id {result} was added successfully.");
                var uri = Url.Link("DefaultApi", new {id = result});
                response.Headers.Location = new Uri(uri);
                return

                    ResponseMessage(response);
            }
            return
                ResponseMessage(Helper.CreateResponseMessage(HttpStatusCode.Conflict,
                    $"User with name {user.Name} already existed."));
        }

        
        public IHttpActionResult Get(int id)
        {
            var user = _userService.Get(id);
            if (user != null)
            {
                return Ok(user);
            }
            return ResponseMessage(Helper.CreateResponseMessage(HttpStatusCode.NotFound,
                    $"User with id {id} not found."));
        }

        [Route("api/setpoints")]
        public IHttpActionResult Update([FromBody] UserUpdate userUpdate)
        {
            if(userUpdate == null)
                return BadRequest("Request needs to pass object with Id and Points");

            if (_userService.Update(userUpdate.Id, userUpdate.Points))
            {
                return Ok();
            }
            return ResponseMessage(Helper.CreateResponseMessage(HttpStatusCode.NotFound,
                    $"User with id {userUpdate.Id} not found and failed to update."));
        }
    }
}
