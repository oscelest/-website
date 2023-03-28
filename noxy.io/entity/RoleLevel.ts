import {Role, RoleJSON} from "./Role";
import {SimpleEntity, SimpleEntityJSON} from "./SimpleEntity";

export class RoleLevel extends SimpleEntity {
  
  public experience: number;
  public role: Role;
  
  constructor(init?: RoleLevelJSON) {
    super(init);
    this.experience = init?.experience ?? 0;
    this.role = new Role(init?.role);
  }
  
  public override toJSON(): RoleLevelJSON {
    return {
      ...super.toJSON(),
      experience: this.experience,
      role: this.role.toJSON()
    };
  }
  
  public getLevel() {
    return Math.floor(this.experience / 100) + 1;
  }
}

export interface RoleLevelJSON extends SimpleEntityJSON {
  experience: number;
  role: RoleJSON;
}
