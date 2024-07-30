import React, { KeyboardEvent, ClipboardEvent, ChangeEvent, useState } from 'react';
import { TextField } from '@mui/material';
import useStableDispatch from '../../customHook/useStableDispatch';
import { setMessage, setIsBlinkTitle } from '../chatBoardSlice';
import useQueryMessageSuggestion from '../../customHook/useQueryMessageSuggestion';
import '../../../../scss/pages/_InputMessage.scss';

const SLASH = '/';

type TProps = {
  inputMessage: string | undefined;
  isBlinkTitle: boolean | undefined;
  onSetTextFieldValue: (value: string | undefined) => void;
  onSetSelectedImage: (blob: File) => void;
  onSendMessage: (event: KeyboardEvent<HTMLInputElement>) => void;
};

const Comment = (props: TProps) => {
  const dispatch = useStableDispatch();
  const [suggestion, setSuggestion] = useState<string | undefined>();
  const [beginSubStringIndex, setBeginSubStringIndex] = useState<number>();
  const [endSubStringIndex, setEndSubStringIndex] = useState<number>();

  const suggestionSource = useQueryMessageSuggestion();
  const {
    inputMessage,
    isBlinkTitle,
    onSetTextFieldValue,
    onSetSelectedImage,
    onSendMessage } = props;

  const handleInputMessageChange = (event: ChangeEvent<HTMLInputElement>) => {
    dispatch(setMessage(event.currentTarget.value));
    const { target } = event;
    const { selectionStart, selectionEnd } = target;

    if (selectionStart && selectionEnd) {
      const previousSlashIndex = target.value.lastIndexOf(SLASH, selectionStart - 1);
      const nextSpaceIndex = target.value.indexOf(" ", selectionEnd);
      const beginSubstringIndex = previousSlashIndex < 0 ? 0 : previousSlashIndex + 1;
      const endSubstringIndex = nextSpaceIndex < 0 ? target.value.length : nextSpaceIndex;
      const stringTarget = SLASH + target.value.substring(beginSubstringIndex, endSubstringIndex);

      if (suggestionSource.length) {
        const suggest = suggestionSource.find(v => v.shortcut === stringTarget);
        setBeginSubStringIndex(beginSubstringIndex);
        setEndSubStringIndex(endSubstringIndex);
        setSuggestion(suggest?.message);
      }
    }
  };

  const handlePaste = (event: ClipboardEvent<HTMLInputElement>) => { //NOSONAR
    const items: DataTransferItemList = (event.clipboardData).items;
    if (items.length === 0) {
      return;
    }
    for (let i = 0; i < items.length; i++) {
      if (items[i].type.indexOf('image') !== -1) {
        const blob = items[i].getAsFile();
        if (blob) {
          const imageUrl = URL.createObjectURL(blob);
          onSetTextFieldValue(imageUrl);
          onSetSelectedImage(blob);
          break;
        }
      }
    }
  };

  const handleOnKeyPress = (e: KeyboardEvent<HTMLInputElement>) => {
    if (e.key === 'Enter' && !e.shiftKey) {
      !suggestion && onSendMessage(e);
      suggestion && handleClickSuggestMessage();
      e.preventDefault();
    }
  };

  const handleClickSuggestMessage = () => {
    if (inputMessage
      && beginSubStringIndex
      && endSubStringIndex
      && suggestion) {
      const newValue = inputMessage.substring(0, beginSubStringIndex - 1) + suggestion + inputMessage.substring(endSubStringIndex);
      dispatch(setMessage(newValue));
    }
    setSuggestion(undefined);
  };

  return (
    <div className='comment'>
      {suggestion && <div
        onClick={handleClickSuggestMessage}
        className='suggestion__message'>
        <span>{suggestion}
        </span>
      </div>}
      <TextField
        inputRef={textFieldRef => textFieldRef?.focus()}
        variant='outlined'
        placeholder='Type a message...'
        onChange={handleInputMessageChange}
        value={inputMessage}
        autoFocus
        multiline
        onFocus={() => isBlinkTitle && dispatch(setIsBlinkTitle(false))}
        onPaste={handlePaste}
        maxRows={4}
        onKeyPress={handleOnKeyPress}
        spellCheck={false}
      />
    </div>
  );
};

export default Comment;