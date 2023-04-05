import Superagent from "superagent";
import {SimpleEntity, SimpleEntityJSON} from "./SimpleEntity";
import {Unit, UnitJSON} from "./Unit";

export class Mission extends SimpleEntity {
  
  public unit_list: Unit[];
  public base_duration: number;
  public time_started: Date | null;
  
  constructor(init?: MissionJSON) {
    super(init);
    this.unit_list = init?.unitList.map(x => new Unit(x)) ?? [];
    this.base_duration = init?.baseDuration ?? 0;
    this.time_started = init?.timeStarted ?? null;
  }
  
  public override toJSON(): MissionJSON {
    return {
      ...super.toJSON(),
      unitList: this.unit_list.map(x => x.toJSON()),
      baseDuration: this.base_duration,
      timeStarted: this.time_started
    };
  }
  
  public static async load(): Promise<MissionJSON[]> {
    const response = await Superagent.get(`${process.env.NEXT_PUBLIC_API_HOST}/Mission/Load`).auth(localStorage.auth, {type: "bearer"}).send();
    return response.body;
  }
  
}

export interface MissionJSON extends SimpleEntityJSON {
  unitList: UnitJSON[];
  baseDuration: number;
  timeStarted: Date | null;
}
