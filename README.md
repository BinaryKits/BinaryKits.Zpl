# ZPLUtility
A .net library helping to generate ZPL string

## Usage:
### Single element
```C#
 var context = new ZPLContext();
 var output = new ZPLGraphicBox(100, 100, 10, 10).Render(context);
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

var renderEngine = new ZPLEngine(labelElements);
var output = renderEngine.Render(new ZPLContext() { DisplayComments = true, AddEmptyLineBeforeElementStart = true });
```
