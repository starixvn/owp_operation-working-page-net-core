var fs = require('fs')
let rootDir = process.argv[2];
let lcovFile = rootDir + "\\js-lcov\\lcov.info";
let replacement = "SF:" + rootDir + "\\NAP.OWP.Websites\\app\\src\\";
console.log("======lcovFile was replacing===============");
fs.readFile(lcovFile, 'utf8', function (err, data) {
  if (err) {
    return console.log(err);
  }
  var result = data.replace(/SF:src\\/g, replacement);

  fs.writeFile(lcovFile, result, 'utf8', function (err) {
    if (err) return console.log(err);
  });
});

console.log("======lcovFile was replaced===============");