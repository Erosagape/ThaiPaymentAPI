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
    public class ActionLog
    {
        public int log_id { get; set; }
        public string log_source { get; set; }
        public string log_action { get; set; }
        public string log_data { get; set; }
        public string log_message { get; set; }
        public string log_stacktrace { get; set; }
        public bool log_error { get; set; }
        public IEnumerable<ActionLog> Gets()
        {
            using(SqlConnection cn=new SqlConnection(ConfigurationManager.ConnectionStrings["MainConnection"].ConnectionString))
            {
                return cn.Query<ActionLog>("SELECT * FROM dbo.ActionLog");
            }
        }
        public ErrorResponse Save()
        {
            ErrorResponse ex = new ErrorResponse();
            try
            {
                using(SqlConnection cn=new SqlConnection(ConfigurationManager.ConnectionStrings["MainConnection"].ConnectionString))
                {
                    var sql = @"
BEGIN TRY
    BEGIN TRANSACTION
    
    DECLARE @AUTO_ID as int
    
    INSERT INTO dbo.ActionLog (log_source,log_action,log_data,log_message,log_stacktrace,log_error) 
    VALUES(@source,@action,@data,@message,@stacktrace,@err)
    
    SET @AUTO_ID=SCOPE_IDENTITY()

    SELECT * FROM dbo.ActionLog WHERE log_id=@AUTO_ID

    COMMIT TRANSACTION
END TRY
BEGIN CATCH
    ROLLBACK TRANSACTION
END CATCH
";
                    var o=cn.QueryFirstOrDefault<ErrorResponse>(sql,new { 
                        @source=this.log_source,
                        @action=this.log_action,
                        @data=this.log_data,
                        @message=this.log_message,
                        @stacktrace=this.log_stacktrace,
                        @err=this.log_error
                    });
                    ex.success = true;
                    ex.error = "OK";
                    ex.data = JsonConvert.SerializeObject(o);
                }
            } catch (Exception e)
            {
                ex.error = e.Message;
                ex.data = e.StackTrace;
            }
            return ex;
        }
    }
}