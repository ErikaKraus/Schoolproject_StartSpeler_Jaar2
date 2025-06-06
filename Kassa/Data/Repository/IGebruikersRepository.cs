using Kassa.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kassa.Data.Repository
{
    public interface IGebruikersRepository
    {
        public IEnumerable<Gebruiker> OphalenGebruikersZoekterm(string zoekterm);

        public IEnumerable<Gebruiker> OphalenGebruikersUsername(string username);
        public bool VoegGebruikerToe(Gebruiker gebruiker);

        public IEnumerable<Gebruiker> AlleGebruikersOphalen();

        public Task<bool> BestaatGebruiker(string gebruikerId);

        public bool PasswordReset(Gebruiker gebruiker,string newpassword);


    }
}
