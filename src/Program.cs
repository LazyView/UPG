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
            if (args.Length > 0)
            {
                // Use the provided parameter
                parameter = args[0];
                // Integrate the parameter into the application logic
            }
            else
            {
                parameter = "0";
            }
            // Create the MainForm and pass the parameter to it
            using (var mainForm = new MainForm(parameter))
            {
                mainForm.Show();
                Application.Run(mainForm);
            }
        }
    }
}