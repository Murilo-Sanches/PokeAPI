using PokeAPI.DAOs;
using PokeAPI.Data;
using PokeAPI.Models;
using AutoMapper;

namespace PokeAPI.Repositories
{
    public class RegionRepository : IRegionDAO
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public RegionRepository(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public bool RegionExists(int regionId)
        {
            return _context.Regions.Any((region) => region.Id == regionId);
        }

        public bool CreateRegion(Region region)
        {
            _context.Add(region);
            return Save();
        }

        public bool DeleteRegion(Region region)
        {
            _context.Remove(region);
            return Save();
        }

        public ICollection<Region> GetRegions()
        {
            return _context.Regions.ToList();
        }

        public Region GetRegion(int regionId)
        {
            return _context.Regions.Where((region) => region.Id == regionId).FirstOrDefault();
        }

        public Region GetRegionByOwner(int ownerId)
        {
            return _context.Owners.Where((owner) => owner.Id == ownerId)
                                  .Select((owner) => owner.Region)
                                  .FirstOrDefault();
        }

        public ICollection<Owner> GetOwnersFromARegion(int regionId)
        {
            return _context.Owners.Where((owner) => owner.Region.Id == regionId).ToList();
        }

        public bool Save()
        {
            int saved = _context.SaveChanges();
            return saved > 0;
        }

        public bool UpdateRegion(Region region)
        {
            _context.Update(region);
            return Save();
        }
    }
}