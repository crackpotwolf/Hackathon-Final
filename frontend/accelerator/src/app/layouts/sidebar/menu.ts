import {MenuItem} from './menu.model';

export const MENU: MenuItem[] = [
  {
    id: 1,
    label: 'Сотрудники',
    isTitle: true
  },
  {
    id: 1001,
    label: 'Сотрудники',
    icon: 'fas fa-users',
    link: '/employees'
  },
  {
    id: 2,
    label: 'Справочники',
    isTitle: true
  },
  {
    id: 2003,
    label: 'Участки',
    icon: 'fas fa-map-marker-alt',
    link: '/places'
  },
  {
    id: 2004,
    label: 'Статусы',
    icon: 'far fa-check-circle',
    link: '/statuses'
  },
  {
    id: 2005,
    label: 'Специальности',
    icon: 'fas fa-user-tie',
    link: '/specialties'
  }
  ,
  {
    id: 2006,
    label: 'Типы файлов',
    icon: 'far fa-file-alt',
    link: '/file-types'
  },
  {
    id: 2007,
    label: 'Отчеты',
    icon: 'far fa-clipboard',
    subItems: [
      {
        id: 20078,
        label: 'Отправка на работу',
        link: '/reports/departure-orders',
        parentId: 2007
      },
      {
        id: 20079,
        label: 'Трудовой договор',
        link: '/reports/labor-contracts',
        parentId: 2007
      },
      {
        id: 200710,
        label: 'Прекращение трудового договора',
        link: '/reports/order-dismissals',
        parentId: 2007
      },
      {
        id: 200711,
        label: 'Приказ о ВРИО',
        link: '/reports/order-vrios',
        parentId: 2007
      },
      {
        id: 200712,
        label: 'Командировочное удостоверение',
        link: '/reports/travel-certificates',
        parentId: 2007
      }
    ]
  },
  {
    id: 3,
    label: 'Управление',
    isTitle: true,
  },
  {
    id: 3001,
    label: 'Уведомления',
    icon: 'fas fa-users',
    link: '/notifications',
  },
  {
    id: 3002,
    label: 'Пользователи',
    icon: 'fas fa-users',
    link: '/users',
    role: 'Manager'
  },
];
