export enum ProjectServiceEventType {
  /**
   * Загрузкапроектов завершена
   */
  LoadingComplete,
  /**
   * Выполнить глобальный поиск (по всем, индексируемым, параметрам) проектов
   */
  RunninSearchByGlobal,
  /**
   * Глобальный поиск проектов выполнен
   */
  RunninSearchByGlobalDone,
  /**
   * Глобальный поиск проектов завершился ошибкой
   */
  RunninSearchByGlobalError,

  /**
   * Получение текущих/отображаемых проектов
   */
  GetCurrentProjects,

  /**
   * Выгрузка текущих проектов
   */
  CurrentProjects,

  /**
   * Применение фильтро
   */
  ApplyFilters,
  /**
   * Сброс фильтро
   */
  ClearFilters,

  /**
   * Добавление/исключение из избранного
   */
  onClickFavorites,

  /**
   * Заявка на инновацию отправлена
   */
  SendRequestInvest,

  /**
   * Строка поиска очищена
   */
  ClearSearch

}
