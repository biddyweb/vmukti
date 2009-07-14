using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;

namespace ScriptDesigner.Business
{
    public enum TypeOfOptions
    {
        RadioButton = 0,
        CheckBox = 1,
        ComboBox = 2,
        ListBox = 3,
        TextBox = 4
    }

    public class clsQuestionDynamic
    {
        private int _NoOfOptions = 0;
        private string _Header = "";
        private List<string> _Options = null;
        private TypeOfOptions _Type = TypeOfOptions.RadioButton;

        public int NoOfOptions
        {
            get { return _NoOfOptions; }
            set { _NoOfOptions = value; }
        }

        public string Header
        {
            get { return _Header; }
            set { _Header = value; }
        }

        public List<string> Options
        {
            get { return _Options; }
            set { _Options = value; }
        }

        public TypeOfOptions Type
        {
            get { return _Type; }
            set { _Type = value; }
        }

        public clsQuestionDynamic(string header, int noOfOptions, TypeOfOptions type, List<string> options)
        {
            Header = header;
            NoOfOptions = noOfOptions;
            Type = type;
            Options = options;
        }
    }
}
