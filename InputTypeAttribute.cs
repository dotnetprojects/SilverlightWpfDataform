using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WPF.DataForm
{
    public class InputTypeAttribute: Attribute
    {
        public InputTypeAttribute() { }
        public InputTypeAttribute(FormTypes FormType, int PreferredWidth)
        {
            this.PreferredWidth = PreferredWidth;
            this.FormType = FormType;
        }
        public InputTypeAttribute(FormTypes FormType, int PreferredWidth, int PreferredHeight)
        {
            this.PreferredWidth = PreferredWidth;
            this.FormType = FormType;
            this.PreferredHeight = PreferredHeight;
        }
        public InputTypeAttribute(FormTypes FormType)
        {
            this.FormType = FormType;
        }
        public InputTypeAttribute(int PreferredWidth)
        {
            this.PreferredWidth = PreferredWidth;
        }

        public FormTypes FormType { get; set; }

        public int? PreferredWidth { get; set; }

        public int? PreferredHeight { get; set; }

        public enum FormTypes
        {
            @default, //lets the class decide
            check, // for boolean
            dates, //datepicker
            box, // no increments
            textArea, //larger, multi-line box               
            calculator
        }
    }
}
