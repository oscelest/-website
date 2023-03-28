import {SimpleEntity, SimpleEntityJSON} from "./SimpleEntity";

export class RoleType extends SimpleEntity {
  
  public name: string;
  
  constructor(init?: RoleTypeJSON) {
    super(init);
    this.name = init?.name ?? "";
  }
  
  public override toJSON(): RoleTypeJSON {
    return {
      ...super.toJSON(),
      name: this.name
    };
  }
  
  public getIndex() {
    switch (this.name) {
      case "Affinity":
        return 0;
      case "Profession":
        return 1;
      default:
        throw new Error(`Name of RoleType unknown, got '${this.name}'`)
    }
  }
  
}

export interface RoleTypeJSON extends SimpleEntityJSON {
  name: string;
}
