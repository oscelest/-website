import Superagent from "superagent";
import {LocalStorageKeyType} from "../enums/LocalStorageKeyType";
import {Role, RoleJSON} from "./Role";
import {SimpleEntity, SimpleEntityJSON} from "./SimpleEntity";
import {Unit, UnitJSON} from "./Unit";

export class Mission extends SimpleEntity {
  
  public unit_list: Unit[];
  public role_list: Role[];
  public time_started: Date | null;
  
  constructor(init?: MissionJSON) {
    super(init);
    this.unit_list = init?.unitList.map(x => new Unit(x)) ?? [];
    this.role_list = init?.roleList.map(x => new Role(x)) ?? [];
    this.time_started = init?.timeStarted ?? null;
  }
  
  public override toJSON(): MissionJSON {
    return {
      ...super.toJSON(),
      unitList: this.unit_list.map(x => x.toJSON()),
      roleList: this.role_list.map(x => x.toJSON()),
      timeStarted: this.time_started
    };
  }
  
  public static async load(): Promise<Mission[]> {
    const {body} = await Superagent.get(`${process.env.NEXT_PUBLIC_API_HOST}/Mission/Load`).auth(localStorage[LocalStorageKeyType.JWT], {type: "bearer"}).send();
    return (body as MissionJSON[]).map(x => new Mission(x));
  }
  
  public static async refreshAvailable(): Promise<Mission[]> {
    const {body} = await Superagent.post(`${process.env.NEXT_PUBLIC_API_HOST}/Mission/RefreshAvailable`).auth(localStorage[LocalStorageKeyType.JWT], {type: "bearer"}).send();
    return (body as MissionJSON[]).map(x => new Mission(x));
  }
  
}

export interface MissionJSON extends SimpleEntityJSON {
  unitList: UnitJSON[];
  roleList: RoleJSON[];
  timeStarted: Date | null;
}
