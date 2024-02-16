using System.Collections.Generic;
using VirtoCommerce.Platform.Core.Settings;

namespace VirtoCommerce.Module.Core;

public static class ModuleConstants
{
    public static class Security
    {
        public static class Permissions
        {
            public const string Access = "TestModule:access";
            public const string Create = "TestModule:create";
            public const string Read = "TestModule:read";
            public const string Update = "TestModule:update";
            public const string Delete = "TestModule:delete";

            public static string[] AllPermissions { get; } =
            {
                Access,
                Create,
                Read,
                Update,
                Delete,
            };
        }
    }

    public static class Settings
    {
        public static class General
        {
            public static SettingDescriptor TestModuleEnabled { get; } = new()
            {
                Name = "TestModule.TestModuleEnabled",
                GroupName = "TestModule|General",
                ValueType = SettingValueType.Boolean,
                DefaultValue = false,
            };

            public static SettingDescriptor TestModulePassword { get; } = new()
            {
                Name = "TestModule.TestModulePassword",
                GroupName = "TestModule|Advanced",
                ValueType = SettingValueType.SecureString,
                DefaultValue = "qwerty",
            };

            public static IEnumerable<SettingDescriptor> AllGeneralSettings
            {
                get
                {
                    yield return TestModuleEnabled;
                    yield return TestModulePassword;
                }
            }
        }

        public static IEnumerable<SettingDescriptor> AllSettings
        {
            get
            {
                return General.AllGeneralSettings;
            }
        }
    }
}
