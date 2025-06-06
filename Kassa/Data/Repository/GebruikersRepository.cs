using Dapper;
using Kassa.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kassa.Data.Repository
{
    public class GebruikersRepository: BaseRepository, IGebruikersRepository
    {
        public IEnumerable<Gebruiker> OphalenGebruikersZoekterm(string zoekterm)
        {
            string sql = @"SELECT * FROM startspelercompanion.aspnetusers WHERE Naam LIKE '%";
            sql += zoekterm;
            sql += "%'";
            
            
            using (IDbConnection db = new MySqlConnection(ConnectionString))
            {
                return db.Query<Gebruiker>(sql);
            }

        }

        public IEnumerable<Gebruiker> AlleGebruikersOphalen()
        {
            string sql = @"SELECT * FROM startspelercompanion.aspnetusers";  
            
            using (IDbConnection db = new MySqlConnection(ConnectionString))
            {
                return db.Query<Gebruiker>(sql);
            }
        }

        public bool VoegGebruikerToe(Gebruiker gebruiker)
        {
            string sql = @"INSERT INTO startspelercompanion.aspnetusers (Voornaam, Naam, Email) VALUES (@Voornaam, @Naam, @Email)";

            using (IDbConnection db = new MySqlConnection(ConnectionString))
            {
                var affectedRows = db.Execute(sql, gebruiker);
                return affectedRows == 1;
            }
        }

        public async Task<bool> BestaatGebruiker(string gebruikerId)
        {
            using (var connection = new MySqlConnection(ConnectionString))
            {
                var exists = await connection.QueryFirstOrDefaultAsync<int>(
                    "SELECT COUNT(1) FROM aspnetusers WHERE Id = @GebruikerId",
                    new { GebruikerId = gebruikerId });
                return exists > 0;
            }

        }

        public IEnumerable<Gebruiker> OphalenGebruikersUsername(string username)
        {

            string sql = @"SELECT * FROM startspelercompanion.aspnetusers WHERE UserName = '";
            sql += username;
            sql += "'";

            using (IDbConnection db = new MySqlConnection(ConnectionString))
            {
                return db.Query<Gebruiker>(sql);
            }


        }

        public bool PasswordReset(Gebruiker gebruiker, string newpassword)
        {
            // Id is een string in tabel aspnetusers 

            string sql = @"UPDATE startspelercompanion.aspnetusers SET SimpleHash = '";
            sql += newpassword;
            sql += "' WHERE Id = '";
            sql += gebruiker.Id + "'";
            

            var parameters = new
            {
                Id = gebruiker.Id.ToString(),
                PasswordHash = newpassword
            };

            // sql = "UPDATE `startspelercompanion`.`aspnetusers` SET `PasswordHash` = 'xyz' WHERE (`Id` = '2');"      // dit werkt correct

            using IDbConnection db = new MySqlConnection(ConnectionString);
            var affectedRows = db.Execute(sql, parameters);

            return affectedRows == 1;

        }

    }
}
