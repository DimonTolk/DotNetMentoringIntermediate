using MediatR;
using Microsoft.EntityFrameworkCore;
using CatalogService.Application.Entities;
using CatalogService.Infrastructure;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CatalogService.BLL.Categories.Queries
{
    public record GetCategoriesWithCountQuery(int count) : IRequest<IEnumerable<Category>>;

    public class GetCategoriesWithCountQueryHandler : IRequestHandler<GetCategoriesWithCountQuery, IEnumerable<Category>>
    {
        private readonly CatalogServiceContext _context;

        public GetCategoriesWithCountQueryHandler(CatalogServiceContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Category>> Handle(GetCategoriesWithCountQuery request, CancellationToken cancellationToken)
        {
            return await _context.Categories.Include(x => x.Items).Take(request.count).ToListAsync(cancellationToken);
        }
    }
}
