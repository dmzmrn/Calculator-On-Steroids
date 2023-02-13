namespace Calculator_On_Steroids
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
          

            bool createdNew;
            using (new Mutex(true, Application.ProductName, out createdNew))
            {
                if (createdNew)
                {
                    Application.EnableVisualStyles();
                    Application.SetCompatibleTextRenderingDefault(false);
                    ApplicationConfiguration.Initialize();
                    Application.Run(new frmCalculator());
                }
                else
                {
                    MessageBox.Show("An instance of the application is already running.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                }
            }
        }
    }
}