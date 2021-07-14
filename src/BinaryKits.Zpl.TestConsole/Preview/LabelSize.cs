using System;

namespace BinaryKits.Zpl.TestConsole.Preview
{
    public class LabelSize
    {
        private double _width { get; set; }
        private double _height { get; set; }
        private Measure _measure { get; set; }
        private double _millimeterToInch = 25.4;

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

                return Math.Round(_width / _millimeterToInch, 0);
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

                return Math.Round(_height / _millimeterToInch, 0);
            }
        }
    }
}
