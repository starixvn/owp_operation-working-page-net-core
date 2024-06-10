const trimWhiteSpaceAfterHashtag = assignment => assignment.replace(/^#\s+/gm, '#');
const getTraderNameFromInput = inputTextParam => {
  if (!inputTextParam) {
    return {
      userName: '',
      noteFirst: '',
      noteSecond: ''
    };
  }

  const inputText = `${inputTextParam} `;
  const indexOfPattern = inputText.indexOf('#');

  if (indexOfPattern <= -1) {
    return {
      userName: ' ',
      noteFirst: inputText,
      noteSecond: ' '
    };
  }

  let textBeforePattern = inputText.substring(0, indexOfPattern);
  const textAfterPattern = trimWhiteSpaceAfterHashtag(inputText.substring(textBeforePattern.length, inputText.length));
  const usernameText = textAfterPattern.substring(0, textAfterPattern.indexOf(' '));
  let textAfterUsername = textAfterPattern.substring(usernameText.length, textAfterPattern.length);

  if (textBeforePattern !== '' && textBeforePattern.endsWith('')) {
    textBeforePattern = `${textBeforePattern} `;
  }

  if (textAfterUsername !== '' && textAfterUsername.startsWith('')) {
    const trimAfterUsername = textAfterUsername.trim();
    textAfterUsername = `${trimAfterUsername}`;
  }
  const trimBeforePattern = textBeforePattern.trim();

  return {
    userName: usernameText,
    noteFirst: trimBeforePattern ? `${trimBeforePattern} ` : `${trimBeforePattern}`,
    noteSecond: textAfterUsername ? ` ${textAfterUsername}` : `${textAfterUsername}`
  };
};

const getAssignmentNoteToShow = otherAssignment => {
  const detectNote = getTraderNameFromInput(otherAssignment);
  if (detectNote.userName) {
    const formatUsername = detectNote.userName.replace('#', '');
    detectNote.userName = formatUsername;
  }
  return detectNote;
};

const formatOtherAssignment = otherAssignment => getAssignmentNoteToShow(otherAssignment);

export {
  getAssignmentNoteToShow,
  getTraderNameFromInput,
  formatOtherAssignment
};