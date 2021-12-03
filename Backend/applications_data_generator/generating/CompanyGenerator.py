from pymorphy2 import MorphAnalyzer
from string import punctuation
from transliterate import translit

import random


class CompanyGenerator:
  """[summary]
  """

  def __init__(self) -> None:
    self.name = ""
    self.inn = ""
    self.people = ""
    self.website = ""

  def __generate__(self, adjectives, nouns, people_count):
    
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

    def random_people(people_count, current_random: random.SystemRandom):
      return current_random.choice(people_count)
    
    def generate_inn(current_random: random.SystemRandom):
      part_1 = current_random.randint(10, 99)
      part_2 = current_random.randint(10, 99)
      part_3 = current_random.randint(10, 99)
      part_4 = current_random.randint(10, 99)
      part_5 = current_random.randint(10, 99)
      return "%s%s%s%s%s" % (str(part_1), str(part_2), str(part_3), str(part_4), str(part_5))
    
    def generate_website(name: str, current_random: random.SystemRandom):
      #
      website_name = translit(name, language_code="ru", reversed=True).replace(punctuation, " ").replace(" ", "").lower()
      #
      domen = current_random.choice(["com", "io", "ru"])
      return "%s.%s" % (website_name, domen)
    
    current_random = random.SystemRandom()
    
    #
    self.name = generate_name(adjectives=adjectives, nouns=nouns, current_random=current_random)
    self.website = generate_website(name=self.name, current_random=current_random)
    self.people = random_people(people_count=people_count, current_random=current_random)
    self.inn = generate_inn(current_random=current_random)
