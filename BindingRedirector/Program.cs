namespace BindingRedirector;

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
#if NET8_0
        ApplicationConfiguration.Initialize();
#elif NET472
        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);
#endif
        Application.Run(new Form1());
    }
}