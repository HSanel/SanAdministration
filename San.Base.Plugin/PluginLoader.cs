using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace San.Base.Plugin
{
    public class PluginLoader
    {
        string[] dllFileNames = null;
        string pluginPath = "";
        ICollection<Assembly> assemblies;
        ICollection<IPlugin> plugins;

        public PluginLoader(string pluginPath)
        {
            this.pluginPath = pluginPath;           
        }

        public ICollection<IPlugin> LoadPlugins()
        {
            if (Directory.Exists(pluginPath))
            {
                dllFileNames = Directory.GetFiles(pluginPath, "*.dll");
                assemblies = new List<Assembly>(dllFileNames.Length);
                foreach(string dllFile in dllFileNames)
                {
                    AssemblyName assemblyName = AssemblyName.GetAssemblyName(dllFile);
                    Assembly assembly = Assembly.Load(assemblyName);
                    assemblies.Add(assembly);
                }

                Type pluginType = typeof(IPlugin);
                plugins = new List<IPlugin>();

                foreach (Assembly assembly in assemblies)
                {
                    if(assembly != null)
                    {
                        Type[] types = assembly.GetTypes();

                        foreach(Type type in types)
                        {
                            if(type.IsInterface || type.IsAbstract)
                            {
                                continue;
                            }
                            else
                            {
                                if(type.GetInterface(pluginType.FullName) != null)
                                {
                                    IPlugin plugin = (IPlugin)Activator.CreateInstance(type);
                                    plugins.Add(plugin);
                                    plugin.OnLoad();
                                }
                            }
                        }
                    }
                }

                return plugins;
            }
            else
            {
                return null;
            }
        }
    }
}
