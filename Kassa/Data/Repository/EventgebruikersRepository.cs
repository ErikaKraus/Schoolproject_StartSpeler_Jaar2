using Dapper;
using Kassa.Models;
using Microsoft.Data.SqlClient;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kassa.Data.Repository
{
    public class EventGebruikersRepository: BaseRepository, IEventGebruikersRepository
    {
        public IEnumerable<EventGebruiker> AlleGebruikersVanEventOphalen(Evenement evenement)
        {
            string sql = @"SELECT * FROM startspelercompanion.eventgebruikers eg
                            INNER JOIN startspelercompanion.aspnetusers a ON eg.gebruikerid = a.id
                            INNER JOIN startspelercompanion.evenementen e ON eg.evenementid = e.id
                            WHERE eg.EvenementId = @EvenementId";

            using (IDbConnection db = new MySqlConnection(ConnectionString))
            {
                var debugVar = db.Query<EventGebruiker, Gebruiker, Evenement, EventGebruiker>(
                    sql,
                    (eventgebruiker, gebruiker, evenement) =>
                    {
                        eventgebruiker.Evenement = evenement;
                        eventgebruiker.Gebruiker = gebruiker;
                        return eventgebruiker;
                    },
                    new { EvenementId = evenement.Id },
                    splitOn: "Id"
                    );
                return debugVar;
            }
        }

        public Gebruiker HaalGebruikerOpUserName(string username)
        {
            string sql = @"SELECT * FROM startspelercompanion.aspnetusers WHERE UserName = @UserName";

            using (IDbConnection db = new MySqlConnection(ConnectionString))
            {
                try
                {
                    Debug.WriteLine($"Executing SQL: {sql} with UserName={username}");
                    var gebruiker = db.QueryFirstOrDefault<Gebruiker>(sql, new { UserName = username });
                    if (gebruiker != null)
                    {
                        Debug.WriteLine($"User found: {gebruiker.Id}, {gebruiker.UserName}");
                    }
                    else
                    {
                        Debug.WriteLine("No user found with the given username.");
                    }
                    return gebruiker;
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"SQL Error: {ex.Message}");
                    return null;
                }
            }
        }


        public bool VoegEventGebruikerToe(EventGebruiker eventGebruiker)
        {
            string sql = @"INSERT INTO startspelercompanion.eventgebruikers (GebruikerId, EvenementId, AantalDeelnemers) 
            VALUES (@GebruikerId, @EvenementId, @AantalDeelnemers)";

            using (IDbConnection db = new MySqlConnection(ConnectionString))
            {
                try
                {
                    Debug.WriteLine($"Executing SQL: {sql} with GebruikerId={eventGebruiker.GebruikerId}, EvenementId={eventGebruiker.EvenementId}, AantalDeelnemers={eventGebruiker.AantalDeelnemers}");
                    var affectedRows = db.Execute(sql, new
                    {
                        GebruikerId = eventGebruiker.GebruikerId,
                        EvenementId = eventGebruiker.EvenementId,
                        AantalDeelnemers = eventGebruiker.AantalDeelnemers
                    });

                    Debug.WriteLine($"Affected rows: {affectedRows}");
                    return affectedRows == 1;
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"SQL Error: {ex.Message}");
                    return false;
                }
            }
        }

        public bool VerwijderEventGebruiker(string gebruikerId, int evenementId )
        {
            string sql = @"DELETE FROM startspelercompanion.eventgebruikers 
                           WHERE GebruikerId = @GebruikerId AND EvenementId = @EvenementId";

            using (IDbConnection db = new MySqlConnection(ConnectionString))
            {
                var affectedRows = db.Execute(sql, new { GebruikerId = gebruikerId, EvenementId = evenementId  });
                return affectedRows == 1;
            }
        }

        

        public bool WijzigGebruiker(Gebruiker gebruiker)
        {
            string sql = @"UPDATE startspelercompanion.aspnetusers 
                   SET Voornaam = @Voornaam, Naam = @Naam 
                   WHERE Id = @Id";
            
            using (IDbConnection db = new MySqlConnection(ConnectionString))
            {
                Debug.WriteLine($"SQL Query: {sql}");
                Debug.WriteLine($"Voornaam: {gebruiker.Voornaam}, Naam: {gebruiker.Naam}, Id: {gebruiker.Id}");

                var affectedRows = db.Execute(sql, new { gebruiker.Voornaam, gebruiker.Naam, gebruiker.Id });
                Debug.WriteLine($"Aantal gewijzigde rijen: {affectedRows}");

                // Controleer of de rij daadwerkelijk is bijgewerkt
                var updatedUser = db.QueryFirstOrDefault<Gebruiker>("SELECT Voornaam, Naam FROM startspelercompanion.aspnetusers WHERE Id = @Id", new { gebruiker.Id });
                Debug.WriteLine($"Bijgewerkte gegevens: Voornaam={updatedUser.Voornaam}, Naam={updatedUser.Naam}");

                return affectedRows == 1;
            }
        }

        public bool WijzigAantalDeelnemers(Gebruiker gebruiker, int evenementId)
        {
            string sql = @"UPDATE startspelercompanion.eventgebruikers 
                   SET AantalDeelnemers = @AantalDeelnemers 
                   WHERE GebruikerId = @GebruikerId AND EvenementId = @EvenementId";

            using (IDbConnection db = new MySqlConnection(ConnectionString))
            {
                var affectedRows = db.Execute(sql, new
                {
                    AantalDeelnemers = gebruiker.AantalDeelnemers,
                    GebruikerId = gebruiker.Id,
                    EvenementId = evenementId
                });
                return affectedRows == 1;
            }
        }




    }
}
