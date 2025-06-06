using Dapper;
using Kassa.Models;
// using Microsoft.Data.SqlClient;
using MySql.Data.MySqlClient;
using Mysqlx.Crud;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Kassa.Data.Repository
{
    public class BestellingenRepository:BaseRepository, IBestellingenRepository
    {
        public IEnumerable<Bestelling> OphalenBestellingen()                            // to delete               
        {
            string sql = @"SELECT BE.*, AU.*, BL.*, AR.* FROM startspelercompanion.bestellingen BE ";
            sql += "INNER JOIN startspelercompanion.aspnetusers AU ON BE.GebruikerId = AU.Id ";
            sql += "INNER JOIN startspelercompanion.bestellijnen BL ON BE.Id = BL.BestellingId ";
            sql += "INNER JOIN startspelercompanion.artikels AR ON BL.ArtikelId = AR.Id ";
            sql += "WHERE BE.IsGeleverd = false ";
            sql += "ORDER BY BE.Tijdstip ";

            using (IDbConnection db = new MySqlConnection(ConnectionString))
            {
                var bestellingenX = db.Query<Bestelling,  BestellingLijn,  Bestelling>(
                    sql,
                    (bestelling, bestellinglijn ) =>
                    {
                        
                        bestellinglijn.Bestelling = bestelling;
                        bestelling.Bestellijnen = new List<BestellingLijn>() { bestellinglijn};
                        
                        return bestelling;
                    }
                    

                );
                return GroepeerBestellingen(bestellingenX);
            }
        }

        private static ICollection<Bestelling> GroepeerBestellingen(IEnumerable<Bestelling> bestellingen)       // to delete
        {
            var gegroepeerd = bestellingen.GroupBy(k => k.GebruikerId);              // zorgt ervoor elke bestelling slechts 1 keer voorkomt

            List<Bestelling> BestellingenMetBestellijnen = new List<Bestelling>();
            foreach (var groep in gegroepeerd)
            {
                var bestelling = groep.First();
                List<BestellingLijn>? alleBestellijnen = new List<BestellingLijn>();
                foreach (var x in groep)
                {
                    alleBestellijnen.Add(x.Bestellijnen.First());
                }
                bestelling.Bestellijnen = alleBestellijnen;
                BestellingenMetBestellijnen.Add(bestelling);
            }

            return BestellingenMetBestellijnen;
        }

        public bool MarkerenGeleverd(int BestellingId)
        {
            string sql = @"UPDATE startspelercompanion.bestellingen
                        SET IsGeleverd = @IsGeleverd
                WHERE Id = @BestellingId";

            var parameters = new
            {
                BestellingId = BestellingId,
                IsGeleverd = '1'
            };
            using IDbConnection db = new MySqlConnection(ConnectionString);
            var affectedRows = db.Execute(sql, parameters);

            return affectedRows == 1;
        }


        public bool MarkerenBetaald(string GebruikerID)
        {
            string sql = @"UPDATE startspelercompanion.bestellingen
                        SET IsBetaald = @IsBetaald
                WHERE GebruikerId = @GebruikerID";

            var parameters = new
            {
                GebruikerId = GebruikerID,
                IsBetaald = '1'
            };

            // sql = "UPDATE `startspelercompanion`.`bestellingen` SET `IsBetaald` = '1' WHERE (`GebruikerId` = '3');";       // dit werkt correct

            using IDbConnection db = new MySqlConnection(ConnectionString);
            var affectedRows = db.Execute(sql, parameters);

            return affectedRows >= 1;
        }

        public bool CancelBestelling(int BestellingId) 
        {
            // dus eerst de bestellijnen deleten
            

            string sql = @"DELETE FROM startspelercompanion.bestellijnen WHERE BestellingId = @id;";

            using IDbConnection db = new MySqlConnection(ConnectionString);
            var affectedRows = db.Execute(sql, new { id = BestellingId });

            // return affectedRows == 1;

            // en dan de bestelling deleten

            string sql2 = @"DELETE FROM startspelercompanion.bestellingen WHERE Id = @id;";

            using IDbConnection db2 = new MySqlConnection(ConnectionString);
            var affectedRows2 = db2.Execute(sql2, new { id = BestellingId });


            return affectedRows2 == 1;
        }

    }
}