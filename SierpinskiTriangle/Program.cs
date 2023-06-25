using System.Windows.Forms;
using System.Drawing;
namespace Sierpinski
{
    public static class Program 
    {
        public static void Main()
        {
            Console.WriteLine("hello");
            Console.ReadLine();

            var f = new Form();
            f.BackColor = Color.White;
            f.FormBorderStyle = FormBorderStyle.None;
            f.Bounds = Screen.PrimaryScreen.Bounds;
            f.TopMost = true;

            Application.EnableVisualStyles();
            Application.Run(f);
        }
    }
}