import { Tag } from "./Tag";

export class TagHelper {
    public static TagToString(tag: Tag): string | null {
        switch (tag) {
            case Tag.Buildings: {
                return "Buildings";
            }
            case Tag.Characters: {
                return "Characters";
            }
            case Tag.Cheats: {
                return "Cheats";
            }
            case Tag.Clothing_Or_Armour: {
                return "Clothing/Armour";
            }
            case Tag.Environment_Or_Map: {
                return "Environment/Map";
            }
            case Tag.Factions: {
                return "Factions";
            }
            case Tag.GUI: {
                return "GUI";
            }
            case Tag.Gameplay: {
                return "Gameplay";
            }
            case Tag.Graphical: {
                return "Graphical";
            }
            case Tag.Items_Or_Weapons: {
                return "Items/Weapons";
            }
            case Tag.Races: {
                return "Races";
            }
            case Tag.Research: {
                return "Research";
            }
            case Tag.Total_Overhaul: {
                return "Total Overhaul";
            }
            case Tag.Translation: {
                return "Translation";
            }
            default: {
                return null;
            }
        }
    }

    public static StringToTag(tagString: string): Tag | null {
        switch (tagString) {
            case "Buildings": {
                return Tag.Buildings;
            }
            case "Characters": {
                return Tag.Characters;
            }
            case "Cheats": {
                return Tag.Cheats;
            }
            case "Clothing/Armour": {
                return Tag.Clothing_Or_Armour;
            }
            case "Environment/Map": {
                return Tag.Environment_Or_Map;
            }
            case "Factions": {
                return Tag.Factions;
            }
            case "GUI": {
                return Tag.GUI;
            }
            case "Gameplay": {
                return Tag.Gameplay;
            }
            case "Graphical": {
                return Tag.Graphical;
            }
            case "Items/Weapons": {
                return Tag.Items_Or_Weapons;
            }
            case "Races": {
                return Tag.Races;
            }
            case "Research": {
                return Tag.Research;
            }
            case "Total Overhaul": {
                return Tag.Total_Overhaul;
            }
            case "Translation": {
                return Tag.Translation;
            }
            default: {
                return null;
            }
        }
    }

    public static StringsToTags(tagStrings: string[]): Tag[] {
        let output: Tag[] = [];
        for (let i = 0; i < tagStrings.length; i++) {
            const tag: Tag | null = TagHelper.StringToTag(tagStrings[i]);
            if (tag === null || output.includes(tag)) { continue; }
            output.push(tag);
        }
        return output;
    }
}