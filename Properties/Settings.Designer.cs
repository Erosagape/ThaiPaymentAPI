﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ThaiPaymentAPI.Properties {
    
    
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "16.10.0.0")]
    internal sealed partial class Settings : global::System.Configuration.ApplicationSettingsBase {
        
        private static Settings defaultInstance = ((Settings)(global::System.Configuration.ApplicationSettingsBase.Synchronized(new Settings())));
        
        public static Settings Default {
            get {
                return defaultInstance;
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("wQhgUQAEzQyJt1feCWmScuDYoa6xHI2iGRcOnYqB")]
        public string INETMerchantKeyUAT {
            get {
                return ((string)(this["INETMerchantKeyUAT"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("P7CxC6SRvoc3krJ54ATUUzC3W0Gwvw8ICNCuucAm")]
        public string INETMerchantKeyPRD {
            get {
                return ((string)(this["INETMerchantKeyPRD"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("https://test.thaidotcompayment.co.th/INETServiceWeb/api/v1/accesstoken")]
        public string INETUrlOrderPlaceUAT {
            get {
                return ((string)(this["INETUrlOrderPlaceUAT"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("https://www.aih-consultant.com/backend/order/confirmation")]
        public string INETCallbackUrl {
            get {
                return ((string)(this["INETCallbackUrl"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("https://portal.thaidotcompayment.co.th/INETServiceWeb/api/v1/accesstoken")]
        public string INETUrlOrderPlacePRD {
            get {
                return ((string)(this["INETUrlOrderPlacePRD"]));
            }
        }
    }
}
