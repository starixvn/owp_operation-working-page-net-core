const getIndexRowShowNote = rowElements => {
  for (let index = 0; index < rowElements.length; index++) {
    const elementShowNote = rowElements[index].querySelector('.is-show');
    if (elementShowNote) {
      return index;
    }
  }
  return -1;
};

const getElementHeader = rowElements => {
  for (let index = 0; index < rowElements.length; index++) {
    const elementHeader = rowElements[index].querySelector(
      '.public_fixedDataTable_header'
    );
    if (elementHeader) {
      return rowElements[index];
    }
  }
  return null;
};

const getfourIndexElementLast = rowElements => {
  const fourIndexElementLast = [];
  for (let index = 0; index < rowElements.length; index++) {
    const elementLast = rowElements[index].querySelector('.is-last-row');
    elementLast && fourIndexElementLast.push(index);
  }
  return fourIndexElementLast;
};

const rollbackClassName = rowElements => {
  for (let index = 0; index < rowElements.length; index++) {
    rowElements[index].className = 'fixedDataTableRowLayout_rowWrapper';
  }
};

const appendClassNameToElementShowNote = function(element, className) {
  element.className = `fixedDataTableRowLayout_rowWrapper ${className}`;
};

const appendClassNameToElementHeader = function(element) {
  element.className = `${element.className} is-header`;
};

const appendClassNameToElementLastRow = function(element) {
  element.className = `${element.className} last-row`;
};

export {
  getIndexRowShowNote,
  getElementHeader,
  getfourIndexElementLast,
  rollbackClassName,
  appendClassNameToElementHeader,
  appendClassNameToElementShowNote,
  appendClassNameToElementLastRow
};
