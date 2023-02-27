using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using Dapper;
namespace ThaiPaymentAPI.Models
{
    public class INETPaymentResponse
    {
        [JsonProperty("event")]
        public string event_text { get; set; }
        public string merchant_id { get; set; }
        public string timestamp { get; set; }
        public INETPaymentResponseDetail detail { get; set; }
        public bool Save()
        {
            try
            {
                using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["MainConnection"].ConnectionString))
                {
                    string sql = @"
BEGIN TRY
    BEGIN TRANSACTION

    DECLARE @AUTO_ID as int
    
    INSERT INTO [dbo].[INET_Payment_Response] 
    (
        [timestamp],
        [merchant_id],
        [event]
    ) VALUES (
        @timestamp,
        @merchant_id,
        @event
    )
        
    SET @AUTO_ID=SCOPE_IDENTITY()
        
    INSERT INTO [dbo].[INET_Payment_Response_Detail] 
    (
        [id],
        [response_code],
        [response_message],
        [merchant_id],
        [order_id],
        [payment_reference_id],
        [receive_amount],
        [payment_type],
        [payment_acquirer_bank],
        [transaction_date],
        [transaction_time],
        [order_description]
    ) VALUES (
        @AUTO_ID,
        @response_code,
        @response_message,
        @merchant_id2,
        @order_id,
        @payment_reference_id,
        @receive_amount,
        @payment_type,
        @payment_acquirer_bank,
        @transaction_date,
        @transaction_time,
        @order_description
    )

    SELECT * FROM [dbo].[INET_Payment_Response_Detail] WHERE [id]=@AUTO_ID

    COMMIT TRANSACTION
END TRY
BEGIN CATCH
    ROLLBACK TRAN
END CATCH
";
                    var dtl=cn.QueryFirstOrDefault<INETPaymentResponseDetail>(sql, new
                    {
                        @timestamp = this.timestamp,
                        @merchant_id = this.merchant_id,
                        @event = this.event_text,
                        @response_code = this.detail.response_code,
                        @response_message = this.detail.response_message,
                        @merchant_id2 = this.detail.merchant_id,
                        @order_id = this.detail.order_id,
                        @payment_reference_id = this.detail.payment_reference_id,
                        @receive_amount = this.detail.receive_amount,
                        @payment_type = this.detail.payment_type,
                        @payment_acquirer_bank = this.detail.payment_acquirer_bank,
                        @transaction_date = this.detail.transaction_date,
                        @transaction_time = this.detail.transaction_time,
                        @order_description = this.detail.order_description
                    });
                    if (dtl.merchant_id !="")
                    {
                        var hdr = cn.QueryFirstOrDefault<INETPaymentResponse>("SELECT * FROM [dbo].[INET_Payment_Response] WHERE [timestamp]=@timestamp", new
                        {
                            @timestamp=this.timestamp
                        });
                        if (hdr.timestamp == this.timestamp)
                        {
                            return true;
                        }
                        return false;
                    }
                    return false;
                }
            }catch(Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }
    }
    public class INETPaymentResponseDetail
    {
        public string response_code { get; set; }
        public string response_message { get; set; }
        public string merchant_id { get; set; }
        public string order_id { get; set; }        
        public string payment_reference_id { get; set; }
        public string receive_amount { get; set; }
        public string payment_type { get; set; }
        public string payment_acquirer_bank { get; set; }
        public string transaction_date { get; set; }
        public string transaction_time { get; set; }
        public string order_description { get; set; }
    }
}