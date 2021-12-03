from app import app
from flask import Blueprint, request, jsonify
from app.loading.DocumentLoader import DataLoader
from app.generating.DataGenerator import DataGenerator

import requests


# регистрация модуля api
api = Blueprint('api', __name__, url_prefix='/api')

@api.route('/', methods=['DELETE', 'GET', 'POST', 'PUT'])
def return_message_error():
    print("Wrong URL")
    app.logger.info("Wrong URL")
    return jsonify({'status': 'Failed', 'info': 'Wrong URL'}), 404

#
@api.route('/docs_processor', methods=['DELETE', 'GET', 'POST', 'PUT'])
def process_docs():
    """
    API обёртка над функцией загрузки данных из документов в БД
    """
    if request.method == "POST":
        
        message_info = "Process of posting data from docs is failed"
        
        try:
            # 
            uploaded_docs = request.files.getlist("file[]")
            
            data_loader = DataLoader()
            data_loader.__load__(files=uploaded_docs)
            
            data_generator = DataGenerator()
            data_generator.__generate__(data_loader=data_loader)
            
            # отправка данных
            r = requests.post(url=app.config["URL_DB_SERVICE"] + "/api/accelerator/v1/project/project-creating")
            
            if r.status_code == 200:
            
                message_info = "Processes of loading and posting documents to database completed successfully"
                print(message_info)
                app.logger.info(message_info)
                return jsonify({'status': 'Success', 'info': message_info}), 200
            else:
                app.logger.info(message_info)
                return jsonify({'status': 'Failed', 'info': message_info}), 500

        except Exception as ex:
            exception_text = ex.args[0]
            if "404 Not Found" in exception_text:
                app.logger.info(message_info)
                return jsonify({'status': 'Failed', 'info': "Process of posting data to database is failed"}), 404
            else:
                app.logger.info(message_info)
                return jsonify({'status': 'Failed', 'info': message_info}), 500
        
    else:
        # остальные методы не поддерживаются
        print(f'Method {request.method} not supported')
        app.logger.info(f'Method {request.method} not supported')
        return jsonify({'status': 'Failed', 'info': f'Method {request.method} not supported'}), 404
