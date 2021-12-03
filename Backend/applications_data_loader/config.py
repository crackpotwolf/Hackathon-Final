import os
basedir = os.path.abspath(os.path.dirname(__file__))

# определение IP-адреса сервиса БД УПЭ
dict_keyword_ip_DB_SERVICE = {
    "PROD": "http://51.250.1.24:5000",
    "DEV": "http://51.250.1.24:5000/",
    "TEST": "http://51.250.1.24:5000/"
}

#
log_filename = "app_data_loader.log"
path_to_save = os.path.join(os.getcwd(), "app")
# C:\Projects\C#\hackaton_2_12_21\Hackathon-Final\Backend\applications_data_loader\app\logs
path_to_save_logs = os.path.join(path_to_save, "logs")


class BaseConfig(object):
    DEBUG = False


class ProductionConfig(BaseConfig):
    DEBUG = False
    LOGS_APP_DATA_LOADER = log_filename
    PATH_TO_SAVE = path_to_save
    KEY_DB_SERVICE = "PROD"
    URL_DB_SERVICE = dict_keyword_ip_DB_SERVICE[KEY_DB_SERVICE]


class DevelopConfig(BaseConfig):
    DEBUG = False
    LOGS_APP_DATA_LOADER = log_filename
    PATH_TO_SAVE = path_to_save
    KEY_DB_SERVICE = "DEV"
    URL_DB_SERVICE = dict_keyword_ip_DB_SERVICE[KEY_DB_SERVICE]


class TestConfig(BaseConfig):
    DEBUG = False
    LOGS_APP_DATA_LOADER = log_filename
    PATH_TO_SAVE = path_to_save
    KEY_DB_SERVICE = "TEST"
    URL_DB_SERVICE = dict_keyword_ip_DB_SERVICE[KEY_DB_SERVICE]


class LocalConfig(BaseConfig):
    DEBUG = True
    LOGS_APP_DATA_LOADER = log_filename
    PATH_TO_SAVE = path_to_save
    KEY_DB_SERVICE = "DEV"
    URL_DB_SERVICE = dict_keyword_ip_DB_SERVICE[KEY_DB_SERVICE]
