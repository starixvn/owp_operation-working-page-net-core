import React, { useState } from 'react';
import { Input } from 'reactstrap';
import '../../../../scss/pages/_InputMessage.scss';
import useQueryMessageSuggestion from '../../customHook/useQueryMessageSuggestion';

const SLASH='/';

const Comment = React.forwardRef((_, commentRef)=>{
  const suggestionSource= useQueryMessageSuggestion();
  const [value, setValue]= useState();
  const [suggestion, setSuggestion] = useState();
  const [beginSubStringIndex, setBeginSubStringIndex] = useState();
  const [endSubStringIndex, setEndSubStringIndex] = useState();

const handleValueChange=event=>{
  const {target}= event;
  const {selectionStart, selectionEnd}= target;
  const previousSlashIndex = target.value.lastIndexOf(SLASH, selectionStart - 1);
  const nextSpaceIndex = target.value.indexOf(" ", selectionEnd );
  const beginSubstringIndex = previousSlashIndex < 0 ? 0 : previousSlashIndex + 1;
  const endSubstringIndex = nextSpaceIndex < 0 ? target.value.length : nextSpaceIndex;
  const stringTarget = SLASH+target.value.substring(beginSubstringIndex, endSubstringIndex);
  const suggest= suggestionSource.find(v=>v.shortcut===stringTarget);
  setBeginSubStringIndex(beginSubstringIndex);
  setEndSubStringIndex(endSubstringIndex);
  setSuggestion(suggest?.message);
  setValue(target.value);
};
 
const handleClickSuggestMessage=()=>{
  if(value
    && beginSubStringIndex
     && endSubStringIndex
    && suggestion){
      const newValue= value.substring(0, beginSubStringIndex-1) + suggestion + value.substring(endSubStringIndex);
    setValue(newValue);
  }
  setSuggestion();
};

const handleKeyDown=e=>{
  if (e.key === "Enter" && suggestion)
  {
      const newValue= value.substring(0, beginSubStringIndex-1) + suggestion + value.substring(endSubStringIndex);
      setSuggestion();
      setValue(newValue);
      e.preventDefault();
  }
};

return (
    <div className='right-form'>
          {suggestion && <div
           onClick={handleClickSuggestMessage}
            className='suggestion__message'>
            <span>{suggestion}
            </span>
          </div>}
          <div className='box-textmessage'>
            <Input
              type='textarea'
              value={value}
              id='comment'
              innerRef={commentRef}
              name='message'
              className='input-field'
              maxLength='200'
              onChange={handleValueChange}
              multiple
              onKeyDown={handleKeyDown}
            />
          </div>
        </div>
  );
});

export default Comment;