using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace DashBoard.Presentation.Active
{
    public class MyCommands
    {

        public static RoutedCommand Barge = new RoutedCommand("Barge", typeof(ctlrptActiveAgent));

        public static RoutedCommand Hangup = new RoutedCommand("Hangup", typeof(ctlrptActiveAgent));

    }
}
