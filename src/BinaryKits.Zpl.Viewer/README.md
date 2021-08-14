# Open Tasks

- Barcodes scaling
- Calculate label size by the drawn elements
- Optimize font drawing for different font names
- Optimize GraphicBox drawing special border (^XA^FO50,50^GB100,100,120,B,1^FS^XZ)
- Add more elements (^GE Graphic Ellipse, ^GS Graphic Symbol, ...)
- NetBarcode not support Interleaved2of5

# Example code

```
IPrinterStorage printerStorage = new PrinterStorage();
var drawer = new ZplElementDrawer(printerStorage);

var analyzer = new ZplAnalyzer(printerStorage);
var analyzeInfo = analyzer.Analyze(request.ZplData);

foreach (var labelInfo in analyzeInfo.LabelInfos)
{
	var imageData = drawer.Draw(labelInfo.ZplElements);
	//imageData is bytes of png image
}
```