using Microsoft.AspNetCore.Mvc;
using NOV.ES.Framework.Core.CQRS.Commands;
using NOV.ES.TAT.Job.Infrastructure;

namespace NOV.ES.TAT.Job.API.Application.Commands;
public class IdempotencyCommandHandler : ICommandHandler<IdempotencyCommand, ContentResult>
{
    private readonly ICommandBus _commandBus;
    private readonly IRequestManager _requestManager;

    public IdempotencyCommandHandler(ICommandBus commandBus, IRequestManager requestManager)
    {
        _commandBus = commandBus;
        _requestManager = requestManager;
    }

    protected virtual ContentResult CreateResultForDuplicateRequest()
    {
        return new ContentResult()
        {
            StatusCode = StatusCodes.Status200OK
        };
    }

    public async Task<ContentResult> Handle(IdempotencyCommand message, CancellationToken cancellationToken)
    {
        var alreadyExists = await _requestManager.ExistAsync(message.Id);
        if (alreadyExists)
        {
            return CreateResultForDuplicateRequest();
        }
        else
        {
            await _requestManager.CreateRequestForCommandAsync<ICommand>(message.Id);

            var command = message.Command;

            return await _commandBus.Send<ICommand<ContentResult>, ContentResult>(command);
        }
    }
}
