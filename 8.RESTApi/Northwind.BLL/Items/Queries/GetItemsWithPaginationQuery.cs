using MediatR;
using Microsoft.EntityFrameworkCore;
using CatalogService.BLL.ViewModels;
using CatalogService.Infrastructure;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CatalogService.BLL.Products.Queries
{
    public record GetItemsWithPaginationQuery(int pageNumber, int pageSize, int? categoryId) : IRequest<ItemPaginationVm>;

    public class GetItemsWithPaginationQueryHandler : IRequestHandler<GetItemsWithPaginationQuery, ItemPaginationVm>
    {
        private readonly CatalogServiceContext _context;

        public GetItemsWithPaginationQueryHandler(CatalogServiceContext context)
        {
            _context = context;
        }

        public async Task<ItemPaginationVm> Handle(GetItemsWithPaginationQuery request, CancellationToken cancellationToken)
        {
            var products = await _context.Items.ToListAsync(cancellationToken);
            if (request.categoryId.HasValue)
                products = products.Where(x => x.CategoryID == request.categoryId.Value).ToList();

            var productVm = new ItemPaginationVm();
            productVm.PageNumber = request.pageNumber;
            productVm.Items = products.Take(request.pageSize);

            return productVm;
        }
    }
}
