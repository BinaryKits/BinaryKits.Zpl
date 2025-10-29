# BinaryKits.Zpl.Viewer

## Supported Barcode Formats

### 1D Barcodes
- [x] Code 39 (^B3)
- [x] Code 93 (^BA)
- [x] Code 128 (^BC)
- [x] EAN-13 (^BE)
- [x] UPC-A (^BU)
- [x] UPC-E (^B9)
- [x] UPC Extension (^BS)
- [x] Interleaved 2 of 5 (^B2)
- [x] ANSI Codabar (^BK)

### 2D Barcodes
- [x] QR Code (^BQ)
- [x] Data Matrix (^BX)
- [x] PDF417 (^B7)
- [x] Aztec (^B0)
- [x] MaxiCode (^BD)

## Unsupported Barcode Formats

### 1D Barcodes
- [ ] MSI (^BM): Supported by ZXing.NET
- [ ] Databar/RSS-14 (^BR): Not supported by ZXing.NET
- [ ] Code 11 (^B1): Not supported by ZXing.NET, [simple to implement](https://web.archive.org/web/20070202060711/http://www.barcodeisland.com/code11.phtml)
- [ ] POSTNET (^BB): Not supported by ZXing.NET
- [ ] PLANET (^B8): Not supported by ZXing.NET
- [ ] Composite (^BC): Not supported by ZXing.NET, simple to implement
  - **Note:** Shares command with Code 128 - requires special handling

### 2D Barcodes
- [ ] Micro PDF417 (^B7): Not supported by ZXing.NET
  - ASCII data only
  - Appears to be a subset of PDF417
  - **Note:** Shares command with PDF417 - differentiated by mode parameter
- [ ] Micro QR Code (^BQ): Not supported by ZXing.NET
  - Appears to be a subset of QR Code
  - **Note:** Shares command with QR Code - differentiated by model parameter

## Supported Label Elements

### Text Elements
- [x] Text Field (^FD with ^A or ^CF)
- [x] Field Block (^FB) - Multi-line text with justification
- [x] Field Typeset (^FT) - Typeset field positioning
- [x] Scalable/Bitmapped Font (^A, ^CF)
- [x] Change International Font (^CI)
- [x] Hexadecimal Indicator (^FH)
- [x] Field Reverse Print (^FR)
- [x] Field Number (^FN) - Variable field for templates
- [x] Recall Field Number - Variable field data

### Graphic Elements
- [x] Graphic Box (^GB) - Rectangle with optional fill
- [x] Graphic Circle (^GC)
- [x] Graphic Field (^GF) - Raster graphics
- [x] Image Move (^IM) - Recall stored image
- [x] Recall Graphic (^XG) - Recall stored graphic with scaling

### Positioning & Layout
- [x] Field Origin (^FO) - Set field position
- [x] Field Separator (^FS)
- [x] Field Orientation (^FW) - Rotate fields
- [x] Label Home (^LH) - Set label home position
- [x] Label Reverse Print (^LR) - Reverse entire label

### Storage & Templates
- [x] Download Graphics (~DG) - Store graphic to memory
- [x] Download Objects (~DY) - Store objects/fonts to memory
- [x] Download Format (^DF) - Store label template
- [x] Recall Format (^XF) - Recall label template

### Control Elements
- [x] Comment (^FX)
- [x] Barcode Field Default (^BY) - Set barcode module width

## Unsupported Label Elements

### Graphic Elements
- [ ] Graphic Ellipse (^GE): Implemented in Label project, missing drawer
- [ ] Graphic Symbol (^GS): Implemented in Label project, missing drawer
- [ ] Graphic Diagonal Line (^GD): Implemented in Label project, missing drawer

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

# Adding Barcode support
Also applies to other "drawables", like shapes and graphics.

## Examples
- [EAN Barcode](https://github.com/BinaryKits/BinaryKits.Zpl/commit/3fac409732e19be9e047ee71f942ba1f68c6fa5c)
- [Datamatrix Barcode](https://github.com/BinaryKits/BinaryKits.Zpl/commit/f79d01512eee7e3d16246e932877ca6d4aa4e306)
- [PDF417 Barcode](https://github.com/BinaryKits/BinaryKits.Zpl/pull/190/files)

## Steps
Generally, looking at files in the directory for each step should give you a clue on how to implement a file.

### 1: Create the abstract element
This is a class used to represent the barcode with all it's settings, mostly to be used for zpl generation.
We are currently in the middle of a (stalled) refactor which leads to some amount of code duplication.
A class should be created in these `<project>/<directory>`:
- `BinaryKits.Zpl.Label/Elements/`
- `BinaryKits.Zpl.Protocol/Commands/`

For 2d barcodes, implement `ZplPositionedElementBase` and `IFormatElement`, for 1d barcodes, subclass `ZplBarcode`

### 2: Create a model in `BinaryKits.Zpl.Viewer/Models/`
This one is only used for the viewer project and should contain mostly the same properties as the class created in the previous step.

### 3: Create the command analyzer in `BinaryKits.Zpl.Viewer/CommandAnalyzers/`
This is where you write the code for parsing zpl to the Model/Element.

- Subclass from `ZplCommandAnalyzerBase`
- Use `this.SplitCommand` get all the zpl arguments
- Remember to check that the length of args array is long enough before trying to parse at an index
- Remember to set the defaults each parameter before parsing
	- There are parameters stored in the virtual printer such as `^BY`
	- Otherwise use the defaults as described in the zpl programming manual
- Because there is always data to encode, set the next field data encountered by the virtual printer to return the content to the model

### 4: Add an entry to `BinaryKits.Zpl.Viewer/CommandAnalyzers/FieldDataZplCommandAnalyzer.cs`
This is to make sure that we grab the `text`, the model from the previous step it into a new instance of the abstract element.

Find the massive `if` chain following `if (this.VirtualPrinter.NextElementFieldData != null)` and add an entry for your mode.

Architecturally, this type of hard binding is probably not good. We should probably fix this.

### 5: Add an entry to `BinaryKits.Zpl.Viewer/ZPLAnalyzer.cs`
Find the List `elementAnalyzers` and instantiate your analyzer there.

### 6: Create a drawer in `BinaryKits.Zpl.Viewer/ElementDrawers`
- Subclass `BarcodeDrawerBase`
- Implement `CanDraw` by checking against the abstract element in step 1.
- Implement `Draw`. Working from the end:
	- The end goal is to write some pixels (a skia bitmap) to the base canvas with `this.DrawBarcode`
	- Those pixels will be obtained by converting an abstract barcode drawing to a png `SKBitmap`
	- The abstract drawing will be generated by calling an external library with the correct parameters obtained from the abstract element
		- Zxing.net uses hints to pass in extra parameters
	- Some adaptative helper functions may needed to convert between the library types and skia bitmaps or binarykit.zpl models
	- `BarcodeDrawerBase` may have some helper functions needed

### 7: Add an entry to `BinaryKits.Zpl.Viewer/ZPLElementDrawer.cs`
Find the array `this._elementDrawers` and instantiate your drawer there.

### 8: Create a test in to `BinaryKits.Zpl.Viewer.UnitTest/DrawerTest.cs`
- todo: load data from BinaryKits.Zpl.Viewer.UnitTest/Data/zpl like in the webapi project
- Use the `DefaultPrint` function when possible, you can pass dimensions, density and drawer options
- todo: an empty custom file `BinaryKits.Zpl.Viewer.UnitTest/Data/zpl/custom.zpl2` is not under vcs to enable quick testing
