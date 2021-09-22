using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebApiTest.Data;
using WebApiTest.Models;

namespace WebApiTest.Services.WifeService
{
    public class WifeService : IWifeService
    {
        private readonly TowerContext _context;

        public WifeService(TowerContext context)
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
        public void AddProduct(WantedList wantedListItem)
        {
            var _wantedListItem = new WantedList
            {
                Id = wantedListItem.Id,
                NameProduct = wantedListItem.NameProduct,
                BoughtStatus = wantedListItem.BoughtStatus
            };
            _context.WantedLists.Add(_wantedListItem);
        }
        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
        public async Task<WantedList> FindWantedListAsync(long id)
        {
            return await _context.WantedLists.FindAsync(id);
        }
        public void RemoveWantedList(WantedList wantedListItem)
        {
            _context.WantedLists.Remove(wantedListItem);
        }
        public void RemoveAllWantedList()
        {
            var wantedList = _context.WantedLists.Select(i => new WantedList
            {
                Id = i.Id,
                BoughtStatus = i.BoughtStatus,
                NameProduct = i.NameProduct
            }).ToList();
            foreach (var item in wantedList)
            {
                _context.WantedLists.Remove(item);
            }
        }
    }
}
