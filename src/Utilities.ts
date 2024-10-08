import * as fs from 'fs';
import path from 'path';

export class Utilities {
    public static EncodeHTMLEntity(str: string): string {
        return str.replace(/&#[0-9]+;|&#x[0-9a-fA-F]+;|&[0-9a-zA-Z]{2,};|./gu, (m) => {
            if (m.length >= 4 && m[0] === '&') { return m; }
            if (m.length === 1 && m.match(/[a-zA-Z0-9\s\t\n\r~`!@#$%^&*_+=(){}[\]/\\,?:;|.-]/)) { return m; }
            return `&#${m.codePointAt(0)};`;
        });
    }

    public static EncodeURL(str: string): string {
        const dir: string = encodeURI(path.dirname(str));
        const fileName: string = encodeURIComponent(path.basename(str));
        const url: string = path.join(dir, fileName);
        const encodedURL: string = Utilities.EncodeHTMLEntity(url.replace(/\\/g, '\\\\'));
        return encodedURL;
    }

    public static GetSubdirectories(path: string): string[] {
        return (fs.readdirSync(path, { withFileTypes: true }))
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

    public static MatchFirmFirst(str: string, regex: RegExp): string {
        const matchArray: RegExpMatchArray | null = str.match(regex);
        if (matchArray === null) { return ""; }
        const output:string = matchArray[0].toString();
        return output;
    }

    public static MatchFirm(str: string, regex: RegExp): string[] {
        const matchArray: RegExpMatchArray | null = str.match(regex);
        if (matchArray === null) { return []; }
        const output:string[] = [];
        for (let i = 0; i < matchArray.length; i++)
        {
            output.push(matchArray[i].toString());
        }
        return output;
    }

    public static WriteFile(fileName: string, data: string) {
        const stream = fs.createWriteStream(`C:/Users/Kevin/Desktop/${fileName}`);
        stream.write(data);
        stream.close();
    }
}
