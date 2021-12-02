from forming.ApplicationsFormer import ApplicationsFormer
from generating.ProductPassportGenerator import ProductPassportGenerator
from generating.TransportComplexCoordinatorsGenerator import TransportComplexCoordinatorsGenerator
from loading.DataLoader import DataLoader


class ProductsPassportFormer:
  """Класс создания данных по заявкам
  """

  def __init__(self):
    self.products_passport_data = []

  def __form__(self, applications_former: ApplicationsFormer, data_loader: DataLoader):
    """Метод генерации паспорта продуктов

    Args:
        applications_former (ApplicationsFormer): [список сгенерированных заявок]
    """
    #
    transport_complex_coordinators = TransportComplexCoordinatorsGenerator()
    transport_complex_coordinators.__generate_fullnames__(data_loader=data_loader)

    for num_application in range(len(applications_former.applications_data)):
      current_application = applications_former.applications_data[num_application]
      #
      product_passport_generator = ProductPassportGenerator()
      product_passport_generator.__generate__(
        application=current_application,
        data_loader=data_loader,
        transport_complex_coordinators=transport_complex_coordinators.fullnames
      )
      self.products_passport_data.append(product_passport_generator.product_passport_data)
