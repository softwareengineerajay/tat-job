using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using NOV.ES.Framework.Core.Data;
using NOV.ES.TAT.Job.Domain;
using NOV.ES.TAT.Job.Domain.ReadModel;

namespace NOV.ES.TAT.Job.Infrastructure
{
    public class JobDBContext : BaseContext
    {
        public JobDBContext(DbContextOptions<JobDBContext> dbContextOptions, IHttpContextAccessor httpContextAccessor)
            : base(dbContextOptions)
        {
            if (httpContextAccessor != null && httpContextAccessor.HttpContext != null)
            {
                IIdentityService identityService = new IdentityService(httpContextAccessor);
                UserProvider = identityService.GetUserName();
            }
        }
        public virtual DbSet<ClientRequest> ClientRequests { get; set; }
        public virtual DbSet<NovJob> NovJobs { get; set; }
        public virtual DbSet<JobSnapShot> JobSnapShots { get; set; }
        public virtual DbSet<NovJobDetailsView> NovJobDetailsView { get; set; }
        public virtual DbSet<UsageDetailsView> UsageDetailsView { get; set; }
        // ToDo: Map to view
        public virtual DbSet<FieldTransferSlipDetailsView> FieldTransferSlipDetailsView { get; set; }
        public virtual DbSet<SalesOrderDetailsView> SalesOrderDetailsView { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<NovJob>().Property(x => x.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<NovJob>().Property(x => x.IsActive).HasDefaultValueSql("1");
            modelBuilder.Entity<NovJob>().Property(x => x.DateCreated).HasDefaultValueSql("GETUTCDATE()");
            modelBuilder.Entity<NovJob>().Property(x => x.DateModified).HasDefaultValueSql("GETUTCDATE()");
            modelBuilder.Entity<JobSnapShot>().Property(x => x.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<JobSnapShot>().Property(x => x.IsActive).HasDefaultValueSql("1");
            modelBuilder.Entity<JobSnapShot>().Property(x => x.DateCreated).HasDefaultValueSql("GETUTCDATE()");
            modelBuilder.Entity<JobSnapShot>().Property(x => x.DateModified).HasDefaultValueSql("GETUTCDATE()");

            modelBuilder.Entity<ClientRequest>().ToTable("ClientRequest", "job");

            // Views
            modelBuilder.Entity<NovJobDetailsView>().ToView("NovJobDetailsView", "job");
            modelBuilder.Entity<UsageDetailsView>().ToView("UsageDetailsView", "job");
            modelBuilder.Entity<SalesOrderDetailsView>().ToView("SalesOrderDetailsView", "job");
            modelBuilder.Entity<FieldTransferSlipDetailsView>().ToView("FieldTransferSlipDetailsView", "job");

        }

    }
}
