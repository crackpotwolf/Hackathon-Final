# -*- coding: utf-8 -*-
from app import app

type_ip = app.config['KEY_DB_SERVICE']
db_service_ip = app.config["URL_DB_SERVICE"]

print('Start adapter from %s for loading and posting documents to database (DB SERVICE IP=%s)' % (type_ip, db_service_ip))
app.logger.info('Start adapter from %s for loading and posting documents to database (DB SERVICE IP=%s)' % (type_ip, db_service_ip))
