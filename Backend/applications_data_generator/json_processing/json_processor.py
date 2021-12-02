from config import *

import json
import os


def write_to_json_in_dir(filename, data, dir):
  # сформировать путь для сохранения файла
  path_to_save = os.path.join(PATH_TO_DATA, dir)
  # если папки нет, то создаем ее
  if not os.path.exists(path_to_save):
    # создаем папку и имя файла
    os.makedirs(path_to_save)
  # задать путь для сохранения файла
  filepath = os.path.join(path_to_save, filename)
  with open(filepath, "w", encoding="utf-8") as filejson:
    json.dump(data, filejson, ensure_ascii=False)


def read_from_json_in_dir(filename, dir):
  # сформировать путь для сохранения файла
  path_to_read = os.path.join(PATH_TO_DATA, dir)
  # если папки нет, то возвращаем пустой лист []
  if not os.path.exists(path_to_read):
    return []
  else:
    # задать путь для чтения файла
    path_to_read = os.path.join(path_to_read, filename)
    # если файла нет, то возвращаем пустой лист []
    if not os.path.exists(path_to_read):
      return []
    else:
      # читаем файл и возвращаем результат
      with open(path_to_read, "r", encoding="utf-8") as filejson:
        return json.load(filejson)
