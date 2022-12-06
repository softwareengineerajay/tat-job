using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NOV.ES.TAT.Job.API.Migrations
{
    public partial class AllViewCreation_Job : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                @"CREATE VIEW [job].[NovJobDetailsView]  
                AS  
                SELECT DISTINCT j.Id  
                 ,nj.JobNumber AS NovJobNumber  
                 ,j.JobDescription  
                 ,c.Id AS CompanyId  
                 ,c.Code AS CompanyCode  
                 ,c.Name AS CompanyName  
                 ,BU.Id AS BusinessUnitId  
                 ,BU.BusinessUnit AS RevenueBUCode  
                 ,BU.BusinessUnitName AS RevenueBuName  
                 ,Customer.Id AS CustomerId  
                 ,Customer.CustomerId AS CustomerCode  
                 ,Customer.CustomerName AS CustomerName  
                 ,Rig.RigConfigId AS CorpRigId  
                 ,Rig.ContractorName  
                 ,Rig.RigName  
                 ,j.PlannedStartDate  
                 ,j.PlannedEndDate  
                 ,j.DateOpened AS ActualStartDate  
                 ,j.DateClosed AS ActualEndDate  
                 ,nj.IsActive  
                 ,CASE   
                  WHEN j.ActiveStatus = 1  
                   THEN 'Open'  
                  WHEN j.ActiveStatus = 0  
                   THEN 'Closed'  
                  END AS JobStatus  
                FROM [job].[NovJob] nj  
                INNER JOIN [MasterDataManagement].[dbo].[job] j ON j.jobid = nj.jobnumber  
                LEFT JOIN [MasterDataManagement].[dbo].[BusinessUnit] BU ON BU.BusinessUnit = nj.BusinessUnit  
                 AND BU.IsActive = 1  
                LEFT JOIN [MasterDataManagement].[dbo].[Rig] Rig ON Rig.RigConfigId = nj.CorpRigId  
                LEFT JOIN [MasterDataManagement].[dbo].[Well] Well ON Well.Id = nj.CorpWellSiteId  
                LEFT JOIN [MasterDataManagement].[dbo].[Company] c ON c.code = BU.company  
                 AND c.isactive = 1  
                LEFT JOIN [MasterDataManagement].[dbo].[Customer] Customer ON Customer.CustomerId = nj.Customer  
                LEFT JOIN [customertransfer].[CustomerTransferSlip] ct ON nj.ModuleKey = 'PackingSlip'  
                 AND ct.Id = nj.ModuleId  
                 AND ct.IsCompleted = 1"
                );

            migrationBuilder.Sql(
              @"CREATE VIEW [job].[SalesOrderDetailsView]
                AS
                SELECT Id
	                ,SalesOrderNumber
	                ,SalesOrderDate
	                ,CostCenterCode + ' - ' + CostCenterName as FromRevenueCC
	                ,RigName
	                ,SalesZone
	                ,SalesPerson
	                ,Currency
	                ,Total
	                ,0 AS 'ItemNumber'
	                ,0 AS 'CreditItems'
	                ,0 AS 'SalesOrderCount'
	                ,'' AS 'SalesOrderType'
	                ,ErpInvoiceNumber as InvoiceNumber
	                ,erpjobnumber as JobNumber
	                ,IsActive
                FROM [salesorder].[SalesOrder]"
              );


            migrationBuilder.Sql(
				@"CREATE VIEW [job].[UsageDetailsView]
				AS
				SELECT Id
					,UsageNumber
					,ItemName
					,ItemClassName
					,SerialNumber
					,RevenueBuCode
					,RevenueBuName
					,SendingBuCode
					,SendingBuName
					,DateIn
					,DateOut
					,JobNumber
					,IsActive
					,CASE 
						WHEN paymentType = 'JDE Invoice'
							THEN CONCAT (
									paymentType
									,' : '
									,ErpInvoiceNumber
									,+ ' (' + REPLACE(CONVERT(NVARCHAR, ErpSalesOrderDate, 106), ' ', '-') + ')'
									)
						WHEN paymentType = 'JDESO'
							THEN CONCAT (
									paymentType
									,' : '
									,ErpSalesOrderNumber
									,+ ' (' + REPLACE(CONVERT(NVARCHAR, ErpSalesOrderDate, 106), ' ', '-') + ')'
									)
						WHEN paymentType = 'JDE SO'
							THEN CONCAT (
									paymentType
									,' : '
									,ErpInvoiceNumber
									,+ ' (' + REPLACE(CONVERT(NVARCHAR, ErpSalesOrderDate, 106), ' ', '-') + ')'
									)
						END AS LastBillingInfo
				FROM (
					SELECT DISTINCT u.id
						,u.UsageNumber
						,i.name AS ItemName
						,ic.Name AS ItemClassName
						,i.SerialNumber
						,bu.BusinessUnit AS RevenueBuCode
						,cast(bu.BusinessUnit as nvarchar(15)) + '-' +  bu.BusinessUnitName AS RevenueBuName
						,j.SendingBU AS SendingBuCode
						,cast(j.SendingBU as nvarchar(15)) + '-' + j.SendingBuName AS SendingBuName
						,u.DateInforService AS DateIn
						,u.DateOuttoField AS DateOut
						,uw.ErpInvoiceDate
						,uw.ErpSalesOrderDate
						,uw.ErpInvoiceNumber
						,uw.ErpSalesOrderNumber
						,j.JobId AS JobNumber
						,u.IsActive
						,CASE 
							WHEN uw.ErpInvoiceDate != ''
								AND convert(DATE, '1/1/1900') < uw.ErpInvoiceDate
								THEN 'JDE Invoice'
							WHEN uw.ErpSalesOrderDate != ''
								AND convert(DATE, '1/1/1900') < uw.ErpSalesOrderDate
								THEN 'JDESO'
							WHEN uw.ErpInvoiceDate = ''
								AND uw.ErpSalesOrderDate != ''
								AND uw.ErpSalesOrderDate > convert(DATE, '1/1/1900')
								THEN 'JDE SO'
							END paymentType
					FROM [usage].[Usage] u
					INNER JOIN [MasterDataManagement].[dbo].[Job] j ON j.Id = u.NovJobNumber
						AND j.IsActive = 1
					INNER JOIN [item].[item] i ON u.ItemId = i.id
						AND i.IsActive = 1
					INNER JOIN [item].[ItemClass] ic ON ic.id = i.ItemClassId
					LEFT JOIN [MasterDataManagement].[dbo].[BusinessUnit] bu ON bu.Id = u.BusinessUnitId
						AND bu.IsActive = 1
					LEFT JOIN [usage].[UsageWellSite] uw ON uw.usageid = u.id
						AND uw.IsActive = 1
					) AS UsageData"
				    );
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"DROP VIEW [job].[NovJobDetailsView];");
            migrationBuilder.Sql(@"DROP VIEW [job].[SalesOrderDetailsView];");
            migrationBuilder.Sql(@"DROP VIEW [job].[UsageDetailsView];");
        }
    }
}
