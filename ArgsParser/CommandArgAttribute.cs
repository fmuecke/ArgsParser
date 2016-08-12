namespace fmdev.ArgsParser
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    [AttributeUsage(AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
    public sealed class CommandArgAttribute : Attribute
    {
        public CommandArgAttribute()
        {
            IsRequired = false;
        }

        public string HelpText { get; set; }

        public bool IsRequired { get; set; }
    }
}