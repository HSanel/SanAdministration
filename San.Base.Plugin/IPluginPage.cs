using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace San.Base.Plugin
{
    public interface IPluginPage
    {
        Hashtable SaveData();
        void OpenData(object inData);
    }
}
