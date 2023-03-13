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
        public IEnumerable<OrderGroup> Gets()
        {
            using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["MainConnection"].ConnectionString))
            {
                return cn.Query<OrderGroup>("SELECT * FROM [dbo].[Project_OrderGroup]");
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
        public IEnumerable<OrderDetail> Gets()
        {
            using(SqlConnection cn=new SqlConnection(ConfigurationManager.ConnectionStrings["MainConnection"].ConnectionString))
            {
                return cn.Query<OrderDetail>("SELECT * FROM [dbo].[Project_OrderDetail]");
            }
        }
        public ErrorResponse Save()
        {
            try 
            {
                using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["MainConnection"].ConnectionString))
                {
                    string sql = @"
BEGIN TRY
	BEGIN TRANSACTION
	IF EXISTS(SELECT order_id FROM [dbo].[Project_OrderDetail] WHERE order_id=@order_id) 
	BEGIN 
		UPDATE [dbo].[Project_OrderDetail] SET
		order_name=@order_name,
		order_desc=@order_desc,
		order_pic=@order_pic,
		currency=@order_currency,
		order_amount=@order_amount,
		order_group=@order_group,
		order_target=@order_target,
		order_actual=@order_actual,
		isactive=@isactive
		WHERE order_id=@order_id
	END
	ELSE
	BEGIN
		INSERT INTO [dbo].[Project_OrderDetail]
		(
		order_id,
		order_name,
		order_desc,
		order_pic,
		currency,
		order_amount,
		order_group,
		order_target,
		order_actual,
		isactive
		) VALUES (
		@order_id,
		@order_name,
		@order_desc,
		@order_pic,
		@order_currency,
		@order_amount,
		@order_group,
		@order_target,
		@order_actual,
		@isactive
		)
	END 
	COMMIT TRANSACTION
END TRY
BEGIN CATCH
ROLLBACK TRANSACTION

INSERT INTO [dbo].[ActionLog] (log_source,log_action,log_data,log_message,log_stacktrace,log_error)
SELECT ERROR_PROCEDURE() AS LogSource,
ERROR_STATE() AS ErrorState,
ERROR_SEVERITY() AS LogData,
ERROR_MESSAGE() AS LogMessage,
ERROR_LINE() AS ErrorLine,1;               

END CATCH
SELECT * FROM [dbo].[Project_OrderDetail] WHERE order_id=@order_id
";
                    var o = cn.QueryFirstOrDefault<OrderDetail>(sql, new
                    {
                        @order_id = this.order_id,
                        @order_group = this.order_group,
                        @order_name = this.order_name,
                        @order_desc = this.order_desc,
                        @order_pic = this.order_pic,
                        @order_amount = this.order_amount,
                        @order_currency = this.currency,
                        @order_target = this.order_target,
                        @order_actual = this.order_actual,
                        @isactive = this.isactive
                    });
                    if (o.order_id.Equals(this.order_id))
                    {
                        return new ErrorResponse()
                        {
                            success = true,
                            data = JsonConvert.SerializeObject(this),
                            error = "Save Complete"
                        };
                    }
                    else
                    {
                        return new ErrorResponse()
                        {
                            success = false,
                            data = JsonConvert.SerializeObject(this),
                            error = "Save Failed"
                        };
                    }
                }
            }
            catch (Exception e)
            {
                return new ErrorResponse()
                {
                    success = false,
                    data = JsonConvert.SerializeObject(this),
                    error = e.Message
                };
            }
        }
    }
}