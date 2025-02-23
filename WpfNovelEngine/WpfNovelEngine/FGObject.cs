using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfNovelEngine
{
    class FGObject
    {
        public int ID { get; set; }
        public int? FGWidth;        
        public int? FGHeight;
        public int? FGPositionX;
        public int? FGPositionY;
        private string _FGImagePath;        
        private string _Name;
        public string Name
        {
            get { return _Name; }
        }
        public string FGImagePath
        {
            get { return _FGImagePath; }
            set { _FGImagePath = value; _Name = System.IO.Path.GetFileNameWithoutExtension(FGImagePath);}
        }
        public override string ToString()
        {
            return System.IO.Path.GetFileNameWithoutExtension(FGImagePath);
        }
    }
}
