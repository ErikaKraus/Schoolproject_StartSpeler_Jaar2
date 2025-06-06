using Dapper;
using Kassa.Models;
using Microsoft.Data.SqlClient;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kassa.Data.Repository
{
    public class EventsRepository:BaseRepository, IEventsRepository
    {
        public IEnumerable<Evenement> OphalenEvents()
        {
            string sql = @"SELECT * 
                        FROM evenementen e
                        ORDER BY datum";
            using (IDbConnection db = new MySqlConnection(ConnectionString))
            {
                var evenementen = db.Query<Evenement>(sql);
                foreach (var evenement in evenementen)
                {
                    Console.WriteLine($"Evenement: {evenement.Naam}, Startuur: {evenement.Startuur}, Einduur: {evenement.Einduur}");
                }
                return evenementen;
            }
        }

     
        public IEnumerable<Evenement> OphalenEvenementenVoorCommunity(int communityId)
        {
            string sql = "SELECT * FROM evenementen WHERE CommunityId = @CommunityId";
            using (IDbConnection db = new MySqlConnection(ConnectionString))
            {
                return db.Query<Evenement>(sql, new { CommunityId = communityId });
            }
        }

        public bool ToevoegenEvenement(Evenement evenement)
        {
            string sql = @"INSERT INTO evenementen (naam, datum, startuur, einduur, kostprijs, communityId, maxdeelnemersevent, extrainfo) 
            VALUES (@naam, @datum, @startuur, @einduur, @kostprijs, @communityId, @maxdeelnemersevent, @extraInfo)";

            var parameters = new
            {
                naam = evenement.Naam,
                datum = evenement.Datum,
                startuur = evenement.Startuur,
                einduur = evenement.Einduur,
                kostprijs = evenement.Kostprijs,
                communityId = evenement.CommunityId,
                maxdeelnemersevent = evenement.MaxDeelnemersEvent,
                extraInfo = evenement.ExtraInfo
            };

            using IDbConnection db = new MySqlConnection(ConnectionString);
            var affectedRows = db.Execute(sql, parameters);

            return affectedRows == 1;
        }

        public bool VerwijderenEvenement(int id)
        {
            string sql = "DELETE FROM evenementen WHERE id = @id";
            using IDbConnection db = new MySqlConnection(ConnectionString);
            var affectedRows = db.Execute(sql, new { id });
            return affectedRows == 1;
        }

        public bool BewerkenEvenement(Evenement evenement)
        {
            string sql = @"UPDATE evenementen
                        SET naam = @naam,
                            datum = @datum,
                            startuur = @startuur,
                            einduur = @einduur,
                            kostprijs = @kostprijs,
                            communityid = @communityid,
                            maxdeelnemersevent = @maxdeelnemersevent,
                            extrainfo = @extrainfo
                        WHERE id = @evenementid";

            var parameters = new
            {
                evenementid = evenement.Id,
                naam = evenement.Naam,
                datum = evenement.Datum,
                startuur = evenement.Startuur,
                einduur = evenement.Einduur,
                kostprijs = evenement.Kostprijs,
                communityid = evenement.CommunityId,
                maxdeelnemersevent = evenement.MaxDeelnemersEvent,
                extrainfo = evenement.ExtraInfo
            };

            using IDbConnection db = new MySqlConnection(ConnectionString);
            var affectedRows = db.Execute(sql, parameters);

            return affectedRows == 1;
        }

        public bool DuplicerenEvenement(Evenement evenement)
        {
            string sql = @"INSERT INTO evenementen (naam, datum, startuur, einduur, kostprijs, communityId, maxdeelnemersevent, extrainfo) 
            VALUES (@naam, @datum, @startuur, @einduur, @kostprijs, @communityId, @maxdeelnemersevent, @extraInfo)";

            var parameters = new
            {
                naam = evenement.Naam,
                datum = evenement.Datum,
                startuur = evenement.Startuur,
                einduur = evenement.Einduur,
                kostprijs = evenement.Kostprijs,
                communityId = evenement.CommunityId,
                maxdeelnemersevent = evenement.MaxDeelnemersEvent,
                extraInfo = evenement.ExtraInfo
            };

            using IDbConnection db = new MySqlConnection(ConnectionString);
            var affectedRows = db.Execute(sql, parameters);

            return affectedRows == 1;
        }
    }
}
