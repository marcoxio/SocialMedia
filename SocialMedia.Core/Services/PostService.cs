using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using SocialMedia.Core.Entities;
using SocialMedia.Core.Interfaces;

namespace SocialMedia.Core.Services
{
    public class PostService : IPostService
    {
        private readonly IUnitOfWork _unitOfWork;
        public PostService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

      

        public async Task<Post> GetPost(int id)
        {
            return await _unitOfWork.PostRepository.GetById(id);
        }

        public async Task<IEnumerable<Post>> GetPosts()
        {
            return await _unitOfWork.PostRepository.GetAll();
        }

        public async Task CreatePost(Post post)
        {

            var user = await _unitOfWork.UserRepository.GetById(post.UserId);
             if (user == null)
            {
                throw new Exception("User doesn't exist");
            }
            bool contains = Regex.IsMatch(post.Description, @"\bSexo\b", RegexOptions.IgnoreCase);
             if (contains)
            {
                throw new Exception("Content not allowed");
            }
            await _unitOfWork.PostRepository.Add(post);
        }

        public async Task<bool> UpdatePost(Post post)
        {
            await _unitOfWork.PostRepository.Update(post);
            return true;
        }

          public async Task<bool> DeletePost(int id)
        {
            await _unitOfWork.PostRepository.Delete(id);
            return true;
        }
    }
}