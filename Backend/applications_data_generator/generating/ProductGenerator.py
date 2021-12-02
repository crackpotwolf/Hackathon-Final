from loading.DataLoader import DataLoader
from pymorphy2 import MorphAnalyzer
import string
from transliterate import translit
from typing import Dict, List

import random


class ProductGenerator:
  """Класс генерации продукта
  """

  def __init__(self):
    """Метод генерации объекта класса
    """
    self.fullname_participant = ""
    self.stage = ""
    self.short_description = ""
    self.cases_product_using = ""
    self.product_use = ""
    self.interest_transport_organization = ""
    self.request_to_accelerate = ""
    self.certification = ""
    self.source_info_accelerator = ""
    self.link_presentation = ""
    self.tags = ""
    
  def __generate__(self, data_loader: DataLoader):
    
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
    
    def generate_presentation_link():
      name_link = translit(value=self.fullname_participant, language_code="ru", reversed=True).replace(string.punctuation, " ").replace(" ", "_")
      link = "https://docs.google.com/presentation/d/%s/edit?usp=sharing" % name_link
      return link
    
    def random_tags(fields_subfields: Dict, current_random: random.SystemRandom):
      field = current_random.choice(list(fields_subfields.keys()))
      subfield = current_random.choice(fields_subfields[field])
      return [field, subfield]
    
    #
    current_random = random.SystemRandom()
    #
    self.fullname_participant = generate_name(adjectives=data_loader.adjectives, nouns=data_loader.nouns, current_random=current_random)
    #
    self.stage = current_random.choice(data_loader.product_stages)
    #
    self.short_description = current_random.choice(data_loader.product_short_description)
    #
    self.cases_product_using = current_random.choice(data_loader.product_cases)
    #
    self.product_use = current_random.choice(data_loader.product_use)
    #
    self.interest_transport_organization = current_random.choice(data_loader.interest_transport_organization)
    #
    self.request_to_accelerate = current_random.choice(data_loader.pilot_product)
    #
    self.certification = current_random.choice(data_loader.certification)
    #
    self.source_info_accelerator = current_random.choice(data_loader.source_info_accelerator)
    #
    self.link_presentation = generate_presentation_link()
    #
    self.tags = random_tags(fields_subfields=data_loader.fields_and_subfields, current_random=current_random)
