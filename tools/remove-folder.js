/*
 * Remove all contents of a folder and then delete the folder.
 * Usage: `node ./tools/remove-folder.js ./dir1/ ./dir2/ ./dir3/`
 */
const fs = require("fs");
const { exit } = require("process");

const args = process.argv.slice(2);
if (args === null || args === undefined || args.length === 0) { exit(); }

for (let i = 0; i < args.length; i++) {
    try {
        if (!fs.existsSync(args[i])) { continue; }
        if (!fs.statSync(args[i]).isDirectory()) { continue; }
        fs.rmSync(args[i], { recursive: true, force: true });
    }
    catch(err) { console.error(err); exit(); }
}
