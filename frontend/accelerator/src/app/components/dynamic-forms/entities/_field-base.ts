import {placeholdersToParams} from "@angular/compiler/src/render3/view/i18n/util";

export class FormFieldBase<T> {

  ///// ----------------------------------------------------------
  ///// ---------------------    ОБЩЕЕ   -------------------------
  ///// ----------------------------------------------------------
  /**
   * ???
   */
  value: T | undefined;
  min: T | undefined;
  max: T | undefined;
  /**
   * Идентификатор поля - ключ в результирующем JSON
   */
  key: string;

  /**
   * Идентификатор поля - ключ в исходной сущности
   */
  keyProperty: string;

  /**
   * Название поля - будет размещено над полем ввода
   */
  label: string;
  /**
   * Двоеточие после label
   */
  colon: boolean;
  /**
   * Описание поля - будет размещено под полем ввода
   */
  subLabel: string;
  /**
   * Ялвяется ли полей обязательным
   */
  required: boolean;
  /**
   *
   */
  // order: number;
  /**
   * Тип поля - тестовое | выпадающий спсок | календарь и т.д.
   */
  controlType: string;


  ///// ----------------------------------------------------------
  ///// -------------------   ТЕКСТОВОЕ ПОЛЕ  --------------------
  ///// ----------------------------------------------------------

  /**
   * Тип данных в текстовом поле
   */
  type: string;
  /**
   * Параметры для выпадающего списка
   */

    ///// ----------------------------------------------------------
    ///// ----------------   ВЫПАДАЮЩИЙ СПИСОК  --------------------
    ///// ----------------------------------------------------------

  options: { key: string, value: string }[];
  /**
   * Выводит текст внутри поля формы, который исчезает при получении фокуса.
   */
  placeholder: string;

  /**
   * Скрыть поле
   */
  hide: boolean;

  /**
   *  Маска для поля ввода
   */
  mask: string;

  /**
   * Широкий - на всю ширину
   */
  wide: boolean;

  /**
   * Половина - ширина объекта будет равна полови элемета
   */
  half: boolean;

  /**
   * Лейбел у CheckBox, если активирован
   */
  onLabel: string;

  /**
   * Лейбел у CheckBox, если деактивирован
   */
  offLabel: string;

  /**
   * Функция обработки изменения поля, которая может
   * вернуть новое значение другого элемента формы.
   * Например: форма из 2х полей [firstName, lastName]
   * При изменнеии firstName вызывается обработчик, и он может вернуть список значений:
   * {"lastName": "Новое значение !"}
   * Для изменения воторого поля lastName
   */
  onChange: any;


  constructor(options: {
    value?: T;
    min?: T;
    max?: T;
    key?: string;
    label?: string;
    subLabel?: string;
    required?: boolean;
    // order?: number;
    controlType?: string;
    type?: string;
    options?: { key: string, value: string }[];
    placeholder?: string;
    hide?: boolean;
    mask?: string,
    wide?: boolean,
    keyProperty?: string,
    half?: boolean,
    onLabel?: string,
    offLabel?: string,
    onChange?: (currentValue: any) => void,
    convertLineBreaks?: boolean,
    colon?: boolean,
  } = {}) {
    this.value = options.value || undefined;
    this.min = options.min || undefined;
    this.max = options.max || undefined;
    this.key = options.key || '';
    this.label = options.label || '';
    this.required = !!options.required;
    // this.order = options.order === undefined ? 1 : options.order;
    this.controlType = options.controlType || '';
    this.type = options.type || '';
    this.options = options.options || [];
    this.placeholder = options.placeholder || '';
    this.hide = options.hide || false;
    this.mask = options.mask || '';
    this.wide = options.wide || false;
    this.keyProperty = options.keyProperty || "";
    this.half = options.half || false;
    this.onLabel = options.onLabel || '';
    this.offLabel = options.offLabel || '';
    this.onChange = options.onChange;
    this.colon = options.colon;


    if (options.convertLineBreaks == undefined || options.convertLineBreaks) {
      this.subLabel = options.subLabel?.replace(/\n/g, '<br/>') || '';
    } else {
      this.subLabel = options.subLabel || '';
    }
  }
}
