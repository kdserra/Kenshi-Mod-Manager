const fse = require("fs-extra");

const srcDir = "./src/public";
const destDir = "./dist/public";

try {
  fse.copySync(srcDir, destDir, { overwrite: true | false });
} catch (err) {
  console.error(err);
}
