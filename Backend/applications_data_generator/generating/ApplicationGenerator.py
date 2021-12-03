from generating.ApplicantGenerator import ApplicantGenerator
from generating.CompanyGenerator import CompanyGenerator
from generating.ProductGenerator import ProductGenerator
from loading.DataLoader import DataLoader


class ApplicationGenerator:
  """Класс генерации Заявки
  """

  def __init__(self):
    """Метод генерации объекта класса
    """
    
    # Project-data
    self.fullname_participant = ""
    self.stage_product = ""
    self.short_description = ""
    self.cases_product_using = ""
    self.product_use = ""
    self.interest_transport_organization = ""
    self.request_to_accelerate = ""
    self.certification = ""
    self.source_info_accelerator = ""
    self.link_presentation = ""
    self.tags = ""
    
    # Applicant-data
    self.fullname_applicant = ""
    self.role_applicant = ""
    self.phone_applicant = ""
    self.email_applicant = ""
    
    # Company-data
    self.name_company = ""
    self.inn_company = ""
    self.num_people_company = ""
    self.site_company = ""
    
    #
    # self.application_data = {}
  
  def __generate__(self, data_loader: DataLoader):
    # generate Applicant-obj
    applicantGenerator = ApplicantGenerator()
    applicantGenerator.__generate__(
      names=data_loader.names,
      lastnames=data_loader.lastnames,
      patronymics=data_loader.patronymics,
      roles=data_loader.roles
    )
    # generate Company-obj
    companyGenerator = CompanyGenerator()
    companyGenerator.__generate__(
      adjectives=data_loader.adjectives,
      nouns=data_loader.nouns,
      people_count=data_loader.people_company_number
    )
    
    # generate Project-obj
    productGenerator = ProductGenerator()
    productGenerator.__generate__(data_loader=data_loader)
    
    # Applicant generated data 
    self.fullname_participant = "%s %s %s" % (applicantGenerator.lastname, applicantGenerator.name, applicantGenerator.patronymic)
    self.role_applicant = applicantGenerator.role
    self.phone_applicant = applicantGenerator.phone
    self.email_applicant = applicantGenerator.email
    # Company generated data
    self.name_company = companyGenerator.name
    self.inn_company = companyGenerator.inn
    self.num_people_company = companyGenerator.people
    self.site_company = companyGenerator.website
    # Project generated data
    self.fullname_participant = productGenerator.fullname_participant
    self.stage_product = productGenerator.stage
    self.short_description = productGenerator.short_description
    self.cases_product_using = productGenerator.cases_product_using
    self.product_use = productGenerator.product_use
    self.interest_transport_organization = productGenerator.interest_transport_organization
    self.request_to_accelerate = productGenerator.request_to_accelerate
    self.certification = productGenerator.certification
    self.source_info_accelerator = productGenerator.source_info_accelerator
    self.link_presentation = productGenerator.link_presentation
    self.tags = productGenerator.tags
    #
    # self.application_data = {
    #   "fullname_participant": self.fullname_participant,
    #   "role_applicant": self.role_applicant,
    #   "phone_applicant": self.phone_applicant,
    #   "email_applicant": self.email_applicant,
    #   "name_company": self.name_company,
    #   "inn_company": self.inn_company,
    #   "num_people_company": self.num_people_company,
    #   "site_company": self.site_company,
    #   "fullname_participant": self.fullname_participant,
    #   "stage_product": self.stage_product,
    #   "short_description": self.short_description,
    #   "cases_product_using": self.cases_product_using,
    #   "product_use": self.product_use,
    #   "interest_transport_organization": self.interest_transport_organization,
    #   "request_to_accelerate": self.request_to_accelerate,
    #   "certification": self.certification,
    #   "source_info_accelerator": self.source_info_accelerator,
    #   "link_presentation": self.link_presentation,
    #   "tags": self.tags
    # }

