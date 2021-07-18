import argparse
import hashlib
import os
from pathlib import Path

# Что если в передаваемых путях до файла и/или до папки будут пробелы?
# Если не передаем аргументы, выйдет подсказка
# Если передаем аргументы, но без ключей, выйдет подсказка

def GetArgv():
    filePath = ""
    directoryPath = ""
    parse = argparse.ArgumentParser()
    parse.add_argument("-f", "--file",
                       help='"Путь до файла" (заключен в двойные кавычки)',
                       type=str)
    parse.add_argument("-d", "--directory",
                       help='"Путь до папки" (заключен в двойные кавычки)',
                       type=str)
    args = parse.parse_args()
    if args.file:
        filePath = args.file
    if args.directory:
        directoryPath = args.directory

    return filePath, directoryPath

def CheckFile(filePath, directoryPath):
    result = []
    global blockSize
    blockSize = 32768   # 32 КБ 2^15 - размер буфера
    #blockSize = 65536   # 64 КБ 2^16 - размер буфера

    # Если не передали какой-либо из аргументов
    if len(filePath) == 0 or len(directoryPath) == 0:
        print(
            "Пример комнады\n" +
            os.path.basename(__file__) + ' -f "Путь до файла" -d "Путь до папки"')
    else:
        try:
            # Проверяем существование папки и файла
            directoryExists = Path(directoryPath).resolve().exists()
            fileExists = Path(filePath).resolve().exists()

            if directoryExists == False or fileExists == False:
                if directoryExists == False:
                    print("Указанная директория не найдена")
                if fileExists == False:
                    print("Указанный файл не найден")
            else:
                with open(filePath, 'r', encoding='utf-8') as file:
                    data = file.readlines()
                    lines = []
                    for i in data:
                        # Пропускаем перенос строки
                        if i == '\n':
                            continue
                        # Удаляем перенос строки у самого элемента и разделяем строку по пробелам
                        lines.append(i.rstrip().split(" "))
                    try:
                        for i in lines:
                            fileName = i[0]
                            algorithm = i[1].lower()
                            inputHashSumm = i[2]
                            # Получаем полный путь до файла, который нужно проверить
                            checkFilePath = Path().joinpath(directoryPath, fileName).resolve()
                            # Проверка наличия файла
                            checkFileExists = Path(checkFilePath).exists()
                            if checkFileExists:
                                # Определяем алгоритм шифрования (MD5/SHA1/SHA256)
                                # Получаем хэш-сумму
                                # Сравниваем и выводим результат
                                if algorithm == 'md5':
                                    fileHash = hashlib.md5()
                                    CheckHashSumm(fileHash, checkFilePath, fileName, inputHashSumm, result)
                                elif algorithm == 'sha1':
                                    fileHash = hashlib.sha1()
                                    CheckHashSumm(fileHash, filePath, fileName, inputHashSumm, result)
                                elif algorithm == 'sha256':
                                    fileHash = hashlib.sha256()
                                    CheckHashSumm(fileHash, filePath, fileName, inputHashSumm, result)
                                else:
                                    result.append(fileName + " UNKNOWN ALGORITHM")
                            else:
                                result.append(fileName + " NOT FOUND")

                    # Если не указали имя/алгоритм/хэш-сумму
                    except (IndexError):
                        print(
                            "В одной из строк не найдено: имя файла/алгоритм/хэш-сумма\nПожалуйста проверьте файл и попробуйте заново")
        except (OSError):
            print("В конце строки - путь до файла - не нужен / или \\")
    for i in result:
        print(i)

# Функция для вычисления и проверки хэш-сумм
def CheckHashSumm(fileHash, filePath, fileName, inputHashSumm, resultList):
    # Открываем файл для чтения (байт)
    with open(filePath, 'rb') as f:
        fileBytes = f.read(blockSize)
        # Пока есть данные продолжаем читать
        while len(fileBytes) > 0:
            fileHash.update(fileBytes)
            fileBytes = f.read(blockSize)
    # Получили хэш-сумму для файла
        hashSumm = fileHash.hexdigest()
    # Сравниваем
    if hashSumm == inputHashSumm:
        resultList.append(fileName + " OK")
    else:
        resultList.append(fileName + " FAIL")


file, directory = GetArgv()
CheckFile(file, directory)
