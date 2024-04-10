using System;

namespace BinaryKits.Zpl.Labelary
{
    public class LabelSize
    {
        private readonly double _width;
        private readonly double _height;
        private readonly Measure _measure;
        private readonly double _millimeterToInch = 25.4;

        public LabelSize(double width, double height, Measure measure)
        {
            _width = width;
            _height = height;
            _measure = measure;
        }

        public double WidthInInch
        {
            get
            {
                if (_measure == Measure.Inch)
                {
                    return _width;
                }

                var rounded = Math.Round(_width / _millimeterToInch, 8);
                return rounded;
            }
        }

        public double HeightInInch
        {
            get
            {
                if (_measure == Measure.Inch)
                {
                    return _height;
                }
                
                var rounded = Math.Round(_height / _millimeterToInch, 8);
                return rounded;
            }
        }
    }
}
