using System;
using System.Collections.Generic;
using System.Text;

namespace Ex05.OthelloWindowsUI
{
    public class Program
    {
        public static void Main()
        {
            OthelloSettingsForm othelloForm = new OthelloSettingsForm();
            othelloForm.ShowDialog();
        }
    }
}
