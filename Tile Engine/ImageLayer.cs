/* File Description
 * Original Works/Author: Marshall Ward
 * Other Contributors: None
 * Author Website: https://github.com/marshallward/TiledSharp
 * License: Apache 2.0
*/

using System.Xml.Linq;

namespace SFGL.TileEngine
{
    public class TmxImageLayer : ITmxElement
    {
        public string Name {get; private set;}
        public int Width {get; private set;}
        public int Height {get; private set;}

        public bool Visible {get; private set;}
        public double Opacity {get; private set;}

        public TmxImage Image {get; private set;}

        public PropertyDict Properties {get; private set;}

        public TmxImageLayer(XElement xImageLayer, string tmxDir = "")
        {
            Name = (string)xImageLayer.Attribute("name");
            Width = (int)xImageLayer.Attribute("width");
            Height = (int)xImageLayer.Attribute("height");
            Visible = (bool?)xImageLayer.Attribute("visible") ?? true;
            Opacity = (double?)xImageLayer.Attribute("opacity") ?? 1.0;

            Image = new TmxImage(xImageLayer.Element("image"), tmxDir);

            Properties = new PropertyDict(xImageLayer.Element("properties"));
        }
    }
}
