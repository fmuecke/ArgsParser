namespace FMDev.ArgsParser
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Reflection;

    public abstract class Command
    {
        public virtual string Name
        {
            get
            {
                var name = this.GetType().Name;
                if (name.Contains("Command"))
                {
                    return name.Remove(name.LastIndexOf("Command", StringComparison.Ordinal));
                }
                else
                {
                    return name;
                }
            }
        }

        public string Description
        {
            get
            {
                IEnumerable<Attribute> attributes = TypeDescriptor.GetAttributes(this).Cast<Attribute>();
                return ((DescriptionAttribute)attributes.First(attribute => attribute is DescriptionAttribute)).Description;
            }
        }

        public IEnumerable<PropertyInfo> GetOptions()
        {
            var props = GetType().GetProperties();
            var result = props.Where(prop => prop.IsDefined(typeof(CommandArgAttribute), false));
            return result;
        }
    }
}