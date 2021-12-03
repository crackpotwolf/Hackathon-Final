from flask import Flask
import logging
from logging.handlers import RotatingFileHandler
from config import ProductionConfig, DevelopConfig, TestConfig, LocalConfig

import os


app = Flask(__name__)

ENVIRONMENT = os.environ.get("ENVIRONMENT")
if ENVIRONMENT == 'Production':
    app.config.from_object(ProductionConfig)
elif ENVIRONMENT == 'Development':
    app.config.from_object(DevelopConfig)
elif ENVIRONMENT == 'Test':
    app.config.from_object(TestConfig)
else:
    app.config.from_object(LocalConfig)

log_app_data_loader_handler = RotatingFileHandler(
    app.config['LOGS_APP_DATA_LOADER'], mode='a', maxBytes=5*1024*1024, backupCount=2, encoding=None, delay=0)
logging.basicConfig(level=logging.INFO, format='%(asctime)s %(levelname)s %(name)s %(threadName)s : %(message)s', handlers={log_app_data_loader_handler})

# регистрация модуля api
from app.routes import api
app.register_blueprint(api, url_prefix='/api')

from app import routes
app.run(host='0.0.0.0', port=8008)
