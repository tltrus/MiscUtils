using System.IO;

namespace MiscUtils
{
    namespace IO
    {
        public class Files
        {
            /// <summary>
            /// Чтение файла с данными
            /// </summary>
            /// <returns></returns>
            public static string[] FileRead(string name)
            {
                if (!File.Exists(name)) return null;

                try
                {
                    // чтение строки из файла
                    return File.ReadAllLines(name);
                }
                catch (IOException e)
                {
                    //Console.WriteLine("I/O error occured" + e);
                    return null;
                }
            }


            /// <summary>
            /// Функция сохранения сетки в файл
            /// </summary>
            public static void SaveToFile(string name, string[] rows)
            {
                if (!File.Exists(name)) File.Create(name).Close(); // создаем файл, если его нет

                // Сохранение файла
                File.WriteAllLines(name, rows);
            }

        }
    }
}
