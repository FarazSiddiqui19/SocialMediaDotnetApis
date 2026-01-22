using Microsoft.AspNetCore.Mvc;
using SocialMedia.Data;
using SocialMedia.DTO.PostReaction;
using SocialMedia.DTO.Posts;
using SocialMedia.DTO.Users;
using SocialMedia.mappers;
using SocialMedia.models;
using SocialMedia.Models;
using SocialMedia.Services;
using SocialMedia.Services.Interfaces;

namespace SocialMedia.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BulkCreateController : ControllerBase
    {
       
        private readonly IPasswordHasherService _passwordHasher;
        private readonly SocialContext _context;

        public BulkCreateController(IPasswordHasherService passwordHasher,SocialContext context)
        {
            _passwordHasher = passwordHasher;
            _context = context;
        }

        [HttpPost("/CreateUsers")]
        public async Task<bool> CreateUsers([FromBody] List<CreateUserDTO> Users)
        {

           List<User> users = new List<User>();

             foreach (var userDto in Users)
             {
                var hashedPassword = _passwordHasher.HashPassword(userDto.Password);
                users.Add(userDto.ToEntity(hashedPassword));
            }

            _context.Users.AddRange(users);
            return true;



        }

        [HttpPost("/CreatePosts")]
        public async Task CreatePosts([FromBody] List<PostInput> input)
        {
            List<Post> Posts = new List<Post>();
            foreach (var userPosts in input)
            {
                
                Guid UserId = _context.Users.Where(u=>u.Email==userPosts.email).Select(u=>u.Id).FirstOrDefault();
               
                foreach (var postDto in userPosts.Posts)
                {
                    var post = postDto.ToEntity(UserId);
                    Posts.Add(post);
                }

            }

            await _context.Posts.AddRangeAsync(Posts);
            await _context.SaveChangesAsync();
        }


        [HttpPost("/CreateFriends")]
        public async Task CreateFriends([FromBody] List<FriendRequestInput> input)
        {
            List<FriendRequest> FriendRequests = new List<FriendRequest>();

            foreach (var friendRequest in input)
            {
                
               

                Guid RecieverId = _context.Users.Where(u => u.Email == friendRequest.Reciever)
                    .Select(u => u.Id).FirstOrDefault();
                List<Guid> SenderIds = new List<Guid>();
                foreach (var senderEmail in friendRequest.Senders)
                {
                    var SenderQuery =  _context.Users
                        .Where(u => u.Email == senderEmail)
                        .Select(u => u.Id);
                        
                    var FriendRequestEntity = new FriendRequest
                    {
                        SenderId = SenderQuery.FirstOrDefault(),
                        RecieverId = RecieverId,
                        status = Status.Pending
                    };

                    FriendRequests.Add(FriendRequestEntity);
                }
                await _context.FriendRequest.AddRangeAsync(FriendRequests);
                await _context.SaveChangesAsync();
            }


        }

        [HttpPost("/CreateReactions")]
        public async Task CreateReactions([FromBody] List<ReactionInput> input)
        {
            List<PostReaction> Reactions = new List<PostReaction>();

            foreach (var entry in input)
            {

                     
                Guid PostId = _context.Posts
                    .Where(p => p.Title.ToLower() == entry.Title.ToLower())
                    .Select(p => p.Id)
                    .FirstOrDefault();

                foreach (var reactionDto in entry.reactions)
                {
                    Guid UserId = _context.Users
                        .Where(u => u.Email.ToLower() == reactionDto.email.ToLower())
                        .Select(u => u.Id)
                        .FirstOrDefault();

                    var reaction = new PostReaction
                    {
                        PostId = PostId,
                        UserId = UserId,
                        Type = reactionDto.Type,
                        CreatedAt = DateTime.UtcNow
                    };
                    Reactions.Add(reaction);
                }
            }

            await _context.PostReactions.AddRangeAsync(Reactions);
            await _context.SaveChangesAsync();

        }



        public class FriendRequestInput
        {
            public string Reciever { get; set; }


            public List<string> Senders { get; set; }




        }

        public class PostInput
        {

            public string email { get; set; }

            public List<CreatePostDTO> Posts { get; set; }
        }


        public class ReactionInput
        {
            public string Title { get; set; }

            public List<Reaction> reactions { get; set; }


        }

        public class Reaction
        {
            public string email { get; set; }

            public ReactionType Type { get; set; }
        }


    }
}



