using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApplication1
{
    class DummyDatum
    {
        public string Text1 { get; set; }
        public string Text2 { get; set; }

        public DummyDatum()
        {
            Text1 = "Hello";
            Text2 = "World";
        }
    }

    class DummyData : ObservableCollection<DummyDatum>
    {

    }
}
