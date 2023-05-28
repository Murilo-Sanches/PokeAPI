using PokeAPI.DAOs;
using PokeAPI.Data;
using PokeAPI.Models;
using AutoMapper;

namespace PokeAPI.Repositories
{
    public class OwnerRepository : IOwnerDAO
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public OwnerRepository(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public Owner GetOwner(int ownerId)
        {
            return _context.Owners.Where((owner) => owner.Id == ownerId).FirstOrDefault();
        }

        public ICollection<Owner> GetOwners()
        {
            return _context.Owners.ToList();
        }

        public ICollection<Owner> GetOwnerOfAPokemon(int pokeId)
        {
            return _context.PokemonOwners.Where((x) => x.Pokemon.Id == pokeId)
                                         .Select((pokemon) => pokemon.Owner)
                                         .ToList();
        }

        public ICollection<Pokemon> GetPokemonByOnwer(int ownerId)
        {
            return _context.PokemonOwners.Where((x) => x.Owner.Id == ownerId)
                                         .Select((pokemon) => pokemon.Pokemon)
                                         .ToList();
        }

        public bool OwnerExists(int ownerId)
        {
            return _context.Owners.Any((owner) => owner.Id == ownerId);
        }

        public bool CreateOwner(Owner owner)
        {
            _context.Add(owner);
            return Save();
        }

        public bool Save()
        {
            int saved = _context.SaveChanges();
            return saved > 0;
        }

        public bool UpdateOwner(Owner owner)
        {
            _context.Update(owner);
            return Save();
        }

        public bool DeleteOwner(Owner owner)
        {
            _context.Remove(owner);
            return Save();
        }
    }
}