import path from "path";
import { Guid } from "./Guid";
import { Tag } from "./Tag";

export class Mod {
  public readonly Title: string;
  public readonly ModFilePath: string;
  public readonly InfoFilePath: string;
  public readonly ImageFilePath: string;
  public readonly Tags: Tag[];
  public readonly Guid: string;
  public Index: number = -1;
  public Active: boolean = false;

  constructor(
    title: string,
    modFilePath: string,
    infoFilePath: string,
    imageFilePath: string,
    tags: Tag[]
  ) {
    this.Title = title;
    this.ModFilePath = modFilePath;
    this.InfoFilePath = infoFilePath;
    this.ImageFilePath = imageFilePath;
    this.Tags = tags;
    this.Guid = Guid.New();
  }

  public GetModFileName(): string {
    return path.basename(this.ModFilePath);
  }

  public HasTag(tag: Tag): boolean {
    for (let i = 0; i < this.Tags.length; i++) {
      if (this.Tags[i] !== tag) { continue; }
      return true;
    }
    return false;
  }
}
