export interface Order {
  /// Наименование команды/организации
  teamName: string;
  /// Стадия готовности продукта
  stage: string;
  /// Краткое описание продукта
  shortDescription: string;
  /// Кейсы использования продукта
  cases: string;
  /// Польза продукта
  benefit: string;
  /// Организация Московского транспорта, интересная в первую очередь
  transportOrganization: string;
  /// Запрос к акселератору и видение пилотного проекта
  pilotVision: string;
  /// Требуется ли сертификация продукта
  sertification: string;
  /// ФИО контактного лица по заявке
  contactFio: string;
  /// Должность контактного лица
  contactPosition: string;
  /// Контактный телефон
  phone: string;
  /// Контактная почта
  email: string;
  /// Наименование юридического лица
  legalName: string;
  /// ИНН юридического лица
  inn: string;
  /// Сколько человек в организации
  peopleCount: string;
  /// Сайт
  site: string;
  /// Откуда узнали про акселератор
  acceleratorInfo: string;
  /// Ссылка на презентацию
  presentation: string;
}
