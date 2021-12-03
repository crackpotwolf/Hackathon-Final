/// Проект
import {Order} from "./Order";
import {Effects} from "./Effects";
import {Stage} from "./Stage";
import {Team} from "./Team";
import {Budget} from "./Budget";
import {Status} from "./Status";
import {Activities} from "./Activities";
import {Meeting} from "./Meeting";
import {Material} from "./Material";

export interface Project {
  guid: string;
  // Наименование проекта
  name: string;
  // Организация транспортного комплекса Москвы
  transportComplexOrganization: string;
  // Участник программы пилотирования «Транспортные инновации Москвы»
  pilotMember: string;
  // Руководитель проекта
  leader: string;
  // Координатор от участника программы пилотирования «Транспортные инновации Москвы»
  pilotCoordinator: string;
  // Координатор от организации транспортного комплекса Москвы
  transportComplexCoordinator: string;
  // Краткое описание продукта
  shortDescription: string;
  // Сроки реализации проекта
  timing: string;
  // КОНТЕКСТ И ПОТРЕБНОСТИ
  context: string;
  // Имя пути
  pathName: string;

  //Теги
  tags: string[];


  // Релевантность
  relevance: number;


  // region Сущности

  order: Order;
  effects: Effects;
  stage: Stage;
  team: Team;
  budget: Budget;
  status: Status;
  activities: Activities;
  meeting: Meeting;
  material: Material;
  // endregion

}
