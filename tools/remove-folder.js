const { rmSync, statSync, existsSync } = require("fs");
const { exit } = require("process");

const args = process.argv.slice(2);
if (args === null || args === undefined || args.length === 0) { exit(); }

for (let i = 0; i < args.length; i++)
{
    if (!existsSync(args[i])) { continue; }
    if (!statSync(args[i]).isDirectory()) { continue; }
    rmSync(args[i], { recursive: true, force: true });
}
