import {RoleType, RoleTypeJSON} from "./RoleType";
import {SimpleEntity, SimpleEntityJSON} from "./SimpleEntity";

export class Role extends SimpleEntity {
  
  public name: string;
  public role_type: RoleType;
  
  constructor(init?: RoleJSON) {
    super(init);
    this.name = init?.name ?? "";
    this.role_type = new RoleType(init?.roleType);
  }
  
  public override toJSON(): RoleJSON {
    return {
      ...super.toJSON(),
      name: this.name,
      roleType: this.role_type.toJSON()
    };
  }
  
  public getIcon() {
    switch (this.name) {
      case "Dark Knight":
        return "dark_knight.png";
      case "Knight":
        return "knight.png";
      case "Lancer":
        return "lancer.png";
      case "Marksman":
        return "marksman.png";
      case "Paladin":
        return "paladin.png";
      case "Miner":
      case "Furrier":
      case "Blacksmith":
      case "Tailor":
      case "Alchemist":
      case "Woodcutter":
        return "profession.png";
      case "Ranger":
        return "ranger.png";
      case "Scoundrel":
        return "scoundrel.png";
      case "Spellblade":
        return "spellblade.png";
      default:
        return "";
    }
  }
  
}

export interface RoleJSON extends SimpleEntityJSON {
  name: string;
  roleType: RoleTypeJSON;
}
