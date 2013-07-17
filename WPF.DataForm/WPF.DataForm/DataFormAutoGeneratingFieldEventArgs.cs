using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace WPF.DataForm
{
    public class DataFormAutoGeneratingFieldEventArgs : CancelEventArgs
    {
        public DataField Field { get; set; }

        public string PropertyName { get; private set; }

        public Type PropertyType { get; private set; }

        public DataFormAutoGeneratingFieldEventArgs(string propertyName, Type propertyType, DataField field)
        {
            this.Field = field;
            this.PropertyName = propertyName;
            this.PropertyType = propertyType;
        }
    }
}
