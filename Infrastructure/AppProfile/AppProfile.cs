using AutoMapper;
using Models.Responses;

namespace Infrastructure.AppProfile
{
    public class AppProfile : Profile 
    {
        public AppProfile()
        {
            CreateMap<Response, ResponseDto>();
            CreateMap<ResponseDto, Response>();
        }
    }
}