import { createWriteStream, readdirSync } from "original-fs";

export class Utilities {
    public static EncodeHTML(str: string): string {
        return str
        .replace(/&/g, '&amp;')
        .replace(/</g, '&lt;')
        .replace(/>/g, '&gt;')
        .replace(/"/g, '&quot;')
        .replace(/'/g, '&#39;')
        .replace(/`/g, '&#96;');
    }

    public static ReplaceBadStuff(str: string): string {
        if (str === null || str === undefined) { return ""; }
        return str.replaceAll("'", "").replaceAll(`"`, "").replaceAll("`", "");
    }

    public static GetSubdirectories(path: string): string[] {
        return (readdirSync(path, { withFileTypes: true }))
            .filter(dirent => dirent.isDirectory())
            .map(dirent => dirent.name);
    }

    public static GenerateRandomString(length: number): string {
        var result = '';
        var characters = 'ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789';
        var charactersLength = characters.length;
        for (var i = 0; i < length; i++) {
            result += characters.charAt(Math.floor(Math.random() * charactersLength));
        }
        return result;
    }

    public static WriteFile(fileName: string, data: string) {
        const stream = createWriteStream(`C:/Users/Kevin/Desktop/${fileName}`);
        stream.write(data);
        stream.close();
    }
}
