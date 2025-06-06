using Dapper;
using Kassa.Models;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kassa.Data.Repository
{
    public class AspnetrolesRepository :BaseRepository, IAspnetrolesRepository
    {
        public IEnumerable<Rol> OphalenRollen()
        {
            string sql = "SELECT * FROM Employee" ;
            using (IDbConnection db = new SqlConnection(ConnectionString))
            {
                return db.Query<Rol>(sql);
            }
        }
    }
}
