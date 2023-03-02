using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using Dapper;
namespace ThaiPaymentAPI.Models
{
    public class INETOrderPayment
    {
        public string orderId { get; set; }
        public string merchantId { get; set; }
        public string bankNo { get; set; }
        public string payType { get; set; }
        public string orderIdRef { get; set; }
        public string rcode { get; set; }
        public string rmsg { get; set; }
        public double TxnAmount { get; set; }
        public string txnDate { get; set; }
        public string txnTime { get; set; }
        public string ORDER_DESC { get; set; }
        public ErrorResponse Save()
        {
            var err = new ErrorResponse();
            try
            {
                using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["MainConnection"].ConnectionString))
                {
                    string sql = @"
BEGIN TRY
    BEGIN TRANSACTION

    DECLARE @AUTO_ID as int
    
    INSERT INTO [dbo].[INETOrderPayment] 
    (
        [orderId],
        [merchantId],
        [bankNo],
        [payType],
        [orderIdRef],
        [rcode],
        [rmsg],
        [TxnAmount],
        [txnDate],
        [txnTime],
        [ORDER_DESC]
    ) VALUES (
        @orderid,
        @merchantid,
        @bankno,
        @paytype,
        @orderidref,
        @rcode,
        @rmsg,
        @txnamount,
        @txndate,
        @txntime,
        @orderdesc
    )
        
    SET @AUTO_ID=SCOPE_IDENTITY()        

    SELECT * FROM [dbo].[INETOrderPayment] WHERE [id]=@AUTO_ID

    COMMIT TRANSACTION
END TRY
BEGIN CATCH
    ROLLBACK TRAN
END CATCH
";
                  
                    var dtl = cn.QueryFirstOrDefault<INETOrderPayment>(sql, new
                    {
                        @merchantid = this.merchantId,
                        @rcode = this.rcode,
                        @rmsg = this.rmsg,
                        @orderid = this.orderId,
                        @orderidref = this.orderIdRef,
                        @txnamount = this.TxnAmount,
                        @paytype = this.payType,
                        @bankno = this.bankNo,
                        @txndate = this.txnDate,
                        @txntime = this.txnTime,
                        @orderdesc = this.ORDER_DESC
                    });

                    if (dtl.orderId.Equals(this.orderId))
                    {
                        err.error = "OK";
                        err.data = JsonConvert.SerializeObject(dtl);
                        return err;
                    }
                    err.error = "Cannot save Data";
                    err.data = JsonConvert.SerializeObject(this);
                    return err;
                }
            }
            catch (Exception e)
            {
                err.error = e.Message;
                err.data = JsonConvert.SerializeObject(this);
                return err;
            }
        }

    }
}