using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace San_Administration.Model
{
    public class PluginData
    {
        public string Name { get; set; }
        public int ID { get; set; }

        public PluginData(string name, int id)
        {
            Name = name;
            ID = id;
        }
    }
}
