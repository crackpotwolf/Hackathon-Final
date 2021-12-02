from datetime import date, datetime
from dateutil import rrule
from dateutil.relativedelta import relativedelta
from generating.ApplicantGenerator import ApplicantGenerator
from generating.ApplicationGenerator import ApplicationGenerator
from loading.DataLoader import DataLoader
from pymorphy2 import MorphAnalyzer
from string import punctuation
from transliterate import translit
from typing import Dict, List

import random
import string


class ProductPassportGenerator:
  """Класс генераци паспорта продукта
  """
  def __init__(self) -> None:
    self.name = ""
    self.transportComplexOrganization = ""
    self.pilotMember = ""
    self.leader = ""
    self.pilotCoordinator = ""
    self.transportComplexCoordinator = ""
    self.shortDescription = ""
    self.timing = ""
    self.context = ""
    self.pathName = ""
    self.tags = []
    self.order = {}
    self.effects = []
    self.stages = []
    self.teams = []
    self.budget = []
    self.statuses = []
    self.activities = []
    self.meetings = []
    self.materials = []
    
    #
    self.product_passport_data = {}
    
  def __generate__(self, application: ApplicationGenerator, data_loader: DataLoader, transport_complex_coordinators: List):
    
    current_random = random.SystemRandom()
    
    def generate_name(adjectives, nouns, current_random: random.SystemRandom):
      fl_found_noun = False
      morph = MorphAnalyzer(lang="ru")

      while not fl_found_noun:
        noun = current_random.choice(nouns)

        noun_info_list = morph.parse(noun)
        for i in range(0, len(noun_info_list)):
          if "NOUN" in noun_info_list[i].tag:
            if "masc" in noun_info_list[i].tag:
              fl_found_noun = True
              break

      adjective = current_random.choice(adjectives)

      return "%s %s" % (adjective.upper(), noun.upper())
    
    def generate_timing(num_month: int, num_year: int):
      return datetime(year=num_year, month=num_month, day=1)
    
    def generate_effects(effects_count: int):
      for effect_num in range(effects_count):
        effect_name = data_loader.__get_random_text_request__(sentences_count=1, paragraph_count=0)
        effect_value = data_loader.__get_random_text_request__(sentences_count=5, paragraph_count=0)
        self.effects.append({
          "name": effect_name,
          "value": effect_value
        })
    
    def generate_stages():
      
      current_date = date.today()
      
      step = int(((self.timing.year - current_date.year) * 12 + (self.timing.month - current_date.month)) / 5)
      
      self.timing = "%s.%s" % (str(self.timing.month), str(self.timing.year))
      
      self.stages = [
        {
            "name": "Позиционирование",
            "date": "%s.%s" % (str((current_date + relativedelta(months=step)).month), str((current_date + relativedelta(months=step)).year))
        },
        {
            "name": "Уточнение деталей продуктового предложения",
            "date": "%s.%s" % (str((current_date + relativedelta(months=2*step)).month), str((current_date + relativedelta(months=step)).year))
        },
        {
            "name": "Подготовка к пилотному тестированию",
            "date": "%s.%s" % (str((current_date + relativedelta(months=3*step)).month), str((current_date + relativedelta(months=step)).year))
        },
        {
            "name": "Проведение пилотного тестирования",
            "date": "%s.%s" % (str((current_date + relativedelta(months=4*step)).month), str((current_date + relativedelta(months=step)).year))
        },
        {
            "name": "Формирование отчета о пилотном тестировании",
            "date": self.timing
        }
      ]
    
    def generate_team(data_loader: DataLoader, count_participants: int, current_random: random.SystemRandom):
      for num_participant in range(count_participants):
        participant = ApplicantGenerator()
        participant.__generate__(names=data_loader.names, lastnames=data_loader.lastnames, patronymics=data_loader.patronymics, roles=data_loader.roles)
        self.teams.append({
          "fio": "%s %s %s" % (participant.lastname, participant.name, participant.patronymic),
          "position": "%s/%s" % (participant.role, application.name_company),
          "contacts": "phone: %s, e-mail: %s" % (participant.phone, participant.email)
        })
    
    def generate_budget(current_random: random.SystemRandom):
      self.budget = [
        {
            "name": "Позиционирование",
            "value": "%s0000" % (str(current_random.randint(10, 100)))
        },
        {
            "name": "Уточнение деталей продуктового предложения",
            "value": "%s0000" % (str(current_random.randint(10, 100)))
        },
        {
            "name": "Подготовка к пилотному тестированию",
            "value": "%s0000" % (str(current_random.randint(10, 100)))
        },
        {
            "name": "Проведение пилотного тестирования",
            "value": "%s0000" % (str(current_random.randint(10, 100)))
        },
        {
            "name": "Формирование отчета о пилотном тестировании",
            "value": "%s0000" % (str(current_random.randint(10, 100)))
        }
      ]
    
    def generate_statuses(data_loader: DataLoader):
      self.statuses = [
          {
              "name": data_loader.__get_random_text_request__(sentences_count=1, paragraph_count=0),
              "expectations": data_loader.__get_random_text_request__(sentences_count=5, paragraph_count=0),
              "date": self.stages[0]["date"]
          },
          {
              "name": data_loader.__get_random_text_request__(sentences_count=1, paragraph_count=0),
              "expectations": data_loader.__get_random_text_request__(sentences_count=5, paragraph_count=0),
              "date": self.stages[1]["date"]
          },
          {
              "name": data_loader.__get_random_text_request__(sentences_count=1, paragraph_count=0),
              "expectations": data_loader.__get_random_text_request__(sentences_count=5, paragraph_count=0),
              "date": self.stages[2]["date"]
          },
          {
              "name": data_loader.__get_random_text_request__(sentences_count=1, paragraph_count=0),
              "expectations": data_loader.__get_random_text_request__(sentences_count=5, paragraph_count=0),
              "date": self.stages[3]["date"]
          },
          {
              "name": data_loader.__get_random_text_request__(sentences_count=1, paragraph_count=0),
              "expectations": data_loader.__get_random_text_request__(sentences_count=5, paragraph_count=0),
              "date": self.stages[4]["date"]
          }
      ]

    def generate_activities(current_random: random.SystemRandom):
      self.activities = [
        {
            "name": "Позиционирование",
            "expectations": current_random.choice(self.teams)["fio"],
            "date": self.stages[0]["date"]
        },
        {
            "name": "Уточнение деталей продуктового предложения",
            "expectations": current_random.choice(self.teams)["fio"],
            "date": self.stages[1]["date"]
        },
        {
            "name": "Подготовка к пилотному тестированию",
            "expectations": current_random.choice(self.teams)["fio"],
            "date": self.stages[2]["date"]
        },
        {
            "name": "Проведение пилотного тестирования",
            "expectations": current_random.choice(self.teams)["fio"],
            "date": self.stages[3]["date"]
        },
        {
            "name": "Формирование отчета о пилотном тестировании",
            "expectations": current_random.choice(self.teams)["fio"],
            "date": self.stages[4]["date"]
        }
      ]

    def generate_meetings(data_loader: DataLoader):
      self.meetings = [
          {
              "comment": data_loader.__get_random_text_request__(sentences_count=5, paragraph_count=0),
              "date": self.stages[0]["date"]
          },
          {
              "comment": data_loader.__get_random_text_request__(sentences_count=5, paragraph_count=0),
              "date": self.stages[1]["date"]
          },
          {
              "comment": data_loader.__get_random_text_request__(sentences_count=5, paragraph_count=0),
              "date": self.stages[2]["date"]
          },
          {
              "comment": data_loader.__get_random_text_request__(sentences_count=5, paragraph_count=0),
              "date": self.stages[3]["date"]
          },
          {
              "comment": data_loader.__get_random_text_request__(sentences_count=5, paragraph_count=0),
              "date": self.stages[4]["date"]
          }
      ]

    def generate_materials():
      self.materials = [
          {
              "name": "Презентация",
              "link": application.link_presentation
          }
      ]


    #
    generate_team(data_loader=data_loader, count_participants=current_random.randint(4, 12), current_random=current_random)
    self.name = generate_name(adjectives=data_loader.adjectives, nouns=data_loader.nouns, current_random=current_random)
    self.transportComplexOrganization = application.interest_transport_organization
    self.pilotMember = current_random.choice(["да", "нет"])
    self.leader = current_random.choice(self.teams)["fio"]
    self.pilotCoordinator = application.fullname_applicant
    self.transportComplexCoordinator = current_random.choice(transport_complex_coordinators)
    self.shortDescription = application.short_description
    self.timing = generate_timing(num_month=current_random.randint(1, 12), num_year=current_random.randint(2022, 2025))
    self.context = data_loader.__get_random_text_request__(sentences_count=5, paragraph_count=0)
    self.tags = application.tags
    self.order = {
        "teamName": application.fullname_participant,
        "stage": application.stage_product,
        "shortDescription": application.short_description,
        "cases": application.cases_product_using,
        "benefit": application.product_use,
        "transportOrganization": application.interest_transport_organization,
        "pilotVision": application.request_to_accelerate,
        "sertification": application.certification,
        "contactFio": application.fullname_applicant,
        "contactPosition": application.role_applicant,
        "phone": application.phone_applicant,
        "email": application.email_applicant,
        "legalName":application.name_company,
        "inn": application.inn_company,
        "peopleCount": application.num_people_company,
        "site": application.site_company,
        "acceleratorInfo": application.source_info_accelerator,
        "presentation": application.link_presentation
    }
    generate_effects(current_random.randint(3, 6))
    generate_stages()
    generate_budget(current_random=current_random)
    generate_statuses(data_loader=data_loader)
    generate_activities(current_random=current_random)
    generate_meetings(data_loader=data_loader)
    generate_materials()
    
    #
    self.product_passport_data = {
      "name": self.name,
      "transportComplexOrganization": self.transportComplexOrganization,
      "pilotMember": self.pilotMember,
      "leader": self.leader,
      "pilotCoordinator": self.pilotCoordinator,
      "transportComplexCoordinator": self.transportComplexCoordinator,
      "shortDescription": self.shortDescription,
      "timing": self.timing,
      "context": self.context,
      "pathName": self.pathName,
      "tags": self.tags,
      "order": self.order,
      "effects": self.effects,
      "stages": self.stages,
      "teams": self.teams,
      "budget": self.budget,
      "statuses": self.statuses,
      "activities": self.activities,
      "meetings": self.meetings,
      "materials": self.materials
    }
