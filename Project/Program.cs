using Project.GameClasses;
using System.Diagnostics;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Project
{
    public static class Program
    {
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();  //window intialization
            var gameWindow = new Window();  //bind/graphic logic in window

            Game.SetWindow(gameWindow); 
            Game.Start();   //main logic

           
            Application.Run(gameWindow);  
        }
    }
}