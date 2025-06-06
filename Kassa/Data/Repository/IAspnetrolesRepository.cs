using Kassa.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kassa.Data.Repository
{
    public interface IAspnetrolesRepository
    {
        public IEnumerable<Rol> OphalenRollen();
    }
}
