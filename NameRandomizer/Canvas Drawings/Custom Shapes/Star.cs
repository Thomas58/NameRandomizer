using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace NameRandomizer.Canvas_Drawings.Custom_Shapes
{
    class Star : Shape
    {
        public Star() { }

        protected override Geometry DefiningGeometry
        {
            get
            {
                //Star Points
                Point p_t =     new Point(this.Width / 2, Height);
                Point p_itl =   new Point(Width / 3, Height / 1.66);
                Point p_l =     new Point(0.0, Height / 1.66);
                Point p_il =    new Point(Width / 3, Height / 2.5);
                Point p_bl =    new Point(Width / 6, 0.0);
                Point p_ib =    new Point(this.Width / 2, Height / 5);
                Point p_br =    new Point(Width / 1.2, 0.0);
                Point p_ir =    new Point(Width / 1.5, Height / 2.5);
                Point p_r =     new Point(Width, Height / 1.66);
                Point p_itr =   new Point(Width / 1.5, Height / 1.66);

                List<PathSegment> segments = new List<PathSegment>(10);
                segments.Add(new LineSegment(p_t, true));
                segments.Add(new LineSegment(p_itl, true));
                segments.Add(new LineSegment(p_l, true));
                segments.Add(new LineSegment(p_il, true));
                segments.Add(new LineSegment(p_bl, true));
                segments.Add(new LineSegment(p_ib, true));
                segments.Add(new LineSegment(p_br, true));
                segments.Add(new LineSegment(p_ir, true));
                segments.Add(new LineSegment(p_r, true));
                segments.Add(new LineSegment(p_itr, true));

                List<PathFigure> figures = new List<PathFigure>(1);
                PathFigure pf = new PathFigure(p_t, segments, true);
                figures.Add(pf);

                Geometry g = new PathGeometry(figures, FillRule.Nonzero, null);

                return g;
            }
        }
    }
}
