using System;
using System.IO;
using System.Collections.Generic;
using System.Text;

namespace FileManager
{
    class Program
    {
        /// <summary>
        /// Вывод вступительного слова.
        /// </summary>
        static void Start()
        {
            Console.WriteLine("Приветствие");
        }
        /// <summary>
        /// Функция реализующая 1-ую операцию(список дисков, а также выбор).
        /// </summary>
        static public DriveInfo AllDisks()
        {
            DriveInfo[] disks = DriveInfo.GetDrives();
            Console.WriteLine("Все диски вышего компьютера:");
            for (int i = 0; i < disks.Length; ++i)
            {
                Console.WriteLine($"{i + 1}: {disks[i]}");
            }
            Console.WriteLine("Введите НОМЕР диска который хотите выбрать из списка(индексация начинаеться с 1)");
            uint ind = 0;
            while (!uint.TryParse(Console.ReadLine(), out ind) || ind == 0 || ind > disks.Length)
            {
                Console.WriteLine("Вы ввели, что-то другое, попробуйте снова");
            }
            ind--;
            Console.WriteLine($"Отлично, вы выбрали диск {disks[ind]}");
            return disks[ind];
        }
        /// <summary>
        /// Функция реализующая 2-ую операцию(выбор директории).
        /// </summary>
        static public string ChooseFolder(string path)
        {
            try
            {
                List<string> folders = new List<string>(Directory.EnumerateDirectories(@path));
                Console.WriteLine("Все папки настоящей директории/диска");
                for (int i = 0; i < folders.Count; ++i)
                {
                    Console.WriteLine($"{i + 1}: {folders[i]}");
                }
                Console.WriteLine("Введите НОМЕР папки который хотите выбрать из списка(индексация начинаеться с 1)");
                uint ind = 0;
                while (!uint.TryParse(Console.ReadLine(), out ind) || ind == 0 || ind > folders.Count)
                {
                    Console.WriteLine("Вы ввели, что-то другое, попробуйте снова");
                }
                ind--;
                path = folders[(int)ind];
                Console.WriteLine($"Отлично, вы выбрали папку {folders[(int)ind]}");
            }
            catch
            {
                Console.WriteLine("У этой папки/диска нельзя узнать её содержимое");
            }
            return path;
        }
        static public void FilesInFolder(string path)
        {
            string[] files = Directory.GetFiles(@path);
            Console.WriteLine("Все файлы настоящей директории/диска");
            for (int i = 0; i < files.Length; ++i)
            {
                Console.WriteLine($"{i + 1}: {files[i]}");
            }
        }
        static public uint ChooseOperation()
        {
            Console.WriteLine("Выберите операцию(Введите её номер)");
            Console.WriteLine("1.просмотр списка дисков компьютера и выбор диска;\n" +
                "2.переход в другую директорию(выбор папки);\n" +
                "3.просмотр списка файлов в директории;\n" +
                "4.вывод содержимого текстового файла в консоль в кодировке UTF-8;\n" +
                "5.вывод содержимого текстового файла в консоль в выбранной пользователем кодировке(предоставляется не менее трех вариантов);\n" +
                "6.копирование файла;\n" +
                "7.перемещение файла в выбранную пользователем директорию;\n" +
                "8.удаление файла;\n" +
                "9.создание простого текстового файла в кодировке UTF-8;\n" +
                "10.создание простого текстового файла в выбранной пользователем кодировке(предоставляется не менее трех вариантов);\n" +
                "11.конкатенация содержимого двух или более текстовых файлов и вывод результата в консоль в кодировке UTF-8.\n" +
                "12.закончить программу");
            uint operation = 0;
            while (!uint.TryParse(Console.ReadLine(), out operation) || operation == 0 || operation > 11)
            {
                Console.WriteLine("Вы Ввели, что-то другое, попробуйте снова");
            }
            return operation;
        }
        static bool CheckTxt(string path)
        {
            int len = path.Length;
            return path.Length >= 4 && path[len - 1] == 't' && path[len - 2] == 'x' && path[len - 3] == 't' && path[len - 4] == '.';
        }
        static public void PrintFileTxt(string path, string enc = "")
        {
            var encoding = Encoding.UTF8;
            if (enc == "")
            {
                Console.WriteLine("Введите кодировку(UTF8, UTF32, ASCII, Unicode)");
                while (enc != "UTF8" && enc != "UTF32" && enc != "ASCII" && enc != "Unicode")
                {
                    Console.WriteLine("Вы ввели, что-то другое");
                    enc = Console.ReadLine();
                }
                switch (enc)
                {
                    case "UTF8":
                        encoding = Encoding.UTF8;
                        break;
                    case "UTF32":
                        encoding = Encoding.UTF32;
                        break;
                    case "ASCII":
                        encoding = Encoding.ASCII;
                        break;
                    default:
                        encoding = Encoding.Unicode;
                        break;
                }
            }
            string pathFile = ChooseFile(path);
            Console.WriteLine(File.ReadAllText(@pathFile, encoding));
        }
        static public string ChooseFile(string path)
        {
            string[] files = Directory.GetFiles(@path);
            Console.WriteLine("Все файлы настоящей директории/диска");
            for (int i = 0; i < files.Length; ++i)
            {
                Console.WriteLine($"{i + 1}: {files[i]}");
            }
            Console.WriteLine("Введите НОМЕР файла который хотите выбрать из списка(индексация начинаеться с 1)");
            uint ind = 0;
            while (!uint.TryParse(Console.ReadLine(), out ind) || ind == 0 || ind > files.Length)
            {
                Console.WriteLine("Вы ввели, что-то другое, попробуйте снова");
            }
            return files[ind - 1];
        }
        static public void CopyFile(string path)
        {
            string pathFile = ChooseFile(path);
            string text = File.ReadAllText(@pathFile);
            Console.WriteLine("Введите имя нового файла вместе с расширением");
            string pathCopy = path + "\\" + Console.ReadLine();
            //try
            //{
            File.Create(@pathCopy);
            File.WriteAllText(@pathCopy, text);
            string[] files2 = Directory.GetFiles(@path);
            Console.WriteLine("Все файлы настоящей директории/диска(после копирования)");
            for (int i = 0; i < files2.Length; ++i)
            {
                Console.WriteLine($"{i + 1}: {files2[i]}");
            }
            //}
            //catch
            //{
            //Console.WriteLine("Вы ввели что-то не так, программа не умеет работать с таким расширением или такой файл уже существует");
            //}
        }
        static public void Delete(string path)
        {
            string pathFile = ChooseFile(path);
            File.Delete(@pathFile);
        }
        static public void PasteFile(string path)
        {
            Console.WriteLine("Выберите файл, который хотите переместить");
            string pathFile = ChooseFile(path);
            string text = File.ReadAllText(@pathFile);
            while (true)
            {
                try
                {
                    Console.WriteLine("Введите путь к папке, в которую вы бы хотели вставить файл");
                    string newPath = Console.ReadLine().Trim(new char[] { '"' });
                    File.Create(@newPath);
                    File.WriteAllText(@newPath, text);
                    break;
                }
                catch
                {
                    Console.WriteLine("Вы ввели не верный путь, или в этой папке уже есть файл с таким именем. Попробуйте снова");
                    continue;
                }
            }
            File.Delete(@pathFile);
        }
        static void MakeFile(string path, string enc = "")
        {
            var encoding = Encoding.UTF8;
            //if (enc )
        }
        static public string Work(uint operation, string path)
        {
            switch (operation)
            {
                case 1:
                    path = Convert.ToString(AllDisks());
                    break;
                case 2:
                    path = ChooseFolder(path);
                    break;
                case 3:
                    FilesInFolder(path);
                    break;
                case 4:
                    PrintFileTxt(path, "utf8");
                    break;
                case 5:
                    PrintFileTxt(path);
                    break;
                case 6:
                    CopyFile(path);
                    break;
                case 7:
                    PasteFile(path);
                    break;
                case 8:
                    Delete(path);
                    break;
                case 9:
                    MakeFile(path, "utf8");
                    break;
                case 10:
                    MakeFile(path);
                    break;
                default:
                    break;

            }
            Console.WriteLine();
            return path;
        }
        static public void Main(string[] args)
        {
            string path = "";
            while (true)
            {
                Start();
                uint operation = ChooseOperation();
                if (operation == 12)
                {
                    return;
                }
                path = Work(operation, path);
            }
        }
    }
}
