using SocialMedia.Core.CustomEntities;

namespace SocialMedia.Api.Responses
{
    public class ApiResponse<T>
    {
        public T Data { get; set; }
        public Metadata Meta { get; set; }
        public ApiResponse(T data)
        {
            Data = data;
        }
    }
}