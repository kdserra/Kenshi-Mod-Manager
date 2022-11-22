import { Utilities } from "./Utilities";

/**
* Provides Globally Unique Identifiers for the entirety of
* the runtime session.
*/
export class Guid {
    protected static s_RegisteredGuids: string[] = [];
    protected m_Guid:string;

    public static New(): Guid {
        while (true) {
            const randomUUID: string = Utilities.GenerateRandomString(32);
            if (Guid.s_RegisteredGuids.includes(randomUUID)) { continue; }
            return new Guid(randomUUID);
        }
    }

    protected constructor(guid:string)
    {
        this.m_Guid = guid;
    }

    public ToString():string
    {
        return this.m_Guid;
    }
}
