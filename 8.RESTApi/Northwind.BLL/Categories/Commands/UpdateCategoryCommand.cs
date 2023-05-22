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

namespace CatalogService.BLL.Categories.Commands
{
    public record UpdateCategoryCommand(Category category) : IRequest<Unit>;
    
    public class UpdateCategoryCommandHandler : IRequestHandler<UpdateCategoryCommand, Unit>
    {
        private readonly CatalogServiceContext _context;

        public UpdateCategoryCommandHandler(CatalogServiceContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
        {
            var categoryToUpdate = await _context.Categories
                                            .Where(x => x.CategoryID == request.category.CategoryID)
                                            .Include(x => x.Items)
                                            .FirstOrDefaultAsync(cancellationToken);

            if (categoryToUpdate is null)
                throw new NotFoundException();

            _context.Entry(categoryToUpdate).CurrentValues.SetValues(request.category);

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
