using MediatR;
using CatalogService.Application.Entities;
using CatalogService.Infrastructure;
using System.Threading;
using System.Threading.Tasks;

namespace CatalogService.BLL.Products.Commands
{
    public record InsertItemCommand(Item product) : IRequest<Unit>;

    public class InsertItemCommandHandler : IRequestHandler<InsertItemCommand, Unit>
    {
        private readonly CatalogServiceContext _context;

        public InsertItemCommandHandler(CatalogServiceContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(InsertItemCommand request, CancellationToken cancellationToken)
        {
            await _context.Items.AddAsync(request.product, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
