using AutoMapper;
using MWL.ReviewService.Api.DTOs;
using MWL.ReviewService.Domain.Entities;
using MWL.ReviewService.Domain.Models;

namespace MWL.ReviewService.Api.Mappers
{
    public class ReviewMapper : Profile
    {
        public ReviewMapper()
        {
            CreateMap<ReviewFilterDTO, ReviewFilter>();

            CreateMap<Review, ReviewResponseDTO>();

            CreateMap<Pagination<Review>, PaginationDTO<ReviewResponseDTO>>()
                .AfterMap((src, dest, context) =>
                {
                    dest.Result = context.Mapper.Map<IEnumerable<ReviewResponseDTO>>(src.Result);
                });

            CreateMap<ReviewRequestDTO, Review>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore());
        }
    }
}
