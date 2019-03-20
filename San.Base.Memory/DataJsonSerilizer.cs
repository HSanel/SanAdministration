using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace San.Base.Memory
{
    public class DataJsonSerilizer
    {
        public void DeserilizeAllData(string path, ref Hashtable inData)
        {
            using (StreamReader file = File.OpenText(path))
            {
                JsonSerializer serilizer = new JsonSerializer();
                inData = (Hashtable)serilizer.Deserialize(file, typeof(Hashtable));
            }
        }


        public bool SerilizeAllData(string path, ref Hashtable outData)
        {
            using (StreamWriter file = File.CreateText(path))
            {
                JsonSerializer serilizer = new JsonSerializer();
                serilizer.Serialize(file, outData);
                return true;
            }
            return false;
        }
    }
}
