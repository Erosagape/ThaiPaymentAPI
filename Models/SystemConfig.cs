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
    public class SystemConfig
    {
        public string ConfigCode { get; set; }
        public string ConfigKey { get; set; }
        public string ConfigValue { get; set; }
        public IEnumerable<SystemConfig> Gets()
        {
            using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["MainConnection"].ConnectionString))
            {
                return cn.Query<SystemConfig>("SELECT * FROM [dbo].[SystemConfig]");
            }
        }
        public string GetValue(string defaultValue="")
        {
            if (this.Get())
                return this.ConfigValue;
            return defaultValue;
        }
        public bool Get()
        {
            using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["MainConnection"].ConnectionString))
            {
                var o= cn.QueryFirstOrDefault<SystemConfig>("SELECT * FROM [dbo].[SystemConfig] WHERE [ConfigCode]=@cfgcode AND [ConfigKey]=@cfgkey",new { 
                    @cfgcode=this.ConfigCode,
                    @cfgkey=this.ConfigKey
                });
                bool found = false;
                if (o != null)
                {
                    found = true;
                    this.ConfigValue = o.ConfigValue;
                }
                return found;
            }
        }
        public int Delete()
        {
            using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["MainConnection"].ConnectionString))
            {
                return cn.Execute("DELETE FROM [dbo].[SystemConfig] WHERE [ConfigCode]=@cfgcode AND [ConfigKey]=@cfgkey", new
                {
                    @cfgcode=this.ConfigCode,
                    @cfgkey=this.ConfigKey
                });
            }
        }
        public ErrorResponse Save()
        {
            var err = new ErrorResponse();
            try
            {
                using(SqlConnection cn=new SqlConnection(ConfigurationManager.ConnectionStrings["MainConnection"].ConnectionString))
                {
                    string sql = @"
BEGIN TRY
    BEGIN TRANSACTION
        IF EXISTS(SELECT * FROM [dbo].[SystemConfig] WHERE [ConfigCode]=@cfgcode AND [ConfigKey]=@cfgkey)
        BEGIN
            UPDATE [dbo].[SystemConfig]
            SET [ConfigValue]=@cfgvalue
            WHERE [ConfigCode]=@cfgcode AND [ConfigKey]=@cfgkey
        END
        ELSE            
        BEGIN
            INSERT INTO [dbo].[SystemConfig] ([ConfigCode],[ConfigKey],[ConfigValue]) 
            VALUES
            (@cfgcode,@cfgkey,@cfgvalue)
        END
    COMMIT TRANSACTION
    SELECT * FROM [dbo].[SystemConfig] WHERE [ConfigCode]=@cfgcode AND [ConfigKey]=@cfgkey
END TRY
BEGIN CATCH
    ROLLBACK TRANSACTION
END CATCH
";
                    var o=cn.QueryFirstOrDefault<SystemConfig>(sql, new
                    {
                        @cfgcode=this.ConfigCode,
                        @cfgkey=this.ConfigKey,
                        @cfgvalue=this.ConfigValue
                    });
                    if (o.ConfigValue.Equals(this.ConfigValue))
                    {
                        err.success = true;
                        err.data = JsonConvert.SerializeObject(this);
                        err.error = "OK";
                    } else
                    {
                        err.data = JsonConvert.SerializeObject(this);
                        err.error = "Cannot Save Data";
                    }
                }
            } catch(Exception e)
            {
                err.error = e.Message;
                err.data = JsonConvert.SerializeObject(this);
            }
            return err;
        }
    }
}