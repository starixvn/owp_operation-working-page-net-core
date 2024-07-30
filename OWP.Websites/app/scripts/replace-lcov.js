import fs from 'fs';
import path from 'path';
import { fileURLToPath } from 'url';

const __filename = fileURLToPath(import.meta.url);
const __dirname = path.dirname(__filename);

const lcovFilePath = path.join(__dirname, '..', 'lcov.info');

fs.readFile(lcovFilePath, 'utf8', (err, data) => {
  if (err) {
    console.error('Error reading lcov.info:', err);
    return;
  }

  const updatedData = data.replace(
    /SF:\.\.\\src\\/g,
    'SF:.\\OWP.Websites\\app\\src\\'
  );

  const finalUpdatedData = updatedData.replace(/\\/g, '/');

  fs.writeFile(lcovFilePath, finalUpdatedData, 'utf8', err => {
    if (err) {
      console.error('Error writing lcov.info:', err);
      return;
    }
    console.log('lcov.info paths updated successfully.');
  });
});
