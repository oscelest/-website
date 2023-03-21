import {v4} from "uuid";

export abstract class SimpleEntity {
  
  public id: string;
  public time_created: Date;
  public time_updated: Date | null;
  
  protected constructor(init?: SimpleEntityJSON) {
    this.id = init?.id ?? v4();
    this.time_created = new Date(init?.timeCreated ?? 0);
    this.time_updated = init?.timeCreated ? new Date(init.timeCreated) : null;
  }
  
  public toString() {
    return this.id;
  }
  
  public toJSON(): SimpleEntityJSON {
    return {
      id: this.id,
      timeCreated: this.time_created.toString(),
      timeUpdated: this.time_updated?.toString() ?? null
    };
  }
}

export interface SimpleEntityJSON {
  id: string;
  timeCreated: string;
  timeUpdated: string | null;
}
