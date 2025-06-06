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
    public class ArtikelsRepository: BaseRepository, IArtikelsRepository
    {
        public IEnumerable<Artikel> OphalenArtikels(string artikeltype)
        {
            string sql = @"SELECT * FROM startspelercompanion.artikels WHERE Type = '" + artikeltype + "' ORDER BY Naam";         

            using (IDbConnection db = new MySqlConnection(ConnectionString))
            {
                return db.Query<Artikel>(sql).ToList();
            }
        }

        public List<Artikel> OphalenArtikels2()                                                 // wordt niet gebruikt
        {
            string sql = @"SELECT * FROM startspelercompanion.artikels";

            using (IDbConnection db = new MySqlConnection(ConnectionString))
            {
                return db.Query<Artikel>(sql).ToList();
            }
        }

        public IEnumerable<Artikel> OphalenKoudeDrank()
        {
            string sql = @"SELECT * FROM startspelercompanion.artikels WHERE Type = 'Koude_Drank' ORDER BY Naam";

            using (IDbConnection db = new MySqlConnection(ConnectionString))
            {
                return db.Query<Artikel>(sql).ToList();
            }
        }
        public IEnumerable<Artikel> OphalenWarmeDrank()
        {
            string sql = @"SELECT * FROM startspelercompanion.artikels WHERE Type = 'Warme_Drank' ORDER BY Naam";

            using (IDbConnection db = new MySqlConnection(ConnectionString))
            {
                return db.Query<Artikel>(sql).ToList();
            }
        }
        public IEnumerable<Artikel> OphalenAlcoholischeDrank()
        {
            string sql = @"SELECT * FROM startspelercompanion.artikels WHERE Type = 'Alcohol_Drank' ORDER BY Naam";

            using (IDbConnection db = new MySqlConnection(ConnectionString))
            {
                return db.Query<Artikel>(sql).ToList();
            }
        }
        public IEnumerable<Artikel> OphalenSnacks()
        {
            string sql = @"SELECT * FROM startspelercompanion.artikels WHERE Type = 'Snack' ORDER BY Naam";

            using (IDbConnection db = new MySqlConnection(ConnectionString))
            {
                return db.Query<Artikel>(sql).ToList();
            }
        }

        public bool ToevoegenArtikel(Artikel artikel)
        {
            string sql = @"INSERT INTO startspelercompanion.artikels (Naam, Prijs, Type, Logo, Info, Voorraad)
                    VALUES (@Naam, @Prijs, @Type, @Logo, @Info, @Voorraad)";

            var parameters = new
            {
                naam = artikel.Naam,
                prijs = artikel.Prijs,
                type = artikel.Type,
                logo = artikel.Logo,
                info = artikel.Info,
                voorraad = artikel.Voorraad
            };

            using IDbConnection db = new MySqlConnection(ConnectionString);
            var affectedRows = db.Execute(sql, parameters);

            return affectedRows == 1;
        }

        public bool AanpassenArtikel(Artikel artikel)
        {
            string sql = @"UPDATE startspelercompanion.artikels
                        SET Naam = @Naam,
                            Prijs = @Prijs,
                            Type = @Type,
                            Logo = @Logo,
                            Info = @Info,
                            Voorraad = @Voorraad
                        WHERE Id = @id";

            var parameters = new
            {
                Id = artikel.Id,
                naam = artikel.Naam,
                prijs = artikel.Prijs,
                type = artikel.Type,
                logo = artikel.Logo,
                info = artikel.Info,
                voorraad = artikel.Voorraad
            };

            // sql = "UPDATE `startspelercompanion`.`artikels` SET `Logo` = 'xyz' WHERE (`Id` = '14');";       // dit werkt correct

            using IDbConnection db = new MySqlConnection(ConnectionString);
            var affectedRows = db.Execute(sql, parameters);

            return affectedRows == 1;

        }

        public bool VerwijderArtikel(int id)
        {
            string sql = @"DELETE FROM startspelercompanion.artikels WHERE Id = @id;";

            using IDbConnection db = new MySqlConnection(ConnectionString);
            var affectedRows = db.Execute(sql, new { id = id });

            return affectedRows == 1;
        }

    }
}
