import os, shutil
import xml.etree.ElementTree as XmlReader
import tkinter as tk
from tkinter import messagebox as mb
from tkinter import Tk, filedialog, Button, Canvas, Label, Listbox, Scrollbar
from pathlib import Path

class Gui:
    def __init__(self, master):
        self.master = master
        self.create_widgets()

    def create_widgets(self):
        self.master.title("Копирование файлов из config.xml")
        self.master.maxsize(600, 290)
        self.master.minsize(600, 290)
        # Общий макет
        self.canvas = Canvas(
            self.master,
            width=550,
            height=270
        )
        # Кнопка. Выбор файла config
        self.openFileBtn = Button(
            self.master,
            text="Выбрать файл",
            command=self.openFile,
            padx=25,
            pady=25
        )
        # Полоса прокрутки для listBox
        self.scrollbar = Scrollbar(self.master)
        self.scrollbar.pack(side=tk.RIGHT, fill=tk.Y)
        # Вывод позиций в xml для файлов, которые не удалось скопировать
        self.listBox = Listbox(yscrollcommand=self.scrollbar.set, selectmode=tk.EXTENDED, width=55)
        # Связываем полосу прокрутки со списоком
        self.scrollbar.config(command=self.listBox.yview)
        self.errorLog = []

        # Справка
        self.helpLabel = Label(self.master, text="Для копирования ошибки -> Ctrl+C. Можно выбрать несколько пунктов.")
        # Строка предупреждения
        self.warningLabel = Label(
            self.master,
            text="Существующие файлы в папке назначения будут перезаписаны!!!",
            font=('calibri', 11),
            fg='#f00'
        )
        # Информ строка
        self.errorLabel = Label(self.master, text="Не удалось скопировать (в файле):")

        # Размещаем элементы
        self.canvas.pack()
        self.canvas.create_window(90, 160, window=self.openFileBtn)
        self.canvas.create_window(305, 65, window=self.errorLabel)
        self.canvas.create_window(300, 15, window=self.helpLabel)
        self.canvas.create_window(300, 40, window=self.warningLabel)
        self.canvas.create_window(375, 165, window=self.listBox)

    # Открытие диалогового окна для выбора файла
    def openFile(self):
        self.answer = mb.askyesno(title='Внимание!',
                          message='Все пути в файле config должны быть абсолютными, иначе копирование невозможно!!!\nХотите продолжить?')
        if self.answer:
            self.filetypes = (
                ('xml files', '*.xml'),
                ('All files,', '*.*')
            )
            self.filename = filedialog.askopenfilename(
                title='Выбрать файл',
                initialdir='/',
                filetypes=self.filetypes
            )
            if len(self.filename) == 0:
                mb.showinfo("Ошибка", "Перед копированием необходимо выбрать файл!")
            else:
                file_path = os.path.abspath(self.filename)
                # Показываем в окне путь до файла config.xml
                self.filePath = Label(
                    self.master,
                    text=file_path,
                    font=('calibri', 11)
                )
                self.filePath.pack()

                # Вызываем функцию для чтения и копирования файлов
                self.ReadAndCopy(file_path)
                if len(self.errorLog) >= 1:
                    # Проверка на наличие ошибок при копировании
                    mb.showwarning("Внимание!", "Во время копирования были обнаружены ошибки!")
                    if len(self.errorLog) >= 1:
                        for i in self.errorLog:
                            self.listBox.insert(tk.END, i)

                else:
                    mb.showinfo("Внимание!", "Данные успешно скопированы!")

    # Чтение xml и копирование файлов
    def ReadAndCopy(self, file_path):
        xmlData = XmlReader.parse(file_path).getroot()
        dataList = []   # промежуточный список
        for data in xmlData:
            # Заполняем промежуточный список путями и именем файла
            for child in data:
                item = child.text.strip('"')
                if len(item) == 0:
                    self.errorLog.append([
                        "Пустая строка", child.tag
                    ])
                dataList.append(item)
            # Вызываем функцию копирования
            self.CopyFile(dataList)
            # Удаляем данные из списка перед следующей итерацией
            dataList.clear()
            
    # Копирование файлов
    def CopyFile(self, data):
        # Получаем абсолютный путь до папок
        source_path = Path(data[0]).resolve()
        destination_path = Path(data[1]).resolve()
        file = data[2]

        try:
            source = Path().joinpath(source_path, file)
            destination = Path().joinpath(destination_path, file)

            # Проверяем существование файлов и папок в системе
            source_exists = source_path.exists()
            destination_exists = destination_path.exists()
            file_exists = source.exists()

            if (source_exists == False or destination_exists == False or file_exists == False):
                if source_exists == False:
                    self.errorLog.append([
                        "Папка не найдена", source_path
                    ])
                if destination_exists == False:
                    self.errorLog.append([
                        "Папка не найдена", destination_path
                    ])
                if file_exists == False:
                    self.errorLog.append([
                        "Файл не найден", file
                    ])
            else:
                # Если нет ошибок, копируем файлы
                shutil.copy2(src=source, dst=destination, follow_symlinks=True)

        except PermissionError:
            self.errorLog.append([
                "Нужно разрешение", destination_path
            ])
        except OSError:
            self.errorLog.append([
                "Запись запрещена", destination_path
            ])

if __name__ == "__main__":
    root = Tk()
    my_gui = Gui(root)
    root.mainloop()
