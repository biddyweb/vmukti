using System.Windows;
using System.Windows.Input;

namespace SpinControl.Presentation
{
    /// <summary>
    /// Interaction logic for CtlSpin.xaml
    /// </summary>
    public partial class CtlSpin : System.Windows.Controls.UserControl
    {
        private int _UpperRange=0;
        private int _LowerRange=0;
        private int _Value = 0;
        private bool _IsRotate = false;

        public int UpperRange
        {
            get { return _UpperRange; }
            set { _UpperRange = value; }
        }

        public int LowerRange
        {
            get { return _LowerRange; }
            set { _LowerRange = value; }
        }

        public int Value
        {
            get { return _Value; }
            set
            {
                _Value = value;
                txtValue.Text = _Value.ToString();
            }
        }

        public bool IsRotate
        {
            get { return _IsRotate; }
            set
            {
                _IsRotate = value;
            }
        }

        public CtlSpin()
        {
            InitializeComponent();
            Validate();
        }

        private void Validate()
        {
            if (_UpperRange == 0)
            {
                _UpperRange = 59;
            }

            if (_LowerRange == 0)
            {
                _LowerRange = 0;
            }

            if (_UpperRange < _LowerRange)
            {
                MessageBox.Show("Lower Range Should Be Less Than Upper Range");
            }

            if (_Value < _LowerRange || _Value > UpperRange)
                Value = _LowerRange;

            btnUp.Click += new RoutedEventHandler(btnUp_Click);
            btnDown.Click += new RoutedEventHandler(btnDown_Click);
            txtValue.KeyDown += new KeyEventHandler(txtValue_KeyDown);
            txtValue.LostFocus += new RoutedEventHandler(txtValue_LostFocus);
        }

        void txtValue_LostFocus(object sender, RoutedEventArgs e)
        {
            if (int.Parse(txtValue.Text) > UpperRange)
            {
                Value = UpperRange;
            }
            else if (int.Parse(txtValue.Text) < LowerRange)
            {
                Value = LowerRange;
            }
        }

        void txtValue_KeyDown(object sender, KeyEventArgs e)
        {
            #region Allowing only numbers and Tab

            if (!((e.Key >= Key.D0 && e.Key <= Key.D9) ||
                (e.Key == Key.Decimal) || e.Key == Key.OemMinus ||
                e.Key == Key.Tab ||
                 (e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9)))
            {
                e.Handled = true;
            }

            #endregion
        }

        public CtlSpin(int Lower, int Upper)
        {
            InitializeComponent();

            _LowerRange = Lower;
            _UpperRange = Upper;
            Validate();
        }

        public CtlSpin(int Lower, int Upper, int DefaultValue)
        {
            InitializeComponent();

            _LowerRange = Lower;
            _UpperRange = Upper;
            Value = DefaultValue;
            Validate();
        }

        void btnDown_Click(object sender, RoutedEventArgs e)
        {
            if (Value == _LowerRange)
            {
                if (_IsRotate == true)
                    Value = _UpperRange;
                else
                    Value = _LowerRange;
            }
            else
            {
                Value = Value - 1;
            }
        }

        void btnUp_Click(object sender, RoutedEventArgs e)
        {
            if (Value == _UpperRange)
            {
                if(_IsRotate == true)
                Value = _LowerRange;
                else
                    Value = _UpperRange;
            }
            else
            {
                Value = Value + 1;
            }
        }
    }
}
