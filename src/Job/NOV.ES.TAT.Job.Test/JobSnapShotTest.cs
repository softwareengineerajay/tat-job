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
    public class JobSnapShotTest : TestBase
    {
        private readonly IJobSnapShotService jobSnapShotService;
        private Paging pagingParameters;
        private JobSnapShotsController JobSnapShotController;
        private IEnumerable<JobSnapShotDto> jobSnapShotDtos = new List<JobSnapShotDto>();
        public JobSnapShotTest() : base()
        {
            jobSnapShotService = new JobSnapShotService(new JobSnapShotQueryRepository(JobDBContext));

            pagingParameters = new Paging();
            IQueryBus queryBus = new Mock<IQueryBus>().Object;
            JobSnapShotController = new JobSnapShotsController(queryBus);
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
            var result = JobSnapShotController.ServiceName();

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            Assert.AreEqual("JobSnapShot API Service.", ((OkObjectResult)result).Value);
        }

        [TestMethod]
        public async Task ShouldReturnsEmptyListOfJobSnapShotWithOkStatus_GetJobSnapShots()
        {
            var mockQueryNus = new Mock<IQueryBus>();
            jobSnapShotDtos = new List<JobSnapShotDto>();
            mockQueryNus.Setup(x => x.Send<GetPaginationJobSnapShotsQuery, PagedResult<JobSnapShotDto>>(It.IsAny<GetPaginationJobSnapShotsQuery>()))
                .ReturnsAsync(new PagedResult<JobSnapShotDto>(jobSnapShotDtos, jobSnapShotDtos.Count(), pagingParameters));

            var queryBus = mockQueryNus.Object;
            JobSnapShotController = new JobSnapShotsController(queryBus);

            JobSnapShotController.ControllerContext.HttpContext = new DefaultHttpContext();
            JobSnapShotController.ControllerContext.HttpContext.Request.QueryString = new QueryString();
            var result = await JobSnapShotController.GetJobSnapShots(new Paging());

            Assert.IsNotNull(result.Result);
            Assert.IsInstanceOfType(result.Result, typeof(OkObjectResult));
        }

        [TestMethod]
        public async Task ShouldReturnsListofJobSnapShotWithOkStatus_GetJobSnapShots()
        {
            var mockQueryNus = new Mock<IQueryBus>();

            mockQueryNus.Setup(x => x.Send<GetPaginationJobSnapShotsQuery, PagedResult<JobSnapShotDto>>(It.IsAny<GetPaginationJobSnapShotsQuery>()))
                .ReturnsAsync(new PagedResult<JobSnapShotDto>(jobSnapShotDtos, jobSnapShotDtos.Count(), new Paging()));

            var queryBus = mockQueryNus.Object;
            JobSnapShotController = new JobSnapShotsController(queryBus);
            JobSnapShotController.ControllerContext.HttpContext = new DefaultHttpContext();
            JobSnapShotController.ControllerContext.HttpContext.Request.QueryString = new QueryString();

            var result = await JobSnapShotController.GetJobSnapShots(new Paging());

            Assert.IsNotNull(result.Result);
            Assert.IsInstanceOfType(result.Result, typeof(OkObjectResult));
        }

        [TestMethod]
        public async Task ShouldReturnsReturnsNotFoundResult_GetJobSnapShotById()
        {
            var mockQueryBus = new Mock<IQueryBus>();
            /*
            * suppressed Possible null reference return warning ,
            * test case intended to check returning null value by method
            * */
            mockQueryBus.Setup(x => x.Send<GetJobSnapShotByIdQuery, JobSnapShotDto>(It.IsAny<GetJobSnapShotByIdQuery>()))
#pragma warning disable CS8603 // Possible null reference return.
                .ReturnsAsync(() => null);
#pragma warning restore CS8603 // Possible null reference return.

            var queryBus = mockQueryBus.Object;
            JobSnapShotController = new JobSnapShotsController(queryBus);
            var result = await JobSnapShotController.GetJobSnapShotById(1);

            Assert.IsNotNull(result.Result);
            Assert.IsInstanceOfType(result.Result, typeof(NotFoundObjectResult));
        }

        [TestMethod]
        public async Task ShouldReturnsJobSnapShotWithOkStatus_GetJobSnapShotId()
        {
            var mockQueryNus = new Mock<IQueryBus>();

            int id = jobSnapShotDtos.First().Id;
            var JobSnapShotDto = jobSnapShotDtos.FirstOrDefault(o => o.Id == id);

            Assert.IsNotNull(JobSnapShotDto);

            mockQueryNus.Setup(x => x.Send<GetJobSnapShotByIdQuery, JobSnapShotDto>(It.IsAny<GetJobSnapShotByIdQuery>()))
                .ReturnsAsync(JobSnapShotDto);

            var queryBus = mockQueryNus.Object;
            JobSnapShotController = new JobSnapShotsController(queryBus);
            var result = await JobSnapShotController.GetJobSnapShotById(id);

            Assert.IsNotNull(result.Result);
            Assert.IsInstanceOfType(result.Result, typeof(OkObjectResult));
        }

        #endregion

        #region DomainService unit tests
        [TestMethod]
        public void ShouldReturnAllJobSnapShot_GetJobSnapShots()
        {
            var jobSnapShots = jobSnapShotService.GetJobSnapShots(null);
            var jobSnapShot = jobSnapShots.FirstOrDefault();
            Assert.IsNotNull(jobSnapShot);
            Assert.AreEqual(4, jobSnapShots.Count());
            Assert.IsNotNull(jobSnapShot);
            Assert.AreEqual(1, jobSnapShot.Id);
            Assert.AreEqual(1, jobSnapShot.EventId);
            Assert.AreEqual(1, jobSnapShot.JobId);
            Assert.AreEqual("Previous Data 4", jobSnapShot.PreviousData);
            Assert.AreEqual("Current Data 4", jobSnapShot.CurrentData);
            Assert.AreEqual("Change Data 4", jobSnapShot.ChangedData);
            
        }

        [TestMethod]
        public void ShouldReturnJobSnapShotForGivenId_GetJobSnapShotById()
        {
            JobSnapShot jobSnapShot = jobSnapShotService.GetJobSnapShotById(2);

            Assert.IsNotNull(jobSnapShot);
            Assert.AreEqual(2, jobSnapShot.Id);
            Assert.AreEqual(2, jobSnapShot.EventId);
            Assert.AreEqual(1, jobSnapShot.JobId);
            Assert.AreEqual("Previoud Data 1", jobSnapShot.PreviousData);
            Assert.AreEqual("Current Data1", jobSnapShot.CurrentData);
            Assert.AreEqual("Change Data 1", jobSnapShot.ChangedData);
            
        }

        
        #region Pagination
        /// <summary>
        ///  test Pagination with valid pagingParameters
        /// </summary>
        [TestMethod]
        public void ShouldReturnsJobSnapShot_GetJobSnapShots()
        {
            pagingParameters = new Paging()
            {
                PageIndex = 0,
                PageSize = 2,
                SortColumn = "Id"
            };

            var JobSnapShots = jobSnapShotService.GetJobSnapShots(pagingParameters);
            JobSnapShot? jobSnapShot = JobSnapShots.Items.FirstOrDefault();

            Assert.IsNotNull(jobSnapShot);
            Assert.AreEqual(2, JobSnapShots.Count());
            Assert.AreEqual(4, JobSnapShots.TotalNumberOfItems);
            Assert.AreEqual(2, JobSnapShots.TotalNumberOfPages);
            Assert.IsNotNull(jobSnapShot);
            Assert.AreEqual(1, jobSnapShot.Id);
            Assert.AreEqual(1, jobSnapShot.EventId);
            Assert.AreEqual(1, jobSnapShot.JobId);
            Assert.AreEqual("Previous Data 4", jobSnapShot.PreviousData);
            Assert.AreEqual("Current Data 4", jobSnapShot.CurrentData);
            Assert.AreEqual("Change Data 4", jobSnapShot.ChangedData);
           
        }

        /// <summary>
        /// test Pagination with invalid SortColumn   .
        /// <returns>
        /// ArgumentException("Sort column SortColumn does not exist.");
        /// </returns>
        /// </summary>
        [TestMethod]
        public void ShouldThrowArgumentExceptionForGivenInvalidSortColumn_GetJobSnapShots()
        {
            pagingParameters = new Paging()
            {
                SortColumn = "SortColumn"
            };
            try
            {
                jobSnapShotService.GetJobSnapShots(pagingParameters);
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
            JobDBContext.NovJobs.AddRange(NovJob);
            JobDBContext.SaveChanges();

            jsonFilePath = Path.Combine(".", "TestData", "JobSnapShotsSeed.json");
            var JobSnapShot = DeserializeJsonToObject<JobSnapShot>(jsonFilePath);

            jobSnapShotDtos = DeserializeJsonToObject<JobSnapShotDto>(jsonFilePath);
            JobDBContext.JobSnapShots.AddRange(JobSnapShot);
            JobDBContext.SaveChanges();
        }
    }
}
