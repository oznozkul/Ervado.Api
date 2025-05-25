using Ervado.Application.Common.Interfaces;
using Ervado.Application.Common.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Ervado.Application.Features.StockMovements.Queries.GetStockMovements
{
    public class GetStockMovementsQueryHandler : IRequestHandler<GetStockMovementsQuery, Response<List<StockMovementListDto>>>
    {
        private readonly IApplicationDbContext _dbContext;

        public GetStockMovementsQueryHandler(IApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Response<List<StockMovementListDto>>> Handle(GetStockMovementsQuery request, CancellationToken cancellationToken)
        {
            var query = _dbContext.StockMovements
                .Include(sm => sm.Inventory)
                .ThenInclude(i => i.Product)
                .Where(sm => !sm.IsDeleted);

            // Filter by inventory if specified
            if (request.InventoryId.HasValue)
            {
                query = query.Where(sm => sm.InventoryId == request.InventoryId.Value);
            }

            // Filter by product if specified
            if (request.ProductId.HasValue)
            {
                query = query.Where(sm => sm.Inventory.ProductId == request.ProductId.Value);
            }

            // Filter by movement type if specified
            if (request.Type.HasValue)
            {
                query = query.Where(sm => sm.Type == request.Type.Value);
            }

            // Filter by date range
            if (request.FromDate.HasValue)
            {
                query = query.Where(sm => sm.CreatedDate >= request.FromDate.Value);
            }

            if (request.ToDate.HasValue)
            {
                var endDate = request.ToDate.Value.AddDays(1); // Include the entire end date
                query = query.Where(sm => sm.CreatedDate < endDate);
            }

            // Search by reference or notes
            if (!string.IsNullOrWhiteSpace(request.SearchTerm))
            {
                var searchTerm = request.SearchTerm.ToLower();
                query = query.Where(sm => 
                    sm.Reference.ToLower().Contains(searchTerm) || 
                    sm.Notes.ToLower().Contains(searchTerm) ||
                    sm.Inventory.Product.Name.ToLower().Contains(searchTerm) ||
                    sm.Inventory.Product.SKU.ToLower().Contains(searchTerm));
            }

            // Get total count for pagination
            var totalItems = await query.CountAsync(cancellationToken);
            var totalPages = (totalItems + request.PageSize - 1) / request.PageSize;

            // Get stock movements with paging
            var stockMovements = await query
                .OrderByDescending(sm => sm.CreatedDate)
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(sm => new StockMovementListDto
                {
                    Id = sm.Id,
                    InventoryId = sm.InventoryId,
                    ProductId = sm.Inventory.ProductId,
                    ProductName = sm.Inventory.Product.Name,
                    ProductSKU = sm.Inventory.Product.SKU,
                    Quantity = sm.Quantity,
                    Type = sm.Type,
                    Reference = sm.Reference,
                    Notes = sm.Notes,
                    CreatedDate = sm.CreatedDate,
                    CreatedByUserName = GetUserNameById(sm.CreatedUserId)
                })
                .ToListAsync(cancellationToken);

            return Response<List<StockMovementListDto>>.Success(stockMovements);
        }

        private string GetUserNameById(int? userId)
        {
            if (!userId.HasValue)
                return string.Empty;

            // In a real application, we would look up the user's name from a user repository
            return $"User {userId}";
        }
    }
} 