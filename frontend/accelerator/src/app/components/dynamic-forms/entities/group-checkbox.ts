import {FormFieldBase} from "./_field-base";
import {FormBuilder, FormGroup} from "@angular/forms";

export class GroupCheckboxField extends FormFieldBase<FormGroup> {
  controlType = 'group-checkbox';

  constructor(options: any) {
    super(options);

    let _value: { [index: string]: any } = {};
    for (const option of options.options) {
      _value[option.key.toString()] = false;
    }
    this.value = new FormBuilder().group(_value);
  }
}
