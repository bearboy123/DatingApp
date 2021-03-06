using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using DatingApp.API.Data;
using DatingApp.API.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using DatingApp.API.Helpers;
using DatingApp.API.Models;

namespace DatingApp.API.Controllers
{
    [ServiceFilter(typeof(LogUserActivity))]
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IMapper mapper;

        private readonly IDatingRepository repo;
        public UsersController(IDatingRepository repo, IMapper mapper)
        {
            this.mapper = mapper;
            this.repo = repo;
        }

        [HttpGet]
        public async Task<IActionResult> GetUsers([FromQuery]UserParams userParams)
        {
            var currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var userFromRepo = await repo.GetUser(currentUserId);
            userParams.UserId = currentUserId;
            if (string.IsNullOrEmpty(userParams.Gender))
            {
                userParams.Gender = userFromRepo.Gender == "male" ? "female" : "male";
            }
            var users = await repo.GetUsers(userParams);
            var usersToReturn = mapper.Map<IEnumerable<UserForListDto>>(users);
            Response.AddPagination(users.CurrentPage, users.PageSize, users.TotalCount, users.TotalPages);
            return Ok(usersToReturn);
        }

        [HttpGet("{id}", Name = "GetUser")]
        public async Task<IActionResult> GetUser(int id)
        {
            var user = await repo.GetUser(id);
            var userToReturn = mapper.Map<UserForDetailedDto>(user);
            return Ok(userToReturn);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, UserForUpdateDto userForUpdateDto)
        {
            if (id != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            var userFromRepo = await repo.GetUser(id);
            mapper.Map(userForUpdateDto, userFromRepo);
            if (await repo.SaveAll())
                return NoContent();

            throw new System.Exception($"Update user {id} failed on save");
        }

        [HttpPost("{id}/like/{recipientId}")]
        public async Task<IActionResult> LikeUser(int id,int recipientId){
            if(id != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value)){
                return Unauthorized();
            }

            var like = await repo.GetLike(id,recipientId);
            if(like !=null)
            return BadRequest("You already like this user.");

            if(await repo.GetUser(recipientId)==null)
            return NotFound();

            like = new Like{
                LikerId =id,
                LikeeId =recipientId
            };

            this.repo.Add<Like>(like);

            if(await repo.SaveAll())
            return Ok();

            return BadRequest("Failed to like user");

        }

    }
}