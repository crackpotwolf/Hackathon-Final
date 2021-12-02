from loading.DataLoader import DataLoader

import random


class TransportComplexCoordinatorsGenerator:
  """Класс генерации основных координаторов от организации транспортного комплекса Москвы
  """
  def __init__(self) -> None:
    self.fullnames = []
  
  def __generate_fullnames__(self, data_loader: DataLoader):
    current_random = random.SystemRandom()
    coordinators_number = current_random.randint(3, 7)
    for coordinate_num in range(coordinators_number):
      gender = current_random.choice(["m", "w"])
      name = current_random.choice(data_loader.names[gender])
      lastname = current_random.choice(data_loader.lastnames[gender])
      patronymic = current_random.choice(data_loader.patronymics[gender])
      #
      self.fullnames.append("%s %s %s" % (name, lastname, patronymic))
