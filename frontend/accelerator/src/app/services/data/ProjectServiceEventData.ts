import {ProjectServiceEventType} from "./ProjectServiceEventType";

export class ProjectServiceEventData {
  constructor(options: {
    type: ProjectServiceEventType,
    data?: any,
  }) {
    this.type = options.type;
    this.data = options?.data || undefined;
  }

  type: ProjectServiceEventType;
  data: any;
}
