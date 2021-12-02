import os

# путь до папки с данными
PATH_TO_DATA = os.path.join(os.getcwd(), "data")
# путь до папок с:
# - входными данными
PATH_TO_INPUT_DATA = os.path.join(PATH_TO_DATA, "input_data")
# - промежуточными данными
PATH_TO_MIDDLE_DATA = os.path.join(PATH_TO_DATA, "middle_data")
# - результирующими данными
PATH_TO_RESULT_DATA = os.path.join(PATH_TO_DATA, "result_data")
