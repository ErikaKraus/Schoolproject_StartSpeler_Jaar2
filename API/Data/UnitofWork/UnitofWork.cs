using API.Data.Repository;
using API.Models;
using API.Data;

namespace API.Data.UnitofWork
{
    public class UnitofWork : IUnitofWork
    {
        private readonly StartspelerContext _context;
        private readonly ILoggerFactory _loggerFactory;


        public UnitofWork(StartspelerContext context, ILoggerFactory loggerFactory )
        {
            _context = context;
            _loggerFactory = loggerFactory;

            ArtikelRepository = new GenericRepository<Artikel>(_context, _loggerFactory.CreateLogger<GenericRepository<Artikel>>());
            BestellingRepository = new GenericRepository<Bestelling>(_context, _loggerFactory.CreateLogger<GenericRepository<Bestelling>>());
            BestellingLijnRepository = new GenericRepository<BestellingLijn>(_context, _loggerFactory.CreateLogger<GenericRepository<BestellingLijn>>());
            CommunityRepository = new GenericRepository<Community>(_context, _loggerFactory.CreateLogger<GenericRepository<Community>>());
            EvenementRepository = new GenericRepository<Evenement>(_context, _loggerFactory.CreateLogger<GenericRepository<Evenement>>());
            EventGebruikerRepository = new GenericRepository<EventGebruiker>(_context, _loggerFactory.CreateLogger<GenericRepository<EventGebruiker>>());
            GebruikerRepository = new GenericRepository<Gebruiker>(_context, _loggerFactory.CreateLogger<GenericRepository<Gebruiker>>());
        }


        public IGenericRepository<Artikel> ArtikelRepository { get; }

        public IGenericRepository<Bestelling> BestellingRepository { get; }

        public IGenericRepository<BestellingLijn> BestellingLijnRepository { get; }

        public IGenericRepository<Community> CommunityRepository { get; }

        public IGenericRepository<Evenement> EvenementRepository { get; }

        public IGenericRepository<EventGebruiker> EventGebruikerRepository { get; }

        public IGenericRepository<Gebruiker> GebruikerRepository { get; }



        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();

        }
    }
}
