using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Data.SqlClient;
using Dapper;
using Newtonsoft.Json;
namespace ThaiPaymentAPI.Models
{
    public class OrderGroup
    {
        public string order_group { get; set; }
        public string order_groupname { get; set; }
        public string order_groupdesc { get; set; }
        public bool isactive { get; set; }
        public IEnumerable<OrderGroup> Gets(bool isactive=true)
        {
            using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["MainConnection"].ConnectionString))
            {
                return cn.Query<OrderGroup>("SELECT * FROM [dbo].[Project_OrderGroup] WHERE [isactive]=@isactive",new { @isactive=isactive });
            }
        }
        public OrderGroup GetValue(string order_group)
        {
            using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["MainConnection"].ConnectionString))
            {
                return cn.QueryFirstOrDefault<OrderGroup>("SELECT * FROM [dbo].[Project_OrderGroup] WHERE [order_group]=@groupcode", new { @groupcode = order_group });
            }
        }
        public ErrorResponse Save()
        {
            using(SqlConnection cn=new SqlConnection(ConfigurationManager.ConnectionStrings["MainConnection"].ConnectionString))
            {
                string sql = @"
BEGIN TRY
    BEGIN TRANSACTION
        IF EXISTS(SELECT * FROM [dbo].[Project_OrderGroup] WHERE [order_group]=@groupcode)
        BEGIN
            UPDATE [dbo].[Project_OrderGroup]
            SET [order_groupname]=@groupname,
                [order_groupdesc]=@groupdesc,
                [isactive]=@isactive
            WHERE [order_group]=@groupcode
        END
        ELSE
        BEGIN
            INSERT INTO [dbo].[Project_OrderGroup]
            ([order_group],[order_groupname],[order_groupdesc],[isactive])
            VALUES 
            (@groupcode,@groupname,@groupdesc,@isactive)
        END
    COMMIT TRANSACTION
END TRY
BEGIN CATCH
    ROLLBACK TRANSACTION
END CATCH
SELECT * FROM [dbo].[Project_OrderGroup] WHERE [order_group]=@groupcode
";
                try
                {
                    var o=cn.QueryFirstOrDefault<OrderGroup>(sql, new
                    {
                        @groupcode = this.order_group,
                        @groupname = this.order_groupname,
                        @groupdesc = this.order_groupdesc,
                        @isactive = this.isactive
                    });
                    if (o!=null)
                    {
                        return new ErrorResponse()
                        {
                            success = true,
                            error = "OK",
                            data = JsonConvert.SerializeObject(this)
                        };
                    } else
                    {
                        return new ErrorResponse()
                        {
                            success = false,
                            error = "Cannot Save Data",
                            data = JsonConvert.SerializeObject(this)
                        };
                    }
                }
                catch (Exception e)
                {
                    return new ErrorResponse()
                    {
                        success = false,
                        error =e.Message,
                        data = JsonConvert.SerializeObject(this)
                    };
                }
            }
        }
    }
    public class OrderDetail
    {
        public string order_id { get; set; }
        public string order_name { get; set; }
        public string order_desc { get; set; }
        public string order_pic { get; set; }
        public string currency { get; set; }
        public double order_amount { get; set; }
        public string order_group { get; set; }
        public double order_target { get; set; }
        public double order_actual { get; set; }
        public bool isactive { get; set; }
    }
}