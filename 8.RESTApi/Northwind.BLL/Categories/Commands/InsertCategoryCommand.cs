using MediatR;
using CatalogService.Application.Entities;
using CatalogService.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CatalogService.BLL.Categories.Commands
{
    public record InsertCategoryCommand(Category category) : IRequest<Unit>;

    public class InsertCategoryCommandHandler : IRequestHandler<InsertCategoryCommand, Unit>
    {
        private readonly CatalogServiceContext _context;

        public InsertCategoryCommandHandler(CatalogServiceContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(InsertCategoryCommand request, CancellationToken cancellationToken)
        {
            await _context.Categories.AddAsync(request.category, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
