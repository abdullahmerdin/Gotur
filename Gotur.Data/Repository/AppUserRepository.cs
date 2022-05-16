using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gotur.Data.Repository.IRepository;
using Gotur.Models;

namespace Gotur.Data.Repository
{
    public class AppUserRepository : Repository<AppUser>, IAppUserRepository
        
    {
        public AppUserRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
