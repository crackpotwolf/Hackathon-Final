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
      label: 'Что болит?',
      key: "1",
      placeholder: "",
      subLabel: `Опишите своими словами существующую в организации проблему.
      Можно сформулировать проблему в форму задачи`,
      required: true,
      wide: true,
    }),
    new TextareaField({
      label: 'Как проявляется ваша проблема?',
      key: "2",
      placeholder: "",
      subLabel: 'Приведите описание реальной ситуации, в которой проблема бы проявилась',
      required: true,
      wide: true,
    }),
    new TextareaField({
      label: 'Что будет если проблему не решать?',
      key: "3",
      placeholder: "",
      subLabel: 'Опишите нежелательные эффекты, которые возникают или могут возникнуть из-за того, что проблема не решается',
      required: true,
      wide: true,
    }),
    new TextareaField({
      label: 'Почему так происходит?',
      key: "4",
      placeholder: "",
      subLabel: `Какие на ваш взгляд ключевые причины возникновения проблемы?
       Что на ваш взгляд является причиной возникновения проблемы?`,
      required: true,
      wide: true,
    }),
    new TextareaField({
      label: 'У кого болит?',
      key: "5",
      placeholder: "",
      subLabel: 'Кто является непосредственно ответственным за проблемный участок?',
      required: true,
      wide: true,
    }),
    new TextareaField({
      label: 'Какой желательный срок решения проблемы?',
      key: "6",
      placeholder: "",
      required: true,
      wide: true,
    }),
    new TextareaField({
      label: 'Пробовали решать?',
      key: "7",
      placeholder: "",
      subLabel: `Как пытались решить проблему ранее?
      Почему эти попытки оказались неудачными или почему были признаны неудачными?
      Чем не устроили найденные решения?
      Общались ли с рынком?
      Если да, то с кем?`,
      required: true,
      wide: true,
    }),
    new TextareaField({
      label: 'Как с вами связаться?',
      key: "8",
      placeholder: "",
      subLabel: 'Укажите наименование вашего предприятия, ваши ФИО, и телефон для связи',
      required: true,
      wide: true,
    }),
    new TextboxField({
      label: 'Email',
      required: true,
      subLabel: `Контактные данные необходимы что бы мы могли прислать вам сообщение о проектах подходящих под вашу заявку`
    }),
    new TextboxField({
      label: 'Фамилия',
      required: true,
    }),
    new TextboxField({
      label: 'Имя',
      required: true,
    }),
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
