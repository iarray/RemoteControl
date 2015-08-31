using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Plugin
{
    public class PluginLoader
    {
       /// <summary>
        /// 从指定路径加载特定T类型的插件
       /// </summary>
       /// <typeparam name="T">插件类型</typeparam>
        /// <param name="pluginsDirPath">插件目录地址</param>
        /// <param name="extension">插件的文件的后缀名,如:"*.dll"</param>
       /// <returns>返回T类插件集合</returns>
        public static List<T> Load<T>(string pluginsDirPath, string extension = "*.dll")
        {
            string[] pluginsFilesPath = Directory.GetFiles(pluginsDirPath, extension);
            List<T> pluginList = new List<T>();
            foreach (string path in pluginsFilesPath)
            {
                Assembly asm = Assembly.LoadFile(path);
                Type[] types = asm.GetExportedTypes();
                Type type = typeof(T);
                foreach (Type t in types)
                {
                    if (type.IsAssignableFrom(t))
                    {
                        try
                        {
                            T pc = (T)Activator.CreateInstance(t);
                            pluginList.Add(pc);
                        }
                        catch { }
                    }
                }
            }
            return pluginList;
        }
    }
}
