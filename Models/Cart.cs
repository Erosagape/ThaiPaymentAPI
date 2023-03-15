using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using Dapper;
using System.Configuration;
using Newtonsoft.Json;
namespace ThaiPaymentAPI.Models
{
    public class ShoppingCart
    {
        public string from_ip { get; set; }
        public string from_browser { get; set; }
        public string from_user { get; set; }
        public int seq { get; set; }
        public DateTime order_date { get; set; }
        public string order_id { get; set; }
        public int order_qty { get; set; }
        public double order_price { get; set; }
        public double order_amount { get; set; }
        public string currency_payment { get; set; }
        public double exchangerate { get; set; }
        public string currency_convert { get; set; }
        public double order_total { get; set; }
        public int order_status { get; set; }
        public string payment_no { get; set; }
        public DateTime payment_date { get; set; }
        public string receipt_no { get; set; }
        public DateTime receipt_date { get; set; }
        public string order_pic
        {
            get
            {
                return new OrderDetail().Gets().Where(e => e.order_id.Equals(this.order_id)).FirstOrDefault().order_pic;
            }
        }
        public string order_name
        {
            get
            {
                return new OrderDetail().Gets().Where(e => e.order_id.Equals(this.order_id)).FirstOrDefault().order_name;
            }
        }
        public string order_group
        {
            get
            {
                return new OrderDetail().Gets().Where(e => e.order_id.Equals(this.order_id)).FirstOrDefault().order_group;
            }
        }
        public IEnumerable<ShoppingCart> Gets(string ip,string user,string agent,bool getall=false)
        {
            using(SqlConnection cn=new SqlConnection(ConfigurationManager.ConnectionStrings["MainConnection"].ConnectionString))
            {
                var sql = "SELECT * FROM [dbo].[Project_ShoppingCart] WHERE ";
                if (getall)
                {
                    sql += "[order_status]>=0";
                } else
                {
                    sql += "[order_status]< 2";
                }
                sql += " AND [from_ip]=@ip";
                sql += " AND [from_user]=@user";
                sql += " AND [from_browser]=@agent";
                return cn.Query<ShoppingCart>(sql,new { 
                    @ip=ip,
                    @user=user,
                    @agent=agent
                });
            }
        }
        public ErrorResponse Delete(string ip, string user, string agent,int seq)
        {
            try
            {
                using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["MainConnection"].ConnectionString))
                {
                    var i = cn.Execute("UPDATE [dbo].[Project_ShoppingCart] SET [order_status]=99 WHERE [from_ip]=@ip AND [from_user]=@user AND [from_browser]=@agent AND [seq]=@seq", new
                    {
                        @ip = ip,
                        @user = user,
                        @agent = agent,
                        @seq = seq
                    });
                    return new ErrorResponse()
                    {
                        success = true,
                        error = i + " row deleted",
                        data = JsonConvert.SerializeObject(this)
                    };
                }
            }
            catch (Exception e)
            {
                return new ErrorResponse()
                {
                    success = false,
                    error = e.Message,
                    data = JsonConvert.SerializeObject(this)
                };
            }
        }
        public ErrorResponse Save(string ip, string user, string agent)
        {
            try
            {
                using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["MainConnection"].ConnectionString))
                {
                    string sql = @"
BEGIN TRY
    BEGIN TRANSACTION
    
    IF EXISTS(SELECT * FROM [dbo].[Project_ShoppingCart] WHERE [from_ip]=@ip AND [from_user]=@user AND [from_browser]=@agent AND [seq]=@seq)
    BEGIN
        UPDATE [dbo].[Project_ShoppingCart] SET
        from_ip=@ip,
        from_browser=@agent,
        from_user=@user,
        seq=@seq,
        order_date=@orderdate,
        order_id=@orderid,
        order_qty=@orderqty,
        order_price=@orderprice,
        order_amount=@orderamount,
        currency_payment=@currencypay,
        exchangerate=@exchangeratepay,
        currency_convert=@currencyrec,
        order_total=@ordertotal,
        order_status=@orderstatus,
        payment_no=@paymentno,
        payment_date=@paymentdate,
        receipt_no=@receiptno,
        receipt_date=@receiptdate
        WHERE [from_ip]=@ip AND [from_user]=@user AND [from_browser]=@agent AND [seq]=@seq
    END
    ELSE
    BEGIN
        INSERT INTO [dbo].[Project_ShoppingCart] (
            from_ip,
            from_browser,
            from_user,
            seq,
            order_date,
            order_id,
            order_qty,
            order_price,
            order_amount,
            currency_payment,
            exchangerate,
            currency_convert,
            order_total,
            order_status,
            payment_no,
            payment_date,
            receipt_no,
            receipt_date
        ) VALUES (
            @ip,
            @agent,
            @user,
            @seq,
            @orderdate,
            @orderid,
            @orderqty,
            @orderprice,
            @orderamount,
            @currencypay,
            @exchangeratepay,
            @currencyrec,
            @ordertotal,
            @orderstatus,
            @paymentno,
            @paymentdate,
            @receiptno,
            @receiptdate
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
SELECT * FROM [dbo].[Project_ShoppingCart] WHERE [order_status]<2
";
                    sql += " AND [from_ip]=@ip";
                    sql += " AND [from_user]=@user";
                    sql += " AND [from_browser]=@agent";
                    sql += " AND [seq]=@seq";
                    var o = cn.QueryFirstOrDefault<ShoppingCart>(sql, new
                    {
                        @ip=ip,
                        @user=user,
                        @agent=agent,
                        @seq=this.seq,
                        @orderid=this.order_id,
                        @orderdate=this.order_date,
                        @orderqty=this.order_qty,
                        @orderprice=this.order_price,
                        @orderamount=this.order_amount,
                        @currencypay=this.currency_payment,
                        @exchangeratepay=this.exchangerate,
                        @currencyrec=this.currency_convert,
                        @ordertotal=this.order_total,
                        @orderstatus=this.order_status,
                        @paymentno=this.payment_no,
                        @paymentdate=this.payment_date,
                        @receiptno=this.receipt_no,
                        @receiptdate=this.receipt_date
                    });
                    if (o.seq.Equals(this.seq))
                    {
                        return new ErrorResponse()
                        {
                            success = true,
                            data = JsonConvert.SerializeObject(this),
                            error = "OK"
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
            } catch (Exception e)
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