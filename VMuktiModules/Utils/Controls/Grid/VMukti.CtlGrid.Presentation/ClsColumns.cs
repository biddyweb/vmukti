
namespace VMukti.CtlGrid.Presentation
{
    public class ClsColumns
    {
        private string _Header = "";
        private string _BindedObject = "";
        private bool _IsIcon = false;

        public string Header
        {
            get { return _Header; }
            set { _Header = value; }
        }

        public string BindedObject
        {
            get { return _BindedObject; }
        }

        public bool IsIcon
        {
            get { return _IsIcon; }
            set { _IsIcon = value; }
        }

        public void BindTo(string ObjectName)
        {
            if (_Header == "")
                _Header = ObjectName;
            _BindedObject = ObjectName;
        }
    }
}
