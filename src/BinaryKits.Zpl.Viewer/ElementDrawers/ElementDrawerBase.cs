using BinaryKits.Zpl.Label.Elements;
using SkiaSharp;

namespace BinaryKits.Zpl.Viewer.ElementDrawers
{
    public abstract class ElementDrawerBase : IElementDrawer
    {
        internal IPrinterStorage _printerStorage;
        internal SKPaint _skPaint;
        internal SKCanvas _skCanvas;
        internal int _padding;

        public void Prepare(
            IPrinterStorage printerStorage,
            SKPaint skPaint,
            SKCanvas skCanvas,
            int padding)
        {
            this._printerStorage = printerStorage;
            this._skPaint = skPaint;
            this._skCanvas = skCanvas;
            this._padding = padding;
            this._skPaint.ColorFilter = null;
        }

        ///<inheritdoc/>
        public abstract bool CanDraw(ZplElementBase element);

        ///<inheritdoc/>
        public abstract void Draw(ZplElementBase element);

        protected void ReversePrint()
        {
            //https://code.tutsplus.com/tutorials/manipulate-visual-effects-with-the-colormatrixfilter-and-convolutionfilter--active-3221
            //https://stackoverflow.com/questions/5941926/how-to-draw-with-an-inverted-paint-in-android-canvas

            //float[] mx1 = {
            //    -1.0f, -1.0f, -1.0f, -9.999f, 10.723f,
            //    -1.0f, -1.0f, -1.0f, -9.999f, 10.723f,
            //    -1.0f, -1.0f, -1.0f, -9.999f, 10.723f,
            //    -1.0f, -1.0f, -1.0f, 100.99f, -100.58f
            //};


            //float[] mx1 = {
            //    -1.0f,  0.0f,  0.0f,  1.0f,  0.0f,
            //    0.0f,  -1.0f,  0.0f,  1.0f,  0.0f,
            //    0.0f,  0.0f,  -1.0f,  1.0f,  0.0f,
            //    1.0f,  1.0f,  1.0f,  1.0f,  0.0f
            //};

            float[] mx1 = {
                -1.0f,  0.0f,  0.0f,  0.0f,  255.0f,
                0.0f,  -1.0f,  0.0f,  0.0f,  255.0f,
                0.0f,  0.0f,  -1.0f,  0.0f,  255.0f,
                0.0f,  0.0f,  0.0f,  1.0f,  0.0f
            };

            this._skPaint.ColorFilter = SKColorFilter.CreateColorMatrix(mx1);
        }
    }
}
