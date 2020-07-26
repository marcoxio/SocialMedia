using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SocialMedia.Api.Responses;
using SocialMedia.Core.DTOs;
using SocialMedia.Core.Entities;
using SocialMedia.Core.Interfaces;
using SocialMedia.Infrastructure.Repositories;

namespace SocialMedia.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostController : ControllerBase
    {
        private readonly IPostService _postService;
        private readonly IMapper _mapper;

        public PostController(IPostService postService, IMapper mapper)
        {
            _mapper = mapper;
            _postService = postService;
        }
        [HttpGet]
        public async Task<IActionResult> GetPosts()
        {
            var posts = await _postService.GetPosts();
            var postsDto = _mapper.Map<IEnumerable<PostDto>>(posts);
            var response = new ApiResponse<IEnumerable<PostDto>>(postsDto);
            // var postsDto = posts.Select(x => new PostDto
            // {
            //     PostId = x.PostId,
            //     Date = x.Date,
            //     Description = x.Description,
            //     Image = x.Image,
            //     UserId = x.UserId
            // });
            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPost(int id)
        {
            var post = await _postService.GetPost(id);
            var postDto = _mapper.Map<PostDto>(post);
            var response = new ApiResponse<PostDto>(postDto);
            // var postDto = new PostDto
            // {
            //     PostId = post.PostId,
            //     Date = post.Date,
            //     Description = post.Description,
            //     Image = post.Image,
            //     UserId = post.UserId
            // };
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> CreatePost(PostDto postDto)
        {
            var post = _mapper.Map<Post>(postDto);
            // var post = new Post
            // {
            //     Date = postDto.Date,
            //     Description = postDto.Description,
            //     Image = postDto.Image,
            //     UserId = postDto.UserId
            // };
            await _postService.CreatePost(post);

            postDto = _mapper.Map<PostDto>(post);
            var response = new ApiResponse<PostDto>(postDto);
            return Ok(response);

        }


        [HttpPut]
          public async Task<IActionResult> EditPost(int id, PostDto postDto)
        {

            var post = _mapper.Map<Post>(postDto);
            post.Id = id;
        
            var result = await _postService.UpdatePost(post);
            var response = new ApiResponse<bool>(result);
            return Ok(response);

        }

          [HttpDelete("{id}")]
          public async Task<IActionResult> DeletePost(int id)
        {
            
            var result = await _postService.DeletePost(id);
            var response = new ApiResponse<bool>(result);
            return Ok(response);

        }
    }
}