using System.Drawing.Drawing2D;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace Fractal
{
    public partial class Form1 : Form
    {
        Triangle T;
        Graphics g;

        public Form1()
        {
            InitializeComponent();
        }


        public Point TranslateToForm(Point p)
        {
            return new Point(
                p.X < 0 ? 400 - (p.X * -1) : p.X + 400
                , p.Y < 0 ? (p.Y * -1) + 400 : 400 - p.Y);
        }

        private void Form1_MouseClick(object sender, MouseEventArgs e)
        {
            Pen p = new Pen(Color.White, 1);

            g = this.CreateGraphics();

            SetupTriangle(p);
            DrawSierpinski(p);
            DrawRandom(p);
        }

        private void SetupTriangle(Pen p)
        {
            T = new Triangle(
             new Point(100, 300),
             new Point(-300, -300),
             new Point(300, -300)
             );
            g.DrawRectangle(p, new Rectangle(TranslateToForm(T.A), new Size(1, 2)));
            g.DrawRectangle(p, new Rectangle(TranslateToForm(T.B), new Size(1, 2)));
            g.DrawRectangle(p, new Rectangle(TranslateToForm(T.C), new Size(1, 2)));
        }

        private void DrawSierpinski(Pen p)
        {
            p.Color = Color.Red;

            var r = new Random(DateTime.Now.Millisecond);
            Point startingPoint = new Point(200, 200);
            do
            {
                startingPoint = new Point(r.Next(-400, 400), r.Next(-400, 400));
            } while (!T.Inside(startingPoint));

            g.DrawRectangle(p, new Rectangle(TranslateToForm(startingPoint), new Size(1, 1)));

            var rKnownPoint = 0;
            for (int i = 0; i < 20000; i++)
            {
                switch (rKnownPoint)
                {
                    case 0:
                        startingPoint = T.MidPoint(T.A, startingPoint);
                        g.DrawRectangle(p, new Rectangle(TranslateToForm(startingPoint), new Size(1, 1)));
                        break;
                    case 1:
                        startingPoint = T.MidPoint(T.B, startingPoint);
                        g.DrawRectangle(p, new Rectangle(TranslateToForm(startingPoint), new Size(1, 1)));
                        break;
                    case 2:
                        startingPoint = T.MidPoint(T.C, startingPoint);
                        g.DrawRectangle(p, new Rectangle(TranslateToForm(startingPoint), new Size(1, 1)));
                        break;
                    default:
                        rKnownPoint = -1;
                        break;
                }
                //if (rKnownPoint <= 2) rKnownPoint = rKnownPoint + 1;
                rKnownPoint = r.Next(-1, 3);
            }
        }

        private void DrawRandom(Pen p)
        {
            p.Color = Color.Green;

            var r = new Random(DateTime.Now.Millisecond);

            for (int i = 0; i < 20000; i++)
            {
                var rp = new Point(r.Next(-400, 400), r.Next(-400, 400));

                if (T.Inside(rp))
                {
                    g.DrawRectangle(p, new Rectangle(TranslateToForm(rp), new Size(1, 1)));
                }

            }
        }
    }

    public class Triangle
    {
        public Point A { get; set; }
        public Point B { get; set; }
        public Point C { get; set; }

        public double AngleABBC { get; }
        public double AngleACBC { get; }

        public Triangle(Point a, Point b, Point c)
        {
            A = a;
            B = b;
            C = c;

            AngleABBC = Angle(A, C, B);
            AngleACBC = Angle(B, A, C);
        }


        /// <summary>
        /// Created from ChatGPT Prompt: how do i calculate the angle in degrees between two points that share a 3rd point in 2D using c#
        /// </summary>
        private double Angle(Point P1, Point P2, Point Shared)
        {
            double angleA = Math.Atan2(P1.Y - Shared.Y, P1.X - Shared.X);
            double angleB = Math.Atan2(P2.Y - Shared.Y, P2.X - Shared.X);

            double angle = (angleB - angleA) * (180 / Math.PI);

            while (angle < 0)
            {
                angle += 360;
            }

            while (angle > 360)
            {
                angle -= 360;
            }

            //modify to get opposite angle for triangle usage
            return angle > 180 ? 360 - angle : angle;
        }

        public bool Inside(Point P)
        {
            var a = Angle(P, C, B);
            var b = Angle(B, P, C);

            return (a <= AngleABBC) && (b <= AngleACBC) && P.Y <= A.Y && P.Y >= B.Y;
        }

        public Point MidPoint(Point a, Point b)
        {
            var p = new Point((int)Math.Round((a.X + b.X) / 2f, 0), (int)Math.Round((a.Y + b.Y) / 2f, 0));
            return p;
        }
    }
}