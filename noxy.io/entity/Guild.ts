import Superagent from "superagent";
import {LocalStorageKeyType} from "../enums/LocalStorageKeyType";
import {SimpleEntity, SimpleEntityJSON} from "./SimpleEntity";

export class Guild extends SimpleEntity {
  
  public name: string;
  public currency: number;
  public state: number | null;
  public time_unit_refresh: Date | null;
  public time_mission_refresh: Date | null;
  
  constructor(init: GuildJSON) {
    super(init);
    this.name = init.name;
    this.currency = init.currency;
    this.state = init.state;
    this.time_unit_refresh = init.timeUnitRefresh;
    this.time_mission_refresh = init.timeMissionRefresh;
  }
  
  public override toJSON(): GuildJSON {
    return {
      ...super.toJSON(),
      name: this.name,
      currency: this.currency,
      state: this.state,
      timeUnitRefresh: this.time_unit_refresh,
      timeMissionRefresh: this.time_mission_refresh
    };
  }
  
  public static async load() {
    const response = await Superagent.get(`${process.env.NEXT_PUBLIC_API_HOST}/Guild/Load`).auth(localStorage[LocalStorageKeyType.JWT], {type: "bearer"}).send();
    return response.status !== 204 ? new Guild(response.body) : undefined;
  }
  
  public static async register(name: string) {
    const response = await Superagent.post(`${process.env.NEXT_PUBLIC_API_HOST}/Guild/Register`).auth(localStorage[LocalStorageKeyType.JWT], {type: "bearer"}).send({name});
    return new Guild(response.body);
  }
  
}

export interface GuildJSON extends SimpleEntityJSON {
  name: string;
  currency: number;
  state: number | null;
  timeUnitRefresh: Date | null;
  timeMissionRefresh: Date | null;
}
