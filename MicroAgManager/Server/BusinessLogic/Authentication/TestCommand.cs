using MediatR;

namespace BackEnd.BusinessLogic.Authentication
{
    public class TestCommand : IRequest
    {
        public class Handler : IRequestHandler<TestCommand>
        {

            public Task Handle(TestCommand request, CancellationToken cancellationToken)
            {
                throw new NotImplementedException();
            }
        }
    }
}
