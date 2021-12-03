import {Component, OnInit} from '@angular/core';
import {FormFieldBase} from "../../components/dynamic-forms/entities/_field-base";
import {FormGroup} from "@angular/forms";
import {TextareaField} from "../../components/dynamic-forms/entities/textarea";
import {TextboxField} from "../../components/dynamic-forms/entities/textbox";

@Component({
  selector: 'app-request-for-innovation',
  templateUrl: './request-for-innovation.component.html',
  styleUrls: ['./request-for-innovation.component.sass']
})
export class RequestForInnovationComponent implements OnInit {
  formFields: FormFieldBase<any>[] = [
    new TextareaField({
      label: 'Опишите проблему',
      key: "1",
      placeholder: "",
      subLabel: `Что болит? Опишите своими словами, возможно, в форме задачи`,
      required: true,
      wide: true,
    }),
    new TextareaField({
      label: 'Как проявляется проблема?',
      key: "2",
      placeholder: "",
      subLabel: 'Приведите описание реальной проблемной ситуации',
      required: true,
      wide: true,
    }),
    new TextareaField({
      label: 'Каковы последствия проблемы?',
      key: "3",
      placeholder: "",
      subLabel: 'Опишите нежелательные последствия нерешенной проблемы',
      required: true,
      wide: true,
    }),
    new TextareaField({
      label: 'Почему возникла проблема?',
      key: "4",
      placeholder: "",
      subLabel: `Опишите ключевые причины возникновения проблемы`,
      required: true,
      wide: true,
    }),
    new TextareaField({
      label: 'Кого затрагивает проблема?',
      key: "5",
      placeholder: "",
      subLabel: 'У кого болит? Кто отвечает за проблемный участок',
      required: true,
      wide: true,
    }),
    new TextareaField({
      label: 'Когда желательно решить проблему?',
      key: "6",
      placeholder: "",
      required: true,
      wide: true,
    }),
    new TextareaField({
      label: 'Как пытались решить проблему?',
      key: "7",
      placeholder: "",
      subLabel: `Опишите предпринятые попытки решения и причины их неудачи`,
      required: true,
      wide: true,
    }),
    new TextareaField({
      label: 'Как с вами связаться?',
      key: "8",
      placeholder: "",
      subLabel: 'Укажите наименование вашего предприятия, ваши ФИО, Email и телефон',
      required: true,
      wide: true,
    }),
    // new TextboxField({
    //   label: 'Email',
    //   required: true,
    //   subLabel: `Контактные данные необходимы что бы мы могли прислать вам сообщение о проектах подходящих под вашу заявку`
    // }),
    // new TextboxField({
    //   label: 'Фамилия',
    //   required: true,
    // }),
    // new TextboxField({
    //   label: 'Имя',
    //   required: true,
    // }),
  ];
  form?: FormGroup;

  constructor() {
  }

  ngOnInit(): void {
  }

  /**
   * СОбытие изменения формы
   * @param $event
   * @param name
   */
  onFormChanged(form: FormGroup, name: string) {
    this.form = form;
  }

  sendRequestForInnovation() {
    console.log(this.form?.getRawValue());
  }

  isValidForm(): boolean {
    return this.form?.valid == undefined ? true : this.form?.valid;
  }
}
