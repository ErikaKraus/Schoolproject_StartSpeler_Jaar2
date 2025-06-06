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
    public class AspnetuserrolesRepository: BaseRepository, IAspnetuserrolesRepository
    {
        public IEnumerable<AspnetUserRole> OphalenUserRollen(string gebruikerId)
        {
            string sql = @"SELECT AUR.*, AU.* , AR.* FROM startspelercompanion.aspnetuserroles AUR ";
            sql += "INNER JOIN startspelercompanion.aspnetusers AU ON AUR.UserId = AU.Id ";
            sql += "INNER JOIN startspelercompanion.aspnetroles AR ON AUR.RoleId = AR.Id ";
            sql += "WHERE AU.id = '" + gebruikerId + "'";


            using (IDbConnection db = new MySqlConnection(ConnectionString))
            {
                var debugVar = db.Query<AspnetUserRole, Gebruiker, AspnetRole, AspnetUserRole>(
                    sql,
                    (aspnetuserrole, gebruiker,  aspnetrole) =>
                    {
                        aspnetuserrole.Gebruiker = gebruiker;
                        aspnetuserrole.AspnetRole = aspnetrole;
                        return aspnetuserrole;
                    },
                    splitOn: "Id"
                );
                return debugVar;
            }
        }

        public bool ToevoegenRol(string UserId,string RoleId)
        {
            string sql = @"INSERT INTO startspelercompanion.aspnetuserroles (UserId, RoleId)
                    VALUES (@UserId, @RoleId)";

            var parameters = new
            {
                UserId = UserId,
                RoleId = RoleId
                
            };

            using IDbConnection db = new MySqlConnection(ConnectionString);
            var affectedRows = db.Execute(sql, parameters);

            return affectedRows == 1;
        }

        public bool VerwijderRol(string UserId,string RoleId)
        {
            string sql = @"DELETE FROM startspelercompanion.aspnetuserroles WHERE UserId = @UserId AND RoleId = @RoleId";
            // string sql = @"DELETE FROM startspelercompanion.aspnetuserroles WHERE UserId = 1 AND RoleId = 4";

            var parameters = new
            {
                UserId = UserId,
                RoleId = RoleId

            };

            using IDbConnection db = new MySqlConnection(ConnectionString);
            // var affectedRows = db.Execute(sql, new { id = UserId });
            var affectedRows = db.Execute(sql, parameters);

            return affectedRows == 1;
        }

        //public IEnumerable<AspnetUserRole> OphalenUserRollenVoorLogin2(string gebruikerId)
        //{
        //    string sql = @"SELECT * FROM startspelercompanion.aspnetuserroles ";
        //    sql += "WHERE UserId =" + gebruikerId;


        //    using (IDbConnection db = new MySqlConnection(ConnectionString))
        //    {
        //        var debugVar = db.Query<AspnetUserRole, Gebruiker, AspnetRole, AspnetUserRole>(
        //            sql,
        //            (aspnetuserrole, gebruiker, aspnetrole) =>
        //            {
        //                aspnetuserrole.Gebruiker = gebruiker;
        //                aspnetuserrole.AspnetRole = aspnetrole;
        //                return aspnetuserrole;
        //            },
        //            splitOn: "Id"
        //        );
        //        return debugVar;
        //    }
        //}


        public List<AspnetUserRole> OphalenUserRollenVoorLogin(string gebruikerId)
        {
            string sql = @"SELECT * FROM startspelercompanion.aspnetuserroles ";
            sql += "WHERE UserId = '" + gebruikerId + "'";

            using (IDbConnection db = new MySqlConnection(ConnectionString))
            {
                return db.Query<AspnetUserRole>(sql).ToList();
            }
        }


    }
}
