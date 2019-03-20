using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace San.Base.Memory
{
    public class ProjectMemory
    {
        Hashtable outMemory = new Hashtable();
        Hashtable inMemory;

        public object Jobject { get; private set; }

        public void ClearOutputMemory()
        {
            outMemory.Clear();
        }

        public void ClearInputMemory()
        {
            if(inMemory != null)
               inMemory.Clear();
        }

        public void SaveData(object obj)
        {
            if (obj.GetType() == typeof(TextBox))
            {
                TextBox textBox = (TextBox)obj;
                outMemory.Add(textBox.Name, textBox.Text);
            }
            else if (obj.GetType() == typeof(CheckBox))
            {
                CheckBox checkBox = (CheckBox)obj;
                outMemory.Add(checkBox.Name, checkBox.IsChecked);
            }

            if (obj is DependencyObject == false) return;
            foreach (object child in LogicalTreeHelper.GetChildren(obj as DependencyObject))
            {
                SaveData(child);
            }
        }

        public void ReadData(object obj)
        {
            try
            {
                if (obj.GetType() == typeof(TextBox))
                {
                    TextBox textBox = (TextBox)obj;
                    textBox.Text = (string)inMemory[textBox.Name];
                }
                else if (obj.GetType() == typeof(CheckBox))
                {
                    CheckBox checkBox = (CheckBox)obj;
                    checkBox.IsChecked = (bool)inMemory[checkBox.Name];
                }

                if (obj is DependencyObject == false) return;
                foreach (object child in LogicalTreeHelper.GetChildren(obj as DependencyObject))
                    ReadData(child);
            }
            catch
            {
            }
           
        }

        public bool SerilizeData(string path)
        {
            using (StreamWriter file = File.CreateText(path))
            {
                JsonSerializer serilizer = new JsonSerializer();
                serilizer.Serialize(file, outMemory);
                return true;
            }
            return false;
        }

        public Hashtable GetOutData()
        {
            return outMemory;
        }

        public void SetInData(object inData)
        {
            JsonSerializer serilizer = new JsonSerializer();
            inMemory = (Hashtable)((JObject)inData).ToObject(typeof(Hashtable));
        }

        public bool DeserilizeData(string path)
        {
            using (StreamReader file = File.OpenText(path))
            {
                JsonSerializer serilizer = new JsonSerializer();
                inMemory = (Hashtable)serilizer.Deserialize(file, typeof(Hashtable));
                return true;
            }
            return false;
        }
    }
}
