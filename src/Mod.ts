export class Mod {
  public readonly DisplayName: string;
  public readonly FileName: string;
  public readonly ModFilePath: string;
  public readonly InfoFilePath: string;
  public readonly ImageFilePath: string;
  public Index: number = -1;
  public Active: boolean = false;

  constructor(displayName: string, fileName: string, modFilePath: string, infoFilePath:string, imageFilePath: string) {
    this.DisplayName = displayName;
    this.FileName = fileName;
    this.ModFilePath = modFilePath;
    this.InfoFilePath = infoFilePath;
    this.ImageFilePath = imageFilePath;
  }
}
