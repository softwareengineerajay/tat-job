using AutoMapper;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NOV.ES.Framework.Core.CQRS.Commands;
using NOV.ES.TAT.Job.API.Validators;
using NOV.ES.TAT.Job.Domain;
using NOV.ES.TAT.Job.DTOs;
using NOV.ES.TAT.Job.Interfaces;
using System.Net;
using System.Net.Mime;

namespace NOV.ES.TAT.Job.API.Application.Commands
{
    public class CreateNovJobHandler
        : ICommandHandler<CreateNovJobCommand, ContentResult>
    {

        private readonly IMapper mapper;
        private readonly INovJobService novJobService;

        public CreateNovJobHandler(IMapper mapper,
            INovJobService novJobService)
        {
            this.mapper = mapper;
            this.novJobService = novJobService;
        }
        public Task<ContentResult> Handle(CreateNovJobCommand request, CancellationToken cancellationToken)
        {
            ContentResult contentResult = new()
            {
                ContentType = MediaTypeNames.Application.Json
            };

            ValidationResult validationResult = new NovJobValidator().Validate(request);
            if (!validationResult.IsValid)
            {
                contentResult.Content = JsonConvert.SerializeObject(validationResult.Errors);
                contentResult.StatusCode = (int)HttpStatusCode.BadRequest;
                return Task.FromResult(contentResult);
            }

            NovJob novJob = mapper.Map<NovJobDto, NovJob>(request.NovJobDto);
            bool result = novJobService.CreateNovJob(novJob);
            if (result)
                contentResult.StatusCode = (int)HttpStatusCode.OK;

            return Task.FromResult(contentResult);
        }
    }
}
