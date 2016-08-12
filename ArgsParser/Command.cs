namespace fmdev.ArgsParser
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Reflection;

    public abstract class Command
    {
        public virtual string GetName()
        {
            var name = this.GetType().Name;
            name = name.Substring(0, 1).ToLowerInvariant() + name.Substring(1); // pascal case
            if (name.Contains("Command"))
            {
                return name.Remove(name.LastIndexOf("Command", StringComparison.Ordinal));
            }
            else
            {
                return name;
            }
        }

        public string GetDescription()
        {
            IEnumerable<Attribute> attributes = TypeDescriptor.GetAttributes(this).Cast<Attribute>();
            return ((DescriptionAttribute)attributes.First(attribute => attribute is DescriptionAttribute)).Description;
        }

        public IEnumerable<PropertyInfo> GetOptions()
        {
            var props = GetType().GetProperties();
            var result = props.Where(prop => prop.IsDefined(typeof(CommandArgAttribute), false));
            return result;
        }
    }
}