type TDropdownActionHeader = {
  refValue: React.RefObject<HTMLDivElement>;
};

const DropdownActionHeader = ({ refValue }: TDropdownActionHeader) => {
  const handleOpenPopup = (key: string) => {
    console.log(key);
  };

  return (
    <div className='dropdown-action is-show' ref={refValue}>
      <li
        className='add-special-product'
        onClick={() => handleOpenPopup('Saba Batch')}>
        Saba Batch
      </li>
      <li
        className='remove-special-product'
        onClick={() => handleOpenPopup('RT3 Batch')}>
        RT3 Batch
      </li>
    </div>
  );
};

export default DropdownActionHeader;
