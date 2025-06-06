using Kassa.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kassa.Data.Repository
{
    public interface IAspnetuserrolesRepository
    {

        public IEnumerable<AspnetUserRole> OphalenUserRollen(string gebruikerId);
        public bool ToevoegenRol(string UserId, string RoleId);
        public bool VerwijderRol(string UserId, string RoleId);

        public List<AspnetUserRole> OphalenUserRollenVoorLogin(string gebruikerId);
    }
}
