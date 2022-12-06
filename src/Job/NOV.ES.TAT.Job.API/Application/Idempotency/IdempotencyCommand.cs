using Microsoft.AspNetCore.Mvc;
using NOV.ES.Framework.Core.CQRS.Commands;

namespace NOV.ES.TAT.Job.API.Application.Commands;

public class IdempotencyCommand : ICommand<ContentResult>
{
    public ICommand<ContentResult> Command { get; }
    public int Id { get; }
    public IdempotencyCommand(ICommand<ContentResult> command, int id)
    {
        Command = command;
        Id = id;
    }
}
