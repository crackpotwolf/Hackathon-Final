import {FormFieldBase} from "./_field-base";
import * as moment from "moment";
import {Moment} from "moment/moment";

export class DateTimePickerFiled extends FormFieldBase<Date | Moment> {
  controlType = 'datetimepicker';
}
