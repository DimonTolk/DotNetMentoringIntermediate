using CatalogService.Application.Entities;
using System.Collections.Generic;

namespace CatalogService.BLL.ViewModels
{
    public class ItemPaginationVm
    {
        public IEnumerable<Item> Items { get; set; }
        public int PageNumber { get; set; }
    }
}
