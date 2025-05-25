using Ervado.Application.Common.Interfaces;
using Ervado.Application.Common.Models;
using Ervado.Domain.Entities;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Ervado.Application.Features.Brands.Commands.Create
{
    public class CreateBrandCommandHandler : IRequestHandler<CreateBrandCommand, Response<CreateBrandResponse>>
    {
        private readonly IApplicationDbContext _dbContext;

        public CreateBrandCommandHandler(IApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Response<CreateBrandResponse>> Handle(CreateBrandCommand request, CancellationToken cancellationToken)
        {
            var brand = new Brand
            {
                Name = request.Name,
                Description = request.Description,
                LogoUrl = request.LogoUrl,
                IsActive = request.IsActive,
                CreatedDate = DateTime.UtcNow
                // CreatedUserId will be set from the user context
            };

            _dbContext.Brands.Add(brand);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return Response<CreateBrandResponse>.Success(new CreateBrandResponse
            {
                Id = brand.Id,
                Name = brand.Name
            });
        }
    }
} 