namespace Simple.Data.Pad.Properties
{
    using System;
    using System.CodeDom.Compiler;
    using System.Configuration;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;

    [CompilerGenerated, GeneratedCode("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "10.0.0.0")]
    internal sealed class Settings : ApplicationSettingsBase
    {
        private static Settings defaultInstance = ((Settings) SettingsBase.Synchronized(new Settings()));

        public static Settings Default
        {
            get
            {
                return defaultInstance;
            }
        }

        [DefaultSettingValue(""), DebuggerNonUserCode, UserScopedSetting]
        public string LastQuery
        {
            get
            {
                return (string) this["LastQuery"];
            }
            set
            {
                this["LastQuery"] = value;
            }
        }

        [DefaultSettingValue("OpenConnection"), UserScopedSetting, DebuggerNonUserCode]
        public string OpenMethod
        {
            get
            {
                return (string) this["OpenMethod"];
            }
            set
            {
                this["OpenMethod"] = value;
            }
        }

        [DefaultSettingValue("ConnectionString"), UserScopedSetting, DebuggerNonUserCode]
        public string OpenMethodParameter1
        {
            get
            {
                return (string) this["OpenMethodParameter1"];
            }
            set
            {
                this["OpenMethodParameter1"] = value;
            }
        }

        [DefaultSettingValue(""), DebuggerNonUserCode, UserScopedSetting]
        public string OpenMethodParameter2
        {
            get
            {
                return (string) this["OpenMethodParameter2"];
            }
            set
            {
                this["OpenMethodParameter2"] = value;
            }
        }

        [UserScopedSetting, DebuggerNonUserCode, DefaultSettingValue("1")]
        public int OpenMethodParameterCount
        {
            get
            {
                return (int) this["OpenMethodParameterCount"];
            }
            set
            {
                this["OpenMethodParameterCount"] = value;
            }
        }

        [DebuggerNonUserCode, UserScopedSetting, DefaultSettingValue("True")]
        public bool UpgradeRequired
        {
            get
            {
                return (bool) this["UpgradeRequired"];
            }
            set
            {
                this["UpgradeRequired"] = value;
            }
        }
    }
}

