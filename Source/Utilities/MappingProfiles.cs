using PokeAPI.DTOs;
using PokeAPI.Models;
using AutoMapper;

namespace PokeAPI.Utilities
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Pokemon, PokemonDTO>();
            CreateMap<PokemonDTO, Pokemon>();

            CreateMap<Category, CategoryDTO>();
            CreateMap<CategoryDTO, Category>();

            CreateMap<Region, RegionDTO>();
            CreateMap<RegionDTO, Region>();

            CreateMap<Owner, OwnerDTO>();
            CreateMap<OwnerDTO, Owner>();

            CreateMap<Review, ReviewDTO>();
            CreateMap<Review, ReviewDTO>().ReverseMap();

            CreateMap<Reviewer, ReviewerDTO>();
            CreateMap<Reviewer, ReviewerDTO>().ReverseMap();
        }
    }
}