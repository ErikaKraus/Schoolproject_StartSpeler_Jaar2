using API.Data.Repository;
using API.Models;

namespace API.Data.UnitofWork
{
    public interface IUnitofWork
    {
        IGenericRepository<Artikel> ArtikelRepository { get; }
        IGenericRepository<Bestelling> BestellingRepository { get; }
        IGenericRepository<BestellingLijn> BestellingLijnRepository { get; }
        IGenericRepository<Community> CommunityRepository { get; }
        IGenericRepository<Evenement> EvenementRepository { get; }
        IGenericRepository<EventGebruiker> EventGebruikerRepository { get; }
        IGenericRepository<Gebruiker> GebruikerRepository { get; }

        public Task SaveChangesAsync();


    }
}
