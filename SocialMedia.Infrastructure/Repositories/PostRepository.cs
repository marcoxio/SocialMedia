using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SocialMedia.Core.Entities;
using SocialMedia.Core.Interfaces;
using SocialMedia.Infrastructure.Data;

namespace SocialMedia.Infrastructure.Repositories
{
    public class PostRepository : IPostRepository
    {
        private readonly SocialMediaContext _context;

        public PostRepository(SocialMediaContext context)
        {
            _context = context;
        }
        // public IEnumerable<Post> GetPosts()
        // {
        //     var posts = Enumerable.Range(1,10).Select(x => new Post
        //     {
        //         PostId = x,
        //         Description = $"Description {x}",
        //         Date = DateTime.Now,
        //         Image = $"https://misapis.com/{x}",
        //         UserId = x * 2
        //     });

        //     return posts;
        // }

        public async Task<IEnumerable<Post>> GetPosts()
        {
            // var posts = Enumerable.Range(1,10).Select(x => new Post
            // {
            //     PostId = x,
            //     Description = $"Description {x}",
            //     Date = DateTime.Now,
            //     Image = $"https://misapis.com/{x}",
            //     UserId = x * 2
            // });
            // await Task.Delay(10);
            var posts = await _context.Posts.ToListAsync();
            return posts;
        }

        public async Task<Post> GetPost(int id)
        {
            var post = await _context.Posts.FirstOrDefaultAsync(x => x.PostId == id);
            return post;
        }

        public async Task CreatePost(Post post)
        {
            await _context.Posts.AddAsync(post);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> UpdatePost(Post post)
        {
            var currentPost = await GetPost(post.PostId);
            
            currentPost.Date = post.Date;
            currentPost.Description = post.Description;
            currentPost.Image = post.Image;

            int result = await _context.SaveChangesAsync();
            return result > 0;
        }

        public async Task<bool> DeletePost(int id)
        {
            var currentPost = await GetPost(id);
            _context.Posts.Remove(currentPost);

            
            int result = await _context.SaveChangesAsync();
            return result > 0;
        }

    }
}