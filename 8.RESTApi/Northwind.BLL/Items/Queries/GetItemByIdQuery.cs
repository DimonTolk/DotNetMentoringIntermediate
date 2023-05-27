using MediatR;
using Microsoft.EntityFrameworkCore;
using CatalogService.Application.Entities;
using CatalogService.BLL.Common.Exceptions;
using CatalogService.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CatalogService.BLL.Items.Queries
{
    public record GetItemByIdQuery(int id) : IRequest<Item>;

    public class GetItemByIdQueryHandler : IRequestHandler<GetItemByIdQuery, Item>
    {
        private readonly CatalogServiceContext _context;

        public GetItemByIdQueryHandler(CatalogServiceContext context)
        {
            _context = context;
        }

        public async Task<Item> Handle(GetItemByIdQuery request, CancellationToken cancellationToken)
        {
            var product = await _context.Items
                                         .Where(x => x.ItemID == request.id)
                                         .FirstOrDefaultAsync(cancellationToken);

            if (product is null)
                throw new NotFoundException();

            return product;
        }
    }
}
