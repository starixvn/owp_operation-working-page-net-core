import { Ref, forwardRef } from 'react';

type TPopup = {
  className?: string | undefined,
  value: string | undefined,
  onBlur: () => void,
};

const PopupEditor = forwardRef((props: TPopup, ref: Ref<HTMLTextAreaElement> | undefined) => (
  <div onBlur={props.onBlur} className={props.className ?? 'popup__editor'}>
    <textarea
      ref={ref}
      defaultValue={props.value}
      rows={8}
      cols={60}
      autoFocus
      onFocus={e => {
        const temp_value = e.target.value;
        e.target.value = '';
        e.target.value = temp_value;
      }} />
  </div>
));

PopupEditor.displayName = 'PopupEditor';
export default PopupEditor;
