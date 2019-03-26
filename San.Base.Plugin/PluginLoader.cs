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
        static ICollection<IPlugin> plugins;

        public PluginLoader()
        {
            plugins = new List<IPlugin>();
        }

        public ICollection<IPlugin> LoadPlugins(string pluginPath)
        {
            if (Directory.Exists(pluginPath))
            {
                string[] dllFileNames = Directory.GetFiles(pluginPath, "*.dll");
                string[] directoryNames = Directory.GetDirectories(pluginPath);
                ICollection<Assembly> assemblies = new List<Assembly>(dllFileNames.Length);
                foreach(string dllFile in dllFileNames)
                {
                    AssemblyName assemblyName = AssemblyName.GetAssemblyName(dllFile);
                    Assembly assembly = Assembly.Load(assemblyName);
                    assemblies.Add(assembly);
                }

                Type pluginType = typeof(IPlugin);
               

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

                foreach(string directory in directoryNames)
                {
                    LoadPlugins(directory);
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
