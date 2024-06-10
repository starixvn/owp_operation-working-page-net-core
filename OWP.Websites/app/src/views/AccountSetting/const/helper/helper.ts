import { TQuickMessage } from '../../Type';

const isFileExceed5Mb = (file: File) => {
  const BYTES = 1024;
  const MB_MAX = 5;

  return file.size / (BYTES * BYTES) >= MB_MAX;
};

const isValidQuickMessage = (
  editModel: TQuickMessage,
  quickMessages: TQuickMessage[]
) => {
  const { shortcut, message, id } = editModel;
  if (!shortcut.replace('/', '') || !message) {
    return 'You must fill in all the text boxes';
  }
  if (quickMessages.some(m => m.shortcut === shortcut && m.id !== id)) {
    return 'This shortcut already exists';
  }
  return '';
};

export { isFileExceed5Mb, isValidQuickMessage };
