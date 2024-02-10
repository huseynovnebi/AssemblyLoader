using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace FileLoaderGUI.Extentions
{
    public static class DllLoading
    {
        private static string dllpath = Path.Combine(Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName, "Dlls");
        public static async Task LoadDll(List<Tuple<string, string>> filesDataList, DataGrid dataGrid)
        {
            List<Task> tasks = new List<Task>();

            foreach (var file in filesDataList)
            {
                tasks.Add(Task.Run(async () =>
                {
                    string fileExtension = file.Item1;
                    fileExtension = fileExtension.TrimStart('.');
                    object[] parameters = { file.Item2 };

                    Assembly assembly = Assembly.LoadFrom(Path.Combine(dllpath, fileExtension + "Loader.dll"));
                    Type? className = assembly.GetType(String.Format("{0}Loader.{0}Loader", fileExtension));

                    if (className != null)
                    {
                        object? instance = Activator.CreateInstance(className);
                        MethodInfo? method = className.GetMethod("LoadContent");

                        object? result = await Task.Run(() => method.Invoke(instance, parameters));

                        IEnumerable? obj = result as IEnumerable;

                        if (obj != null)
                        {
                            foreach (var value in obj)
                            {
                                await dataGrid.Dispatcher.InvokeAsync(() => dataGrid.Items.Add(value));
                            }
                        }
                    }
                    else
                    {
                        System.Windows.MessageBox.Show("Class is not found!" + className);
                    }
                }));
            }

            await Task.WhenAll(tasks);
        }

    }
}
