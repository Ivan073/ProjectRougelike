using Project.GameClasses;
using System.Diagnostics;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Project
{
    internal static class Program
    {
        //public static Stopwatch allTime = new Stopwatch();
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();  //window intialization
            //allTime.Start();
            var gameWindow = new Window();  //bind/graphic logic in window

            Game.SetWindow(gameWindow); 
            Game.Start();   //main logic

           
            Application.Run(gameWindow);  
        }
    }
}