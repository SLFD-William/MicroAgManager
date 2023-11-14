using Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BackEnd.Abstracts
{
    public abstract class BaseRequestHandler<T> : Base where T : BaseQuery
    {
        protected BaseRequestHandler(IMediator mediator, ILogger log) : base(mediator, log)
        {
        }
    }
}
