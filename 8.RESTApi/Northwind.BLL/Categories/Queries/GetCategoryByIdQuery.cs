using MediatR;
using Microsoft.EntityFrameworkCore;
using CatalogService.Application.Entities;
using CatalogService.BLL.Common.Exceptions;
using CatalogService.Infrastructure;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CatalogService.BLL.Categories.Queries
{
    public record GetCategoryByIdQuery(int id) : IRequest<Category>;

    public class GetCategoryByIdQueryHandler : IRequestHandler<GetCategoryByIdQuery, Category>
    {
        private readonly CatalogServiceContext _context;

        public GetCategoryByIdQueryHandler(CatalogServiceContext context)
        {
            _context = context;
        }

        public async Task<Category> Handle(GetCategoryByIdQuery request, CancellationToken cancellationToken)
        {
            var category = await _context.Categories
                                         .Where(x => x.CategoryID == request.id)
                                         .Include(x => x.Items)
                                         .FirstOrDefaultAsync(cancellationToken);

            if (category is null)
                throw new NotFoundException();

            return category;
        }
    }
}
