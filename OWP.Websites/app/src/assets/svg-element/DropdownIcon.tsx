type DropdownIconProps = {
  colorDropdown: string;
  handleOnClick: () => void;
};
const DropdownIcon = (props: DropdownIconProps) => {
  const { colorDropdown, handleOnClick } = props;
  return (
    <svg
      data-testid='dropdown'
      onClick={handleOnClick}
      width='24'
      height='24'
      viewBox='0 0 24 24'
      fill='none'
      xmlns='http://www.w3.org/2000/svg'>
      <path d='M7 10L12 15L17 10H7Z' fill={`${colorDropdown}`} />
    </svg>
  );
};

export default DropdownIcon;
