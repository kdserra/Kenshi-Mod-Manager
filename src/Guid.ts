import { Utilities } from "./Utilities";

/**
* Provides Globally Unique Identifiers for the entirety of
* the runtime session.
*/
export class Guid {
    private static s_RegisteredGuids: string[] = [];

    /**
    * Returns a Globally Unique Identifier.
    */
    public static New(): string {
        while (true) {
            const randomUUID: string = Utilities.GenerateRandomString(32);
            if (Guid.s_RegisteredGuids.includes(randomUUID)) { continue; }
            return randomUUID;
        }
    }
}
