using Kassa.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kassa.Data.Repository
{
    public interface IEventGebruikersRepository
    {

        public IEnumerable<EventGebruiker> AlleGebruikersVanEventOphalen(Evenement evenement);

        public bool VoegEventGebruikerToe(EventGebruiker eventGebruiker);

        public Gebruiker HaalGebruikerOpUserName(string username);


    }
}
