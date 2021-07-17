using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.IO;

namespace CopyFile
{
    public partial class Form1 : Form
    {
        // Список для вывода ошибок при копировании
        public List<string> errorLog = new List<string>();

        public Form1()
        {
            InitializeComponent();
            // Кнопка выбора файла
            openFileBtn.Click += OpenFileBtn_Click;
            // Кнопка копирования списка ошибок в буфер
            copyErrorLogBtn.Click += CopyErrorLogBtn_Click;
        }

        private void CopyErrorLogBtn_Click(object sender, EventArgs e)
        {
            try
            {
                if (errorLog.Count >= 1)
                {
                    StringBuilder data = new StringBuilder();
                    foreach (string error in errorLog)
                    {
                        data.AppendLine(error);
                    }
                    // Копируем в буфер обмена
                    Clipboard.SetText(data.ToString());

                    MessageBox.Show("Список ошибок скопирован!", "Внимание!");
                }
                else
                {
                    MessageBox.Show("Ошибок не найдено!", "Внимание!");
                }
            }
            catch
            {
                MessageBox.Show("Произошло что-то неведомое и не удалось скопировать ошибки в буфер обмена", "Внимание!");
            }
        }

        private void OpenFileBtn_Click(object sender, EventArgs e)
        {
            
            try
            {
                string filePath = "";

                // Предупреждаем о возможных ошибках
                DialogResult result = MessageBox.Show(
                    "Все пути в файле config должны быть абсолютными, иначе копирование невозможно!!!\nХотите продолжить?",
                    "Внимание",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Information,
                    MessageBoxDefaultButton.Button1
                    );

                if (result == DialogResult.Yes)
                {
                    // Открываем окно выбора файла
                    using (OpenFileDialog openFileDialog = new OpenFileDialog())
                    {
                        openFileDialog.Filter = "XML files (*.xml)|*.xml|All files (*.*)|*.*";
                        if (openFileDialog.ShowDialog() == DialogResult.OK)
                        {
                            // Получаем путь до файла
                            filePath = openFileDialog.FileName;
                        }

                    }
                    // функция копирования
                    ReadXmlAndCopy(filePath);
                }

            }
            catch
            {
                MessageBox.Show(
                    "Во время копирования данных что-то пошло не так. Попробуйте снова !",
                    "Ошибка",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
        }

        // Функция чтения xml
        private void ReadXmlAndCopy(string filePath)
        {
            List<string> dataList = new List<string>();
            // Загружаем файл
            XmlDocument document = new XmlDocument();
            document.Load(filePath);
            
            // Читаем элементы
            XmlElement element = document.DocumentElement;
            foreach (XmlNode node in element)   // здесь тэг file
            {
                foreach (XmlNode childnode in node.ChildNodes)  // здесь тэги source_path и тд
                {
                    string item = childnode.InnerText.Trim('"');
                    if (item == "")
                    {
                        errorLog.Add("Пустая строка -> " + childnode.Name);
                    }
                    dataList.Add(item);
                }
                // Вызываем функцию копирования
                CopyFile(dataList);
                // Очищаем список перед следующей итерацией
                dataList.Clear();
            }
            if (errorLog.Count >= 1)
            {
                MessageBox.Show("Часть файлов не удалось скопировать!", "Внимание!");
                foreach (String item in errorLog)
                {
                    errorLogListBox.Items.Add(item);
                }
            }
            else
            {
                MessageBox.Show("Файлы успешно скопированы!", "Внимание!");
            }
            
        }
        
        // Функция копирования
        private void CopyFile(List<string> data)
        {
            string source_path = Path.GetFullPath(data[0]);
            string destination_path = Path.GetFullPath(data[1]);
            string file = data[2];
            try
            {
                // Полные пути, откуда нужно взять и куда копировать
                string source = Path.Combine(source_path, file);
                string destination = Path.Combine(destination_path, file);

                // Проверяем их существование
                bool source_exists = Directory.Exists(source_path);
                bool destination_exists = Directory.Exists(destination_path);
                bool file_exists = File.Exists(source);

                if (source_exists == false || destination_exists == false || file_exists == false)
                {
                    if (source_exists == false)
                    {
                        errorLog.Add("Папка не найдена -> " + source_path);
                    }
                    if (destination_exists == false)
                    {
                        errorLog.Add("Папка не найдена -> " + destination_path);
                    }
                    if (file_exists == false)
                    {
                        errorLog.Add("Файл не найден -> " + file);
                    }
                }
                else
                {
                    // Копируем файл
                    File.Copy(source, destination, true);
                }
            }
            catch (UnauthorizedAccessException)
            {
                errorLog.Add("Нужно разрешение -> " + destination_path);
            }
            catch (ArgumentException)
            {
                // Выше уже обрабатываем пустые строки
                // Это исключение "пустое", чтобы не прекращать общий поток выполнения
            }
            catch (PathTooLongException)
            {
                errorLog.Add("Слишком длинный путь -> " + source_path);
                errorLog.Add("Слишком длинный путь -> " + destination_path);
            }
            catch (DirectoryNotFoundException)
            {
                // Выше уже обрабатываем отсутствие папки
            }
            catch (FileNotFoundException)
            {
                // Выше уже обрабатываем отсутствие файла
            }
            // Выше (в проверке существования папок и файла) мы определяем наверняка какой папки не существует
        }
    }
}
