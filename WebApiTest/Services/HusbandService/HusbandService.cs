using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebApiTest.Data;
using WebApiTest.Models;

namespace WebApiTest.Services.HusbandService
{
    public class HusbandService
    {

        private readonly TowerContext _context;

        public HusbandService(TowerContext context)
        {
            _context = context;
        }
        public async Task<List<WantedList>> GetWantedListAsync()
        {
            return await _context.WantedLists.Select(i => new WantedList
            {
                Id = i.Id,
                BoughtStatus = i.BoughtStatus,
                NameProduct = i.NameProduct
            }).ToListAsync();
        }
        //public async Task<List<Shop>> GeShopsForVisitAsync()
        //{

        //    var wantedList = GetWantedListAsync();
        //    wantedList



        //        return await _context.WantedLists.Select(i => new WantedList
        //    {
        //        Id = i.Id,
        //        BoughtStatus = i.BoughtStatus,
        //        NameProduct = i.NameProduct
        //    }).ToListAsync();
        //}
        public async Task<Shop> FindShopAsync(long id)
        {
            return await _context.Shops.FindAsync(id);
        }
        public async Task<Product> FindProductAsync(string name)
        {
            return await _context.Products.FindAsync(name);
        }
    }
}
