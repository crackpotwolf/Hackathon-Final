from app.loading.DocumentLoader import DataLoader


class DataGenerator:
  """Класс генерации данных
  """
  def __init__(self):
    self.product_passport_data = {}

  def __generate__(self, data_loader: DataLoader):
    self.product_passport_data = {
      "name": data_loader.data_from_form["passport"][0].value,
      "transportComplexOrganization": data_loader.data_from_form["passport"][1].value,
      "pilotMember": data_loader.data_from_form["passport"][2].value,
      "leader": data_loader.data_from_form["passport"][3].value,
      "pilotCoordinator": data_loader.data_from_form["passport"][4].value,
      "transportComplexCoordinator": data_loader.data_from_form["passport"][5].value,
      "shortDescription": data_loader.data_from_form["passport"][6].value,
      "timing": data_loader.data_from_form["passport"][7].value,
      "context": data_loader.data_from_form["context"],
      "pathName": "",
      "tags": "",
      "order": data_loader.data_from_form,
      "effects": [{"name": row[1], "value": row[2]} for row in data_loader.data_from_form["effects"]],
      "stages": [{"name": obj.key(), "date": obj.value()} for obj in data_loader.data_from_form["stages"]],
      "teams": [{"fio": row[1], "position": row[2], "contacts": row[3]} for row in data_loader.data_from_form["teams"]],
      "budget": [{"name": row[1], "value": row[2]} for row in data_loader.data_from_form["budget"]],
      "statuses": [{"name": row[0], "expectations": row[1], "date": row[2]} for row in data_loader.data_from_form["statuses"]],
      "activities": [{"name": row[1], "expectations": row[2], "date": row[3]} for row in data_loader.data_from_form["activities"]],
      "meetings": [{"comment": row[1], "date": row[2]} for row in data_loader.data_from_form["meetings"]],
      "materials": [{"name": row[0], "link": row[1]} for row in data_loader.data_from_form["materials"]],
    }
