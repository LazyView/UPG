using System.Windows.Forms;

namespace UPG_SP_2024
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            string parameter;
            int gridGapWidth;
            int gridGapHeight;
            if (args.Length > 0)
            {
                // Use the provided parameters
                parameter = args[0];

                gridGapWidth = Convert.ToInt32(args[1] + args[2]);
                gridGapHeight = Convert.ToInt32(args[4] + args[5]);
            }
            else
            {
                parameter = "0";
                gridGapWidth = 30;
                gridGapHeight = 30;
            }
            // Create the MainForm and pass the parameter to it
            using (var mainForm = new MainForm(parameter, gridGapWidth, gridGapHeight))
            {
                mainForm.Show();
                Application.Run(mainForm);
            }
        }
    }
}