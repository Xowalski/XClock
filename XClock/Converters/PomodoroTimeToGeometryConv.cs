using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Shapes;

namespace XClock
{
    class PomodoroTimeToGeometryConv : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            TimeSpan timeSpan = (TimeSpan)value;
            double minutes;
            if (timeSpan.Hours > 0)
            {
                minutes = 59.99;
            }
            else
            {
                minutes = timeSpan.Minutes + ((double)timeSpan.Seconds/60);
            }

            double angle = minutes * 360 / 60;
            double x = 140 * Math.Sin(angle * (Math.PI / -180));
            double y = -140 * Math.Cos(angle * (Math.PI / -180));

            PathFigure pathFigure = new PathFigure();
            pathFigure.StartPoint = new Point(0, 0);
            pathFigure.IsClosed = true;

            LineSegment lineSegment = new LineSegment();
            lineSegment.Point = new Point(0, -140);

            ArcSegment arcSegment = new ArcSegment();
            arcSegment.Point = new Point(x, y);
            arcSegment.RotationAngle = 0;
            if (angle > 180)
            {
                arcSegment.IsLargeArc = true;
            }
            else
            {
                arcSegment.IsLargeArc = false;
            }
            arcSegment.SweepDirection = SweepDirection.Counterclockwise;
            arcSegment.Size = new Size(140, 140);


            PathSegmentCollection pathSegments = new PathSegmentCollection();
            pathSegments.Add(lineSegment);
            pathSegments.Add(arcSegment);

            pathFigure.Segments = pathSegments;

            PathFigureCollection pathFigures = new PathFigureCollection();
            pathFigures.Add(pathFigure);

            PathGeometry pathGeometry = new PathGeometry();
            pathGeometry.Figures = pathFigures;

            Path path = new Path();
            path.Data = pathGeometry;

            return path.Data;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
