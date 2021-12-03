from forming.ApplicationsFormer import ApplicationsFormer
from forming.ProductsPassportFormer import ProductsPassportFormer
from json_processing.json_processor import write_to_json_in_dir
from loading.DataLoader import DataLoader


if __name__=="__main__":
  #
  data_loader = DataLoader()
  data_loader.__load__()
  #
  applications_number = 30
  #
  applications_former = ApplicationsFormer()
  applications_former.__form__(applications_number=applications_number, data_loader=data_loader)
  #
  products_passport_former = ProductsPassportFormer()
  products_passport_former.__form__(applications_former=applications_former, data_loader=data_loader)
  #
  write_to_json_in_dir(
    filename="projects.json",
    data=products_passport_former.products_passport_data,
    dir="middle_data"
  )