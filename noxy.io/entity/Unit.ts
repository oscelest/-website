import {RoleLevel, RoleLevelJSON} from "./RoleLevel";
import {SimpleEntity, SimpleEntityJSON} from "./SimpleEntity";

export class Unit extends SimpleEntity {
  
  public name: string;
  public experience: number;
  public recruited: boolean;
  public role_level_list: RoleLevel[];
  
  constructor(init?: UnitJSON) {
    super(init);
    this.name = init?.name ?? "";
    this.experience = init?.experience ?? 0;
    this.recruited = init?.recruited ?? false;
    this.role_level_list = init?.roleLevelList.map(x => new RoleLevel(x)) ?? [];
  }
  
  public override toJSON(): UnitJSON {
    return {
      ...super.toJSON(),
      name: this.name,
      experience: this.experience,
      recruited: this.recruited,
      roleLevelList: this.role_level_list.map(x => x.toJSON())
    };
  }
  
  public getLevel() {
    return Math.floor(this.experience / 100) + 1;
  }
  
  public getCost() {
    return 100;
  }
  
  public getRoleLevelListSet() {
    return this.role_level_list.reduce(
      (result, value) => {
        const index = value.role.role_type.getIndex();
      
        if (!result[index]) result[index] = [];
        result[index].push(value);
      
        return result;
      },
      [] as RoleLevel[][]
    );
  }
  
}

export interface UnitJSON extends SimpleEntityJSON {
  name: string;
  experience: number;
  recruited: boolean;
  roleLevelList: RoleLevelJSON[];
}
