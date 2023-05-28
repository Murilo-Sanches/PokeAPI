using PokeAPI.Models;

namespace PokeAPI.DAOs
{
    public interface IRegionDAO
    {
        ICollection<Region> GetRegions();

        Region GetRegion(int regionId);

        Region GetRegionByOwner(int ownerId);

        ICollection<Owner> GetOwnersFromARegion(int regionId);

        bool RegionExists(int regionId);

        bool CreateRegion(Region region);

        bool Save();

        bool UpdateRegion(Region region);

        bool DeleteRegion(Region region);
    }
}