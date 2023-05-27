using MediatR;
using Microsoft.EntityFrameworkCore;
using CatalogService.Application.Entities;
using CatalogService.BLL.Common.Exceptions;
using CatalogService.Infrastructure;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CatalogService.BLL.Items.Commands
{
    public record UpdateItemCommand(Item item) : IRequest<Unit>;

    public class UpdateItemCommandHandler : IRequestHandler<UpdateItemCommand, Unit>
    {
        private readonly CatalogServiceContext _context;

        public UpdateItemCommandHandler(CatalogServiceContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(UpdateItemCommand request, CancellationToken cancellationToken)
        {
            var itemToUpdate = await _context.Items.Where(x => x.ItemID == request.item.ItemID).FirstOrDefaultAsync(cancellationToken);

            if (itemToUpdate is null)
                throw new NotFoundException();

            _context.Entry(itemToUpdate).CurrentValues.SetValues(request.item);

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
