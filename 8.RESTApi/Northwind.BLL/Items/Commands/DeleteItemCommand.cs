using MediatR;
using Microsoft.EntityFrameworkCore;
using CatalogService.BLL.Common.Exceptions;
using CatalogService.Infrastructure;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CatalogService.BLL.Products.Commands
{
    public record DeleteItemCommand(int id) : IRequest<Unit>;

    public class DeleteItemCommandHandler : IRequestHandler<DeleteItemCommand, Unit>
    {
        private readonly CatalogServiceContext _context;

        public DeleteItemCommandHandler(CatalogServiceContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(DeleteItemCommand request, CancellationToken cancellationToken)
        {
            var itemToDelete = await _context.Items.Where(x => x.ItemID == request.id).FirstOrDefaultAsync(cancellationToken);

            if (itemToDelete is null)
                throw new NotFoundException();

            _context.Items.Remove(itemToDelete);

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
