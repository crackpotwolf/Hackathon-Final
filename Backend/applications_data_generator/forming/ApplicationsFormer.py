from forming.ApplicationDocxFormer import ApplicationDocxFormer
from generating.ApplicationGenerator import ApplicationGenerator
from loading.DataLoader import DataLoader




class ApplicationsFormer:
  """Класс создания данных по заявкам
  """
  def __init__(self):
    self.applications_data = []
  
  def __form__(self, applications_number: int, data_loader: DataLoader):
    """Метод генерации заявок

    Args:
        applications_number (int): [количество заявок для формирования]
    """
    #
    for num_application in range(0, applications_number):
      application_generator = ApplicationGenerator()
      application_generator.__generate__(data_loader=data_loader)
      self.applications_data.append(application_generator)
      
      #
      application_docx_former = ApplicationDocxFormer()
      application_docx_former.__form_document__(
        application_generator_num=num_application, 
        application_generator=application_generator
      )
