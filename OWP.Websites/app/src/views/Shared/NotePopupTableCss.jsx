const NotePopupTableCss = (tableElement, show) => {
  const rowElements = tableElement.querySelectorAll(
    '.fixedDataTableRowLayout_rowWrapper'
  );
  let indexRowShowNote = -1;
  let header = null;
  const fourIndexElementLast = [];

  for (let rowIndex = 0; rowIndex < rowElements.length; rowIndex++) {
    const elementShowNote = rowElements[rowIndex].querySelector('.is-show');
    const elementHeader = rowElements[rowIndex].querySelector(
      '.public_fixedDataTable_header'
    );
    if (elementShowNote) {
      indexRowShowNote = rowIndex;
    }
    if (elementHeader) {
      header = rowElements[rowIndex];
    }
    const elementLast = rowElements[rowIndex].querySelector('.is-last-row');
    elementLast && fourIndexElementLast.push(rowIndex);
  }

  for (const element of rowElements) {
    element.className = 'fixedDataTableRowLayout_rowWrapper';
  }

  if (header) {
    header.className = `${header.className} is-header`;
  }

  if (indexRowShowNote !== -1) {
    rowElements[
      indexRowShowNote
    ].className = `fixedDataTableRowLayout_rowWrapper ${
      show ? 'is-show-note' : ''
    }`;
  }
  fourIndexElementLast.forEach(item => {
    const element = rowElements[item];
    element.className = `${element.className} last-row`;
  });
};
export default NotePopupTableCss;
