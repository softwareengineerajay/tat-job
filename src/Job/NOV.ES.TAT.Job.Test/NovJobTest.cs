using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Newtonsoft.Json;
using NOV.ES.Framework.Core.CQRS.Commands;
using NOV.ES.Framework.Core.CQRS.Queries;
using NOV.ES.Framework.Core.Pagination;
using NOV.ES.TAT.Job.API.Application.Queries;
using NOV.ES.TAT.Job.API.Controllers;
using NOV.ES.TAT.Job.Domain;
using NOV.ES.TAT.Job.DTOs;
using NOV.ES.TAT.Job.Infrastructure;
using NOV.ES.TAT.Job.Interfaces;
using NOV.ES.TAT.Job.Service;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace NOV.ES.TAT.Job.Test
{
    [TestClass]
    public class NovJobTest : TestBase
    {
        private readonly INovJobService novJobService;
        private Paging pagingParameters;
        private NovJobsController NovJobController;
        private IEnumerable<NovJobDto> novJobDtos = new List<NovJobDto>();
        public NovJobTest() : base()
        {
            novJobService = new NovJobService(new NovJobQueryRepository(JobDBContext),
                new NovJobCommandRepository(JobDBContext));

            pagingParameters = new Paging();
            ICommandBus commandBus = new Mock<ICommandBus>().Object;
            IQueryBus queryBus = new Mock<IQueryBus>().Object;
            NovJobController = new NovJobsController(commandBus, queryBus);
        }

        [TestInitialize]
        public void SetUp()
        {
            ClearAndSeedTestData();
        }

        #region Controller unit Tests

        [TestMethod]
        public void ShouldReturnsServiceNameWithOkStatus_ServiceName()
        {
            var result = NovJobController.ServiceName();

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            Assert.AreEqual("NovJob API Service.", ((OkObjectResult)result).Value);
        }

        [TestMethod]
        public async Task ShouldReturnsEmptyListOfNovJobWithOkStatus_GetNovJobs()
        {
            var mockQueryNus = new Mock<IQueryBus>();
            novJobDtos = new List<NovJobDto>();
            mockQueryNus.Setup(x => x.Send<GetPaginationNovJobsQuery, PagedResult<NovJobDto>>(It.IsAny<GetPaginationNovJobsQuery>()))
                .ReturnsAsync(new PagedResult<NovJobDto>(novJobDtos, novJobDtos.Count(), pagingParameters));

            var queryBus = mockQueryNus.Object;
            ICommandBus commandBus = new Mock<ICommandBus>().Object;
            NovJobController = new NovJobsController(commandBus, queryBus);

            NovJobController.ControllerContext.HttpContext = new DefaultHttpContext();
            NovJobController.ControllerContext.HttpContext.Request.QueryString = new QueryString();
            var result = await NovJobController.GetNovJobs(new Paging());

            Assert.IsNotNull(result.Result);
            Assert.IsInstanceOfType(result.Result, typeof(OkObjectResult));
        }

        [TestMethod]
        public async Task ShouldReturnsListofNovJobWithOkStatus_GetNovJobs()
        {
            var mockQueryNus = new Mock<IQueryBus>();

            mockQueryNus.Setup(x => x.Send<GetPaginationNovJobsQuery, PagedResult<NovJobDto>>(It.IsAny<GetPaginationNovJobsQuery>()))
                .ReturnsAsync(new PagedResult<NovJobDto>(novJobDtos, novJobDtos.Count(), new Paging()));

            var queryBus = mockQueryNus.Object;
            ICommandBus commandBus = new Mock<ICommandBus>().Object;
            NovJobController = new NovJobsController(commandBus, queryBus);
            NovJobController.ControllerContext.HttpContext = new DefaultHttpContext();
            NovJobController.ControllerContext.HttpContext.Request.QueryString = new QueryString();

            var result = await NovJobController.GetNovJobs(new Paging());

            Assert.IsNotNull(result.Result);
            Assert.IsInstanceOfType(result.Result, typeof(OkObjectResult));
        }

        [TestMethod]
        public async Task ShouldReturnsReturnsNotFoundResult_GetNovJobById()
        {
            var mockQueryBus = new Mock<IQueryBus>();
            /*
            * suppressed Possible null reference return warning ,
            * test case intended to check returning null value by method
            * */
            mockQueryBus.Setup(x => x.Send<GetNovJobByIdQuery, NovJobDto>(It.IsAny<GetNovJobByIdQuery>()))
#pragma warning disable CS8603 // Possible null reference return.
                .ReturnsAsync(() => null);
#pragma warning restore CS8603 // Possible null reference return.

            var queryBus = mockQueryBus.Object;
            ICommandBus commandBus = new Mock<ICommandBus>().Object;
            NovJobController = new NovJobsController(commandBus, queryBus);
            var result = await NovJobController.GetNovJobById(1);

            Assert.IsNotNull(result.Result);
            Assert.IsInstanceOfType(result.Result, typeof(NotFoundObjectResult));
        }

        [TestMethod]
        public async Task ShouldReturnsNovJobWithOkStatus_GetNovJobId()
        {
            var mockQueryNus = new Mock<IQueryBus>();

            int id = novJobDtos.First().Id;
            var NovJobDto = novJobDtos.FirstOrDefault(o => o.Id == id);

            Assert.IsNotNull(NovJobDto);

            mockQueryNus.Setup(x => x.Send<GetNovJobByIdQuery, NovJobDto>(It.IsAny<GetNovJobByIdQuery>()))
                .ReturnsAsync(NovJobDto);

            var queryBus = mockQueryNus.Object;
            ICommandBus commandBus = new Mock<ICommandBus>().Object;
            NovJobController = new NovJobsController(commandBus, queryBus);
            var result = await NovJobController.GetNovJobById(id);

            Assert.IsNotNull(result.Result);
            Assert.IsInstanceOfType(result.Result, typeof(OkObjectResult));
        }

        [TestMethod]
        public async Task ShouldReturnsReturnsNotFoundResult_GetNovJobByJobId()
        {
            var mockQueryBus = new Mock<IQueryBus>();
            /*
            * suppressed Possible null reference return warning ,
            * test case intended to check returning null value by method
            * */
            mockQueryBus.Setup(x => x.Send<GetNovJobByJobNumberQuery, IEnumerable<NovJobDto>>(It.IsAny<GetNovJobByJobNumberQuery>()))
#pragma warning disable CS8603 // Possible null reference return.
                .ReturnsAsync(() => null);
#pragma warning restore CS8603 // Possible null reference return.

            var queryBus = mockQueryBus.Object;
            ICommandBus commandBus = new Mock<ICommandBus>().Object;
            NovJobController = new NovJobsController(commandBus, queryBus);
            var result = await NovJobController.GetNovJobByJobNumber(426809);

            Assert.IsNotNull(result.Result);
            Assert.IsInstanceOfType(result.Result, typeof(NotFoundObjectResult));
        }

        #endregion

        #region DomainService unit tests
        [TestMethod]
        public void ShouldReturnAllNovJob_GetNovJobs()
        {
            var novJobs = novJobService.GetNovJobs(null);
            var novJob = novJobs.FirstOrDefault();
            Assert.IsNotNull(novJob);
            Assert.AreEqual(3, novJobs.Count());
            Assert.IsNotNull(novJob);
            Assert.AreEqual(1, novJob.Id);
            Assert.AreEqual(true, novJob.IsActive);
            Assert.AreEqual(426809, novJob.JobNumber);
            Assert.AreEqual(5211865, novJob.BusinessUnit);
            Assert.AreEqual(Guid.Parse("E01D2659-8F78-E311-A7B6-0017A4771C44"), novJob.CorpRigId);
            Assert.AreEqual(5316553, novJob.Customer);
            Assert.AreEqual(1, novJob.ModuleId);
            Assert.AreEqual("CustomerTransfer", novJob.ModuleKey);
            Assert.IsNotNull(novJob.DateCreated);
            Assert.IsNotNull(novJob.DateModified);
            Assert.AreEqual("Test User", novJob.CreatedBy);
            Assert.AreEqual("Test User", novJob.ModifiedBy);
            Assert.AreEqual("JDE", novJob.CreatedSource);
            Assert.AreEqual("JDE", novJob.ModifiedSource);
        }

        [TestMethod]
        public void ShouldReturnNovJobForGivenId_GetNovJobById()
        {
            NovJob novJob = novJobService.GetNovJobById(3);

            Assert.IsNotNull(novJob);
            Assert.AreEqual(3, novJob.Id);
            Assert.AreEqual(true, novJob.IsActive);
            Assert.AreEqual(426837, novJob.JobNumber);
            Assert.AreEqual(5369941, novJob.BusinessUnit);
            Assert.AreEqual(Guid.Parse("E01D2659-8F78-E311-A7B6-0017A4771C44"), novJob.CorpRigId);
            Assert.AreEqual(113054, novJob.Customer);
            Assert.AreEqual(3, novJob.ModuleId);
            Assert.AreEqual("SalesOrder", novJob.ModuleKey);
            Assert.IsNotNull(novJob.DateCreated);
            Assert.IsNotNull(novJob.DateModified);
            Assert.AreEqual("Test User", novJob.CreatedBy);
            Assert.AreEqual("Test User", novJob.ModifiedBy);
            Assert.AreEqual("JDE", novJob.CreatedSource);
            Assert.AreEqual("JDE", novJob.ModifiedSource);
        }

        [TestMethod]
        public void ShouldCreateNovJobForGivenData_CreateNovJob()
        {
            try
            {
                string jsonFilePath = Path.Combine(".", "TestData", "CreateNovJob.json");
                NovJob novJob = JsonConvert.DeserializeObject<NovJob>
                (File.ReadAllText(jsonFilePath)) ?? new NovJob();
                novJobService.CreateNovJob(novJob);
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
            Assert.AreEqual(4, JobDBContext.NovJobs.Count());
        }

        [TestMethod]
        public void ShouldReturnNovJobForGivenId_GetNovJobByJobId()
        {
            var novJobList = novJobService.GetNovJobByJobNumber(426809);
            var novJob = novJobList.FirstOrDefault();
            Assert.IsNotNull(novJob);
            Assert.AreEqual(1, novJob.Id);
            Assert.AreEqual(true, novJob.IsActive);
            Assert.AreEqual(426809, novJob.JobNumber);
            Assert.AreEqual(5211865, novJob.BusinessUnit);
            Assert.AreEqual(Guid.Parse("E01D2659-8F78-E311-A7B6-0017A4771C44"), novJob.CorpRigId);
            Assert.AreEqual(5316553, novJob.Customer);
            Assert.AreEqual(1, novJob.ModuleId);
            Assert.AreEqual("CustomerTransfer", novJob.ModuleKey);
            Assert.IsNotNull(novJob.DateCreated);
            Assert.IsNotNull(novJob.DateModified);
            Assert.AreEqual("Test User", novJob.CreatedBy);
            Assert.AreEqual("Test User", novJob.ModifiedBy);
            Assert.AreEqual("JDE", novJob.CreatedSource);
            Assert.AreEqual("JDE", novJob.ModifiedSource);
        }


        #region Pagination
        /// <summary>
        ///  test Pagination with valid pagingParameters
        /// </summary>
        [TestMethod]
        public void ShouldReturnsNovJob_GetNovJobs()
        {
            pagingParameters = new Paging()
            {
                PageIndex = 0,
                PageSize = 2,
                SortColumn = "Id"
            };

            var NovJobs = novJobService.GetNovJobs(pagingParameters);
            NovJob? novJob = NovJobs.Items.FirstOrDefault();

            Assert.IsNotNull(novJob);
            Assert.AreEqual(2, NovJobs.Count());
            Assert.AreEqual(3, NovJobs.TotalNumberOfItems);
            Assert.AreEqual(2, NovJobs.TotalNumberOfPages);
            Assert.IsNotNull(novJob);
            Assert.AreEqual(1, novJob.Id);
            Assert.AreEqual(true, novJob.IsActive);
            Assert.AreEqual(426809, novJob.JobNumber);
            Assert.AreEqual(1, novJob.ModuleId);
            Assert.AreEqual("CustomerTransfer", novJob.ModuleKey);
            Assert.IsNotNull(novJob.DateCreated);
            Assert.IsNotNull(novJob.DateModified);
            Assert.AreEqual("Test User", novJob.CreatedBy);
            Assert.AreEqual("Test User", novJob.ModifiedBy);
            Assert.AreEqual("JDE", novJob.CreatedSource);
            Assert.AreEqual("JDE", novJob.ModifiedSource);
        }

        /// <summary>
        /// test Pagination with invalid SortColumn   .
        /// <returns>
        /// ArgumentException("Sort column SortColumn does not exist.");
        /// </returns>
        /// </summary>
        [TestMethod]
        public void ShouldThrowArgumentExceptionForGivenInvalidSortColumn_GetNovJobs()
        {
            pagingParameters = new Paging()
            {
                SortColumn = "SortColumn"
            };
            try
            {
                novJobService.GetNovJobs(pagingParameters);
                Assert.Fail();
            }
            catch (ArgumentException argumentException)
            {
                Assert.AreEqual("Sort column SortColumn does not exist.", argumentException.Message);
            }
        }

        #endregion

        #endregion

        [TestCleanup]
        public void TestCleanup()
        {
            Dispose();
        }

        private void ClearAndSeedTestData()
        {
            JobDBContext.Database.EnsureDeleted();
            JobDBContext.Database.EnsureCreated();
            string jsonFilePath = Path.Combine(".", "TestData", "NovJobsSeed.json");
            var NovJob = DeserializeJsonToObject<NovJob>(jsonFilePath);
            novJobDtos = DeserializeJsonToObject<NovJobDto>(jsonFilePath);
            JobDBContext.NovJobs.AddRange(NovJob);
            JobDBContext.SaveChanges();
        }
    }
}
