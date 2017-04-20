# ZPLUtility
A .net library helping to generate ZPL string.
Please refer to the Programming Guide for raw ZPL code definitaion, https://www.zebra.com/content/dam/zebra/manuals/en-us/software/zpl-zbi2-pm-en.pdf

## Usage:
### Single element
```C#
var result = new ZPLGraphicBox(100, 100, 100, 100).ToZPLString();
Console.WriteLine(result); 
//Output
//^FO100,100
//^GB100,100,1,B,0^FS
```
### Whole label
```C#
var sampleText = "[_~^][LineBreak\n][The quick fox jumps over the lazy dog.]";
ZPLFont font = new ZPLFont(fontWidth: 50, fontHeight: 50);
var labelElements = new List<ZPLElementBase>();
labelElements.Add(new ZPLTextField(sampleText, 50, 100, font));
labelElements.Add(new ZPLGraphicBox(400, 700, 100, 100, 5));
labelElements.Add(new ZPLGraphicBox(450, 750, 100, 100, 50, ZPLConstants.LineColor.White));
labelElements.Add(new ZPLGraphicCircle(400, 700, 100, 5));
labelElements.Add(new ZPLGraphicDiagonalLine(400, 700, 100, 50, 5));
labelElements.Add(new ZPLGraphicDiagonalLine(400, 700, 50, 100, 5));
labelElements.Add(new ZPLGraphicSymbol(ZPLGraphicSymbol.GraphicSymbolCharacter.RegisteredTradeMark, 600, 600, 50, 50));

//Add raw ZPL code
labelElements.Add(new ZPLRaw("^FO200, 200^GB300, 200, 10 ^FS"));

var renderEngine = new ZPLEngine(labelElements);
var output = renderEngine.ToZPLString(new ZPLRenderOptions() { AddEmptyLineBeforeElementStart = true });

Console.WriteLine(output);
```
### Auto scale based on DPI
```C#
var labelElements = new List<ZPLElementBase>();
labelElements.Add(new ZPLGraphicBox(400, 700, 100, 100, 5));

var options = new ZPLRenderOptions() { SourcePrintDPI = 203, TargetPrintDPI = 300 };
var output = new ZPLEngine(labelElements).ToZPLString(options);

Console.WriteLine(output);
```
