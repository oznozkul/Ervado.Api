using Ervado.Application.Common.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ervado.Application.Features.Domains
{
    public class DomainsCommandHandler : IRequestHandler<DomainsCommand, Response<DomainsResponse>>
    {
        public Task<Response<DomainsResponse>> Handle(DomainsCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
