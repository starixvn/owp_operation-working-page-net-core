type TPopupEditor = {
  defaultValue: string | undefined,
  className?: string,
  onBlur?: (value: string | undefined) => void;
  onChange?: (value: string | undefined) => void;
}

export default TPopupEditor;