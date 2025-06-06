using Dapper;
using Kassa.Models;
// using Microsoft.Data.SqlClient;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kassa.Data.Repository
{
    public class BestellijnenRepository: BaseRepository, IBestellijnenRepository
    {
        //public IEnumerable<BestellingLijn> OphalenBestellinglijnDatum(DateTime datum1)
        //{
        //    string sql = "SELECT * FROM Employee ";
        //    using (IDbConnection db = new MySqlConnection(ConnectionString))
        //    {
        //        return db.Query<BestellingLijn>(sql);
        //    }
        //}

        public IEnumerable<BestellingLijn> OphalenBestellijnPerGebruiker(string gebruikerId)
        {
            string sql = @"SELECT LI.* , A.* FROM startspelercompanion.bestellijnen LI ";
            sql += "INNER JOIN startspelercompanion.artikels A ON LI.ArtikelId = A.Id ";
            sql += "INNER JOIN startspelercompanion.bestellingen B ON LI.BestellingId = B.Id ";
            sql += "WHERE B.IsBetaald = false AND B.GebruikerId = '" + gebruikerId + "'";


            using (IDbConnection db = new MySqlConnection(ConnectionString))
            {
                var debugVar = db.Query<BestellingLijn, Artikel, BestellingLijn>(
                    sql,
                    (bestellinglijn, artikel) =>
                    {
                        bestellinglijn.Artikel = artikel;
                        
                        return bestellinglijn;
                    },
                    splitOn: "Id"
                );
                return debugVar;
            }
        }

        public IEnumerable<BestellingLijn> OphalenBestellijnPerDag(DateTime dag)
        {
            // correct syntax sql = SELECT * FROM startspelercompanion.bestellingen WHERE CONVERT(Tijdstip, date)  = '2024-04-13';
            

            string enkelDag = "'" + dag.ToString("yyyy-MM-dd") + "'";

            string sql = @"SELECT LI.* , A.* FROM startspelercompanion.bestellijnen LI ";
            sql += "INNER JOIN startspelercompanion.artikels A ON LI.ArtikelId = A.Id ";
            sql += "INNER JOIN startspelercompanion.bestellingen B ON LI.BestellingId = B.Id ";
            sql += "WHERE CONVERT(B.Tijdstip,date) = " + enkelDag;


            using (IDbConnection db = new MySqlConnection(ConnectionString))
            {
                var debugVar = db.Query<BestellingLijn, Artikel, BestellingLijn>(
                    sql,
                    (bestellinglijn, artikel) =>
                    {
                        bestellinglijn.Artikel = artikel;

                        return bestellinglijn;
                    },
                    splitOn: "Id"
                );
                return debugVar;
            }


        }

        public IEnumerable<BestellingLijn> OphalenBestellijnenNietGeleverd()                            
        {
            // string sql = @"SELECT LI.* FROM startspelercompanion.bestellijnen LI ";
            string sql = @"SELECT LI.*, AR.*, BL.*, AU.* FROM startspelercompanion.bestellijnen LI ";
            sql += "INNER JOIN startspelercompanion.artikels AR ON LI.ArtikelId = AR.Id ";
            sql += "INNER JOIN startspelercompanion.bestellingen BL ON LI.BestellingId = BL.Id ";
            sql += "INNER JOIN startspelercompanion.aspnetusers AU ON BL.GebruikerId = AU.Id ";
            sql += "WHERE BL.IsGeleverd = false ";
            sql += "ORDER BY BL.Tijdstip ";

            using (IDbConnection db = new MySqlConnection(ConnectionString))
            {
                var debugVar = db.Query<BestellingLijn, Artikel, Bestelling, Gebruiker, BestellingLijn>(
                    sql,
                    (bestellinglijn, artikel, bestelling, gebruiker) =>
                    {
                        bestellinglijn.Artikel = artikel;
                        bestellinglijn.Bestelling = bestelling;
                        bestelling.Gebruiker = gebruiker;
                        return bestellinglijn;
                    },
                    splitOn: "Id"
                );
                return debugVar;
            }
        }

    }
}
