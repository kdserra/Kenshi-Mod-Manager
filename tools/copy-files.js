/*
 * Copy files from one directory to another.
 * Usage: `node ./tools/copy-files.js ./dir1/ ./dir2/`
 */
const fs = require("fs");
const fse = require("fs-extra");
const { exit } = require("process");

const args = process.argv.splice(2, 2);

if (args === null || args === undefined || args.length !== 2) { exit(); }

try {
    if (!fs.existsSync(args[0])) { fs.mkdirSync(args[0]); }
    if (!fs.existsSync(args[1])) { fs.mkdirSync(args[1]); }
    if (!fs.statSync(args[0]).isDirectory() || !fs.statSync(args[1]).isDirectory()) { console.error('no2'); exit(); }
    fse.copySync(args[0], args[1], { overwrite: true | false });
}
catch(err) { console.error(err); exit(); }
