import { forwardRef } from 'react';

interface Props {
  defaultValue?: string,
}

const GroupName = forwardRef<HTMLInputElement, Props>((props: Props, ref) => {
  const { defaultValue } = props;

  return (
    <div className='group-form'>
      <div className='left-form'>
        <label className='label' htmlFor='groupName'>
          Group Name<span className='required'>*</span>
        </label>
      </div>
      <div className='right-form'>
        <input
          id='groupName'
          autoFocus
          ref={ref}
          type='text'
          defaultValue={defaultValue}
          className='input-field'
          maxLength={200}
          autoComplete='off'
        />
      </div>
    </div>
  );
});

GroupName.displayName = "GroupName";
export default GroupName;