using Microsoft.AspNetCore.Mvc;
using NOV.ES.Framework.Core.CQRS.Commands;
using NOV.ES.TAT.Job.DTOs;
using System.Text.Json.Serialization;

namespace NOV.ES.TAT.Job.API.Application.Commands
{
    public class CreateNovJobCommand : ICommand<ContentResult>
    {
        public NovJobDto NovJobDto { get; private set; }

        [JsonConstructor]
        public CreateNovJobCommand(NovJobDto novJobDto)
        {
            this.NovJobDto = novJobDto;
        }
    }
}
