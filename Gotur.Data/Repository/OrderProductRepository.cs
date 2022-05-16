using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gotur.Data.Repository.IRepository;
using Gotur.Models;

namespace Gotur.Data.Repository
{
    public class OrderProductRepository : Repository<OrderProduct>, IOrderProductRepository
    {
        public OrderProductRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
