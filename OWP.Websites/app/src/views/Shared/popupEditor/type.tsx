type TPopupEditor = {
  defaultValue: string | undefined,
  className?: string | undefined,
  onBlur?: (value: string | undefined) => void;
  onChange?: (value: string | undefined) => void;
}

export default TPopupEditor;