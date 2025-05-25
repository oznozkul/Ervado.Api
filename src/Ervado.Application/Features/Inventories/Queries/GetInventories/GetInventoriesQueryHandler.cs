using Ervado.Application.Common.Interfaces;
using Ervado.Application.Common.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Ervado.Application.Features.Inventories.Queries.GetInventories
{
    public class GetInventoriesQueryHandler : IRequestHandler<GetInventoriesQuery, Response<List<InventoryListDto>>>
    {
        private readonly IApplicationDbContext _dbContext;

        public GetInventoriesQueryHandler(IApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Response<List<InventoryListDto>>> Handle(GetInventoriesQuery request, CancellationToken cancellationToken)
        {
            var query = _dbContext.Inventories
                .Include(i => i.Product)
                .ThenInclude(p => p.ProductCategory)
                .Where(i => !i.IsDeleted);

            // Filter by product if specified
            if (request.ProductId.HasValue)
            {
                query = query.Where(i => i.ProductId == request.ProductId.Value);
            }

            // Filter by product category if specified
            if (request.ProductCategoryId.HasValue)
            {
                query = query.Where(i => i.Product.ProductCategoryId == request.ProductCategoryId.Value);
            }

            // Filter by low stock only
            if (request.LowStockOnly)
            {
                query = query.Where(i => i.Quantity < i.MinimumStockLevel);
            }

            // Filter by active status
            if (!request.IncludeInactive)
            {
                query = query.Where(i => i.Product.IsActive);
            }

            // Search by name, SKU, location or warehouse
            if (!string.IsNullOrWhiteSpace(request.SearchTerm))
            {
                var searchTerm = request.SearchTerm.ToLower();
                query = query.Where(i => 
                    i.Product.Name.ToLower().Contains(searchTerm) || 
                    i.Product.SKU.ToLower().Contains(searchTerm) ||
                    i.Location.ToLower().Contains(searchTerm) ||
                    i.Warehouse.ToLower().Contains(searchTerm));
            }

            // Apply sorting
            query = ApplySorting(query, request.SortBy, request.SortDescending);

            // Get total count for pagination
            var totalItems = await query.CountAsync(cancellationToken);
            var totalPages = (totalItems + request.PageSize - 1) / request.PageSize;

            // Get inventories with paging
            var inventories = await query
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(i => new InventoryListDto
                {
                    Id = i.Id,
                    ProductId = i.ProductId,
                    ProductName = i.Product.Name,
                    ProductSKU = i.Product.SKU,
                    ProductCategory = i.Product.ProductCategory != null ? i.Product.ProductCategory.Name : string.Empty,
                    Quantity = i.Quantity,
                    MinimumStockLevel = i.MinimumStockLevel,
                    MaximumStockLevel = i.MaximumStockLevel,
                    Location = i.Location,
                    Warehouse = i.Warehouse,
                    LastStockUpdateDate = i.LastStockUpdateDate
                })
                .ToListAsync(cancellationToken);

            return Response<List<InventoryListDto>>.Success(inventories);
        }

        private IQueryable<Domain.Entities.Inventory> ApplySorting(
            IQueryable<Domain.Entities.Inventory> query, 
            string sortBy, 
            bool sortDescending)
        {
            switch (sortBy.ToLower())
            {
                case "sku":
                    return sortDescending 
                        ? query.OrderByDescending(i => i.Product.SKU) 
                        : query.OrderBy(i => i.Product.SKU);
                case "quantity":
                    return sortDescending 
                        ? query.OrderByDescending(i => i.Quantity) 
                        : query.OrderBy(i => i.Quantity);
                case "category":
                    return sortDescending 
                        ? query.OrderByDescending(i => i.Product.ProductCategory.Name) 
                        : query.OrderBy(i => i.Product.ProductCategory.Name);
                case "location":
                    return sortDescending 
                        ? query.OrderByDescending(i => i.Location) 
                        : query.OrderBy(i => i.Location);
                case "warehouse":
                    return sortDescending 
                        ? query.OrderByDescending(i => i.Warehouse) 
                        : query.OrderBy(i => i.Warehouse);
                case "lastupdate":
                    return sortDescending 
                        ? query.OrderByDescending(i => i.LastStockUpdateDate) 
                        : query.OrderBy(i => i.LastStockUpdateDate);
                default: // Default sort by product name
                    return sortDescending 
                        ? query.OrderByDescending(i => i.Product.Name) 
                        : query.OrderBy(i => i.Product.Name);
            }
        }
    }
} 