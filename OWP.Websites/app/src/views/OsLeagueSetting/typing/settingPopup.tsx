type TSettingPopup = {
  title?: string | null;
  isAdd: boolean;
  children?: JSX.Element;
  onSave: () => void;
};

export { TSettingPopup };
