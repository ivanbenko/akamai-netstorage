namespace Akamai.NetStorage
{
    using System;
    using System.Collections.Generic;

    class ActionBuilder
    {
        public static readonly string DefaultAction = "version=1";

        readonly List<string> actions = new List<string>();

        ActionBuilder()
        {
            this.actions.Add(DefaultAction);
        }

        public void Add(string key, string value)
        {
            this.actions.Add($"{key}={value}");
        }

        public static string Build(string action, Action<ActionBuilder> buildAction)
        {
            var builder = new ActionBuilder();
            builder.Add("action", action);

            buildAction(builder);

            return string.Join("&", builder.actions);
        }

        public string FormatValue(int? value) => value == null ? "" : value.Value.ToString();

        public string FormatValue(object value) =>
            value == null ? "" : value.ToString();

        public string FormatValue(DateTime value) => value.GetEpochSeconds().ToString();

        public string FormatValue(bool value) => value ? "0" : "1";

        public string FormatValue(byte[] value) => value.ToHex().ToLower();
    }
}