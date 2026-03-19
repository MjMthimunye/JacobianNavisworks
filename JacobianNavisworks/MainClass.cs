using Autodesk.Navisworks.Api.Plugins;
using System.Windows;

namespace JacobianNavisworks
{
    // Plugin attribute defines the plugin name and developer ID
    [Plugin("JacobianNavisworks.MainClass",
            "JCDV",  // 4-character Developer ID 
            DisplayName = "Jacobian Clash Tools",
            ToolTip = "Clash detection and analysis tools.")]

    // RibbonLayout specifies the XAML file that defines the ribbon layout
    [RibbonLayout("JacobianNavisworks.xaml")]

    // RibbonTab defines which tab this plugin creates (must match the Id in XAML)
    [RibbonTab("ID_JacobianDevTab")]

    // Command attributes for each button (must match the Ids in XAML)
    [Command("ID_ClashAnalysis",
                Icon = "Images\\clash_icon_16.ico",
                LargeIcon = "Images\\clash_icon_32.ico",
                ToolTip = "Analyze clash tests and display model information")]

    public class MainClass : CommandHandlerPlugin
    {
        public override int ExecuteCommand(string commandId, params string[] parameters)
        {
            switch (commandId)
            {
                case "ID_ClashAnalysis":
                    var mainUI = new MainUI();
                    mainUI.Show();
                    break;
                default:
                    MessageBox.Show("Unknown command: " + commandId);
                    break;
            }

            return 0;
        }
    }
}
