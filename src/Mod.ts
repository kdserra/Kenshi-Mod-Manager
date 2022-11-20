export class Mod {
  public readonly DisplayName: string;
  public readonly FileName: string;
  public readonly FilePath: string;
  public readonly Image: string;
  public Active: boolean = false;

  constructor(displayName: string, filename: string, filepath: string, image: string) {
    this.DisplayName = displayName;
    this.FileName = filename;
    this.FilePath = filepath;
    this.Image = image;
  }
}
