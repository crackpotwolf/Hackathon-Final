from transliterate import translit
from typing import Dict, List

import random
import string


class ApplicantGenerator:
  """Класс генерации Заявителя
  """

  def __init__(self) -> None:
    self.name = ""
    self.lastname = ""
    self.patronymic = ""
    self.email = ""
    self.phone = ""
    self.role = ""

  def __generate__(self, names, lastnames, patronymics, roles) -> None:
    # случайно выбирается имя
    def random_name(names: Dict, gender: str, current_random: random.SystemRandom):
      return current_random.choice(names[gender])

    # случайно выбирается фамилия
    def random_lastname(lastnames: Dict, gender: str, current_random: random.SystemRandom):
      return current_random.choice(lastnames[gender])

    # случайно выбирается отчество
    def random_patronymic(patronymics: Dict, gender: str, current_random: random.SystemRandom):
      return current_random.choice(patronymics[gender])

    # генерируется email
    def generate_email(name: str, lastname: str, current_random: random.SystemRandom):
      year_burn = str(current_random.randint(1980, 2000))
      lat_name = translit(value=name, language_code="ru", reversed=True)
      lat_lastname = translit(value=lastname, language_code="ru", reversed=True)
      server_name = current_random.choice(["gmail.com", "inbox.ru", "mail.ru", "rambler.ru", "yandex.ru", "ya.ru"])

      first_part = current_random.choice([lat_name, lat_lastname]).replace(string.punctuation, '')
      second_part = lat_lastname.replace(string.punctuation, '') if first_part == lat_name else lat_name.replace(string.punctuation, '')

      email = "%s_%s_%s@%s" % (first_part, second_part, year_burn, server_name)
      return email

    # генерируется номер телефона
    def generate_phone(current_random: random.SystemRandom):
      first_part = str(current_random.randint(100, 999))
      second_part = str(current_random.randint(100, 999))
      third_part = str(current_random.randint(1000, 9999))
      return "+7%s%s%s" % (first_part, second_part, third_part)

    # случайно выбирается роль
    def random_role(roles: List, current_random: random.SystemRandom):
      return current_random.choice(roles)

    current_random = random.SystemRandom()
    gender = current_random.choice(["m", "w"])

    self.name = random_name(names=names, gender=gender, current_random=current_random)
    self.lastname = random_lastname(lastnames=lastnames, gender=gender, current_random=current_random)
    self.patronymic = random_patronymic(patronymics=patronymics, gender=gender, current_random=current_random)
    self.email = generate_email(name=self.name, lastname=self.lastname, current_random=current_random)
    self.phone = generate_phone(current_random=current_random)
    self.role = random_role(roles=roles, current_random=current_random)
