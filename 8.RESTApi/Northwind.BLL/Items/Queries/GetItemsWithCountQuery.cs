using MediatR;
using Microsoft.EntityFrameworkCore;
using CatalogService.Application.Entities;
using CatalogService.Infrastructure;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CatalogService.BLL.Products.Queries
{
    public record GetItemsWithCountQuery(int count) : IRequest<IEnumerable<Item>>;

    public class GetItemsWithCountQueryHandler : IRequestHandler<GetItemsWithCountQuery, IEnumerable<Item>>
    {
        private readonly CatalogServiceContext _context;

        public GetItemsWithCountQueryHandler(CatalogServiceContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Item>> Handle(GetItemsWithCountQuery request, CancellationToken cancellationToken)
        {
            return await _context.Items.Take(request.count).ToListAsync(cancellationToken);
        }
    }
}
