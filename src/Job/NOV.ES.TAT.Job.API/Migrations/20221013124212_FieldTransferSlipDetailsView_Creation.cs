using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NOV.ES.TAT.Job.API.Migrations
{
    public partial class FieldTransferSlipDetailsView_Creation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                @"CREATE VIEW [job].[FieldTransferSlipDetailsView]
					AS
					SELECT Id
					,FieldTransferNumber
						,[TransferDate]
						,[FromErpJobNumber]
						,'' as SourceType
						,[ToErpJobNumber]
						,[FromCompanyCode]
						,[FromCompanyName]
						,[ToCompanyCode]
						,[ToCompanyName]
						,[FromCcCode]
						,cast(FromCcCode as nvarchar(15))+ '-' + FromCcName as FromCcName
						,[ToCcCode]
						,cast(ToCcCode as nvarchar(15)) + '-' + ToCcName as ToCcName
						,[FromCustomerCode]
						,cast(FromCustomerCode as nvarchar(15))+ '-' + FromCustomerName as FromCustomerName
						,[ToCustomerCode]
						,cast(ToCustomerCode as nvarchar(15))+ '-' + ToCustomerName as ToCustomerName
						,[FromRigName]
						,[ToRigName]
						,[WellName]
						,[ContractorName]
						,[IsActive]
					FROM [fieldtransfer].[FieldTransferSlip]"
                 );
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"DROP VIEW [job].[NovJobDetailsView];");
        }
    }
}
