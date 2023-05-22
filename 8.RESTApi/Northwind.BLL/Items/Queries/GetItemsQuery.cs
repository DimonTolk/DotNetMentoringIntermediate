using MediatR;
using Microsoft.EntityFrameworkCore;
using CatalogService.Application.Entities;
using CatalogService.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CatalogService.BLL.Products.Queries
{
    public record GetItemsQuery() : IRequest<IEnumerable<Item>>;

    public class GetItemsQueryHandler : IRequestHandler<GetItemsQuery, IEnumerable<Item>>
    {
        private readonly CatalogServiceContext _context;

        public GetItemsQueryHandler(CatalogServiceContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Item>> Handle(GetItemsQuery request, CancellationToken cancellationToken)
        {
            return await _context.Items.ToListAsync(cancellationToken);
        }
    }
}
