using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SocialMedia.Api.Responses;
using SocialMedia.Core.DTOs;
using SocialMedia.Core.Entities;
using SocialMedia.Core.Interfaces;
using SocialMedia.Core.QueryFilters;
using SocialMedia.Core.CustomEntities;
using SocialMedia.Infrastructure.Repositories;
using SocialMedia.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace SocialMedia.Api.Controllers
{
    [Authorize]
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class PostController : ControllerBase
    {
        private readonly IPostService _postService;
        private readonly IMapper _mapper;
        private readonly IUriService _uriService;

        public PostController(IPostService postService, IMapper mapper, IUriService uriService)
        {
            _uriService = uriService;
            _mapper = mapper;
            _postService = postService;
        }

        /// <summary>
        /// Retrieve all posts
        /// </summary>
        /// <returns>List of Post</returns>
        /// <param name="filters">Filters to apply</param>
        /// <remarks>Informacion This section see information posts</remarks>
        /// <response code="200">Success</response>
        /// <response code="400">BadRequest</response>
        /// <returns></returns>
        [HttpGet(Name = nameof(GetPosts))]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ApiResponse<IEnumerable<PostDto>>))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public IActionResult GetPosts([FromQuery] PostQueryFilter filters)
        {
            var posts = _postService.GetPosts(filters);
            var postsDtos = _mapper.Map<IEnumerable<PostDto>>(posts);
            // var postsDto = posts.Select(x => new PostDto
            // {
            //     PostId = x.PostId,
            //     Date = x.Date,
            //     Description = x.Description,
            //     Image = x.Image,
            //     UserId = x.UserId
            // });
            var metadata = new Metadata
            {
                TotalCount = posts.TotalCount,
                PageSize = posts.PageSize,
                CurrentPage = posts.CurrentPage,
                TotalPages = posts.TotalPages,
                HasNextPage = posts.HasNextPage,
                HasPreviousPage = posts.HasPreviousPage,
                NextPageUrl = _uriService.GetPostPaginationUri(filters, Url.RouteUrl(nameof(GetPosts))).ToString(),
                PreviousPageUrl = _uriService.GetPostPaginationUri(filters, Url.RouteUrl(nameof(GetPosts))).ToString()
            };

            var response = new ApiResponse<IEnumerable<PostDto>>(postsDtos)
            {
                Meta = metadata
            };
            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));
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