using MediatR;
using Microsoft.EntityFrameworkCore;
using CatalogService.BLL.Common.Exceptions;
using CatalogService.Infrastructure;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CatalogService.BLL.Categories.Commands
{
    public record DeleteCategoryCommand(int id) : IRequest<Unit>;

    public class DeleteCategoryCommandHandler : IRequestHandler<DeleteCategoryCommand, Unit>
    {
        private readonly CatalogServiceContext _context;

        public DeleteCategoryCommandHandler(CatalogServiceContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
        {
            var categoryToDelete = await _context.Categories
                                            .Where(x => x.CategoryID == request.id)
                                            .Include(x => x.Items)
                                            .FirstOrDefaultAsync(cancellationToken);

            if (categoryToDelete is null)
                throw new NotFoundException();

            if(categoryToDelete.Items != null)
            {
                foreach (var item in categoryToDelete.Items)
                {
                    _context.Items.Remove(item);
                }
            }

            _context.Categories.Remove(categoryToDelete);

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
