using Microsoft.AspNetCore.Http;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using NOV.ES.Framework.Core.Data;
using NOV.ES.TAT.Job.Infrastructure;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Claims;

namespace NOV.ES.TAT.Job.Test
{
    public class TestBase : IDisposable
    {
        public JobDBContext JobDBContext { get; set; }
        public TestBase()
        {
            JobDBContext = CreateDbContext();
        }

        private static JobDBContext CreateDbContext()
        {
            HttpContextAccessor httpContextAccessor = GetHttpContextAccessor();
            return new JobDBContext(CreateDbContextOptions<JobDBContext>(), httpContextAccessor);
        }

        private static HttpContextAccessor GetHttpContextAccessor()
        {
            HttpContextAccessor httpContextAccessor = new HttpContextAccessor();
            httpContextAccessor.HttpContext = new DefaultHttpContext()
            {
                User = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, "Test User"),
                    new Claim(ClaimTypes.Role, "Role")
                }))
            };
            return httpContextAccessor;
        }

        public static DbContextOptions<T> CreateDbContextOptions<T>() where T : BaseContext
        {
            var connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();
            connection.CreateFunction("newid", () => { return 1; });
            return new DbContextOptionsBuilder<T>().UseSqlite(connection).Options;

        }
        public static IEnumerable<T> DeserializeJsonToObject<T>(string jsonDataPath)
        {
            return JsonConvert.DeserializeObject<List<T>>
                           (File.ReadAllText(jsonDataPath)) ?? new List<T>();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing)
        {
            JobDBContext.Dispose();
        }
    }

}
