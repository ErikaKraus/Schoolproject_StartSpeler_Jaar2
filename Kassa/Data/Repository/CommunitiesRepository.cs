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
    public class CommunitiesRepository:BaseRepository, ICommunitiesRepository
    {
        public IEnumerable<Community> OphalenCommunities()
        {
            string sql = @"SELECT * FROM communities";
            using (IDbConnection db = new MySqlConnection(ConnectionString))
            {
                return db.Query<Community>(sql);
            }
        }
    }
}
