from config import *
from datetime import datetime
from io import BytesIO
from json_processing.json_processor import *
from typing import Dict, List, Tuple, Union
from urllib.request import urlopen
from zipfile import ZipFile

import requests


class DataLoader:
  """Класс для загрузки данных из интернета
  """
  def __init__(self) -> None:
    self.names = []
    self.lastnames = []
    self.patronymics = []
    
    self.nouns = []
    self.adjectives = []
    
    self.fields_and_subfields = []
    
    self.roles = []
    
    self.product_stages = []
    self.pilot_product = []
    self.product_cases = []
    self.product_use = []
    self.product_short_description = []
    self.certification = []
    self.source_info_accelerator = []
    self.interest_transport_organization = []
    
    self.technologies = []
  
  def __load__(self):
    
    def get_file_by_url(url):
      #
      r = requests.get(url=url)
    
    def get_data_by_url(main_url: str, data_type: str, result_type: str) -> List:
      '''
      Метод выгрузки данных из источника в интернете
      '''
      start_time = datetime.now()
      print("Start loading {0}: {1}".format(data_type, str(start_time)))

      r = requests.get(url=main_url)

      loading_status = r.status_code
      if result_type == "json":
        data = r.json() if loading_status == 200 else []
      elif result_type == "csv":
        if loading_status == 200:
          data = r.text
          # приведение к JSON-формату
          lines = data.split('\n')
          data = []
          headers = []

          for num_line, line in enumerate(lines):
            if num_line == 0:
              # определяем основные ключи словаря
              headers = line.split('\t')
            else:
              words = line.split('\t')
              if len(words) == len(headers):
                # добавляем преобразованный элемент русских слов
                item = {}
                for num_element, header in enumerate(headers):
                  item[header] = words[num_element]
              data.append(item)
        else:
          data = []
      elif result_type == "txt":
        if loading_status == 200:
          data = r.text
          # приведение к JSON-формату
          data = data.split('\n')
        else:
          data = []

      end_time = datetime.now()
      print("End loading {0} from DB: {1}".format(data_type, str(end_time)))
      process_time = end_time - start_time
      print("Loading time: {0}. Loading status {1}".format(
          str(process_time), str(r.status_code)))
      return data, loading_status
    
    
    def get_fullnames():
      self.names = read_from_json_in_dir(
        filename="names.json",
        dir="input_data"
      )
      self.lastnames = read_from_json_in_dir(
        filename="lastnames.json",
        dir="input_data"
      )
      self.patronymics = read_from_json_in_dir(
        filename="patronymics.json",
        dir="input_data"
      )
      
      if len(self.names) == 0 or len(self.lastnames) == 0 or len(self.patronymics):

        self.names = {}
        self.lastnames = {}
        self.patronymics = {}
        
        self.names["m"], m_names_loading_status = \
          get_data_by_url(
              main_url="https://raw.githubusercontent.com/linuxforse/random_russian_and_ukraine_name_surname/master/imena_m_ru.txt",
              data_type="men names",
              result_type="txt"
          )
        
        self.lastnames["m"], m_lastnames_loading_status = \
          get_data_by_url(
              main_url="https://raw.githubusercontent.com/linuxforse/random_russian_and_ukraine_name_surname/master/family_m_ru.txt",
              data_type="men lastnames",
              result_type="txt"
          )
          
        self.patronymics["m"], m_patronymics_loading_status = \
          get_data_by_url(
            main_url="https://raw.githubusercontent.com/linuxforse/random_russian_and_ukraine_name_surname/master/otch_m_ru.txt",
            data_type="men patronymics",
            result_type="txt"
        )
        
        self.names["w"], w_names_loading_status = \
          get_data_by_url(
              main_url="https://raw.githubusercontent.com/linuxforse/random_russian_and_ukraine_name_surname/master/imena_f_ru.txt",
              data_type="women names",
              result_type="txt"
          )
        
        self.lastnames["w"], w_lastnames_loading_status = \
          get_data_by_url(
              main_url="https://raw.githubusercontent.com/linuxforse/random_russian_and_ukraine_name_surname/master/family_f_ru.txt",
              data_type="women lastnames",
              result_type="txt"
          )
        
        self.patronymics["w"], m_patronymics_loading_status = \
            get_data_by_url(
            main_url="https://raw.githubusercontent.com/linuxforse/random_russian_and_ukraine_name_surname/master/otch_f_ru.txt",
            data_type="women patronymics",
            result_type="txt"
        )
        
        write_to_json_in_dir(
            filename="names.json",
            data=self.names,
            dir="input_data"
        )
        
        write_to_json_in_dir(
            filename="lastnames.json",
            data=self.lastnames,
            dir="input_data"
        )
        
        write_to_json_in_dir(
            filename="patronymics.json",
            data=self.patronymics,
            dir="input_data"
        )

      # return loading_status
    
    def get_nouns():
      self.nouns = read_from_json_in_dir(
          filename="nouns.json",
          dir="input_data"
      )
      if len(self.nouns) == 0:

        nouns, loading_status = \
          get_data_by_url(
              main_url="https://raw.githubusercontent.com/Badestrand/russian-dictionary/master/nouns.csv",
              data_type="russian nouns",
              result_type="csv"
          )

        self.nouns = [word["bare"] for word in nouns]
        
        write_to_json_in_dir(
          filename="nouns.json",
          data=self.nouns,
          dir="input_data"
        )
      # return loading_status
    
    def get_adjectives():
      self.adjectives = read_from_json_in_dir(
          filename="adjectives.json",
          dir="input_data"
      )
      if len(self.adjectives) == 0:

        adjectives, loading_status = \
            get_data_by_url(
                main_url="https://raw.githubusercontent.com/Badestrand/russian-dictionary/master/adjectives.csv",
                data_type="russian adjectives",
                result_type="csv"
            )
        
        self.adjectives = [word["bare"] for word in adjectives]

        write_to_json_in_dir(
          filename="adjectives.json",
          data=self.adjectives,
          dir="input_data"
        )

      # return loading_status

    def get_roles():
      self.roles = read_from_json_in_dir(
        filename="applicant_roles.json",
        dir="input_data"
      )
      if len(self.roles) == []:
        print("There is no data about {0}", "roles")
    
    def get_fields_and_subfields():
      self.fields_and_subfields = read_from_json_in_dir(
        filename="fields_subfields.json",
        dir="input_data"
      )
      if len(self.fields_and_subfields) == []:
        print("There is no data about {0}", "fields and subfields")

      self.project_sale_stages = read_from_json_in_dir(
        filename="project_sale_stages.json",
        dir="input_data"
      )
      if len(self.project_sale_stages) == []:
        print("There is no data about {0}", "project sale stages")
      
    def get_product_stages():
      self.product_stages = read_from_json_in_dir(
        filename="product_stages.json",
        dir="input_data"
      )
      if len(self.product_stages) == []:
        print("There is no data about {0}", "project stages")
      
    def get_technologies():
      self.technologies = read_from_json_in_dir(
        filename="technologies.json",
        dir="input_data"
      )
      if len(self.technologies) == []:
        print("There is no data about {0}", "technologies")

      data_loader_obj.get_names_and_lastnames()
    
    def get_pilot_product():
      self.pilot_product = read_from_json_in_dir(
          filename="pilot_product.json",
        dir="input_data"
      )
      if len(self.pilot_product) == []:
        print("There is no data about {0}", "pilot product")
    
    def get_product_cases():
      self.product_cases = read_from_json_in_dir(
          filename="product_cases.json",
          dir="input_data"
      )
      if len(self.product_cases) == []:
        print("There is no data about {0}", "product cases")
        
    def get_product_use():
      self.product_use = read_from_json_in_dir(
          filename="product_use.json",
          dir="input_data"
      )
      if len(self.product_use) == []:
        print("There is no data about {0}", "product use")
        
    def get_product_short_description():
      self.product_short_description = read_from_json_in_dir(
          filename="short_descriptions.json",
          dir="input_data"
      )
      if len(self.product_short_description) == []:
        print("There is no data about {0}", "short description")
        
    def get_source_info_accelerator():
      self.source_info_accelerator = read_from_json_in_dir(
          filename="source_info_accelerator.json",
          dir="input_data"
      )
      if len(self.source_info_accelerator) == []:
        print("There is no data about {0}", "source info accelerator")
        
    def get_certification():
      self.certification = read_from_json_in_dir(
          filename="certification.json",
          dir="input_data"
      )
      if len(self.certification) == []:
        print("There is no data about {0}", "certification")
        
    def get_interest_transport_organization():
      self.interest_transport_organization = read_from_json_in_dir(
          filename="interest_transport_organization.json",
          dir="input_data"
      )
      if len(self.interest_transport_organization) == []:
        print("There is no data about {0}", "interest transport organization")
    #
    get_fullnames()
    #
    get_nouns()
    #
    get_adjectives()
    #
    get_fields_and_subfields()
    #
    get_product_stages()
    #
    get_roles()
    #
    get_pilot_product()
    #
    get_product_cases()
    #
    get_product_use()
    #
    get_product_short_description()
    #
    get_source_info_accelerator()
    #
    get_certification()
    #
    get_interest_transport_organization()
    #
    # get_technologies()

  def __get_random_text_request__(self, sentences_count: int, paragraph_count: int):
    """Метод получения случайного текста в зависимости от

    Args:
        sentences_count (int): [количество предложений]
        paragraph_count (int): [количество абзацев]

    Returns:
        [str]: [полученный текст]
    """
    url = "https://fish-text.ru/get?"

    if sentences_count > 0:
      r = requests.get(
          url=url + "format=json&type=sentence&number=%s" % (str(sentences_count)))
    elif paragraph_count > 0:
      r = requests.get(
          url=url + "format=json&type=paragraph&number=%s" % (str(paragraph_count)))

    r.encoding = "utf-8"
    text = r.json()["text"]
    return text
