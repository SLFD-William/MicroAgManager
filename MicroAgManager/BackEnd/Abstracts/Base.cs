using Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BackEnd.Abstracts
{
    public abstract class Base
    {
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        protected static ILogger _log;
        protected static IMediator _mediator;
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        protected Base(IMediator mediator, ILogger log)
        {
                _mediator = mediator;
                _log = log;
        }
    }
}
