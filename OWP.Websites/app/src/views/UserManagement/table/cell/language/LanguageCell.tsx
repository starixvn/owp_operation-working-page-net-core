import { useEffect, useRef, useState } from 'react';
import { TUserInfo } from '../../../type';
import store from '../../../app/useStoreNative';
import DropdownIcon from '../../../../../assets/svg-element/DropdownIcon';
import { hasPermissionUser, updateUserInfo } from '../../../utils';

const LanguageCell = (props: {
  handleShowDropdown: (isOpen: boolean) => void;
  dataSource: Array<TUserInfo>;
  rowIndex: number;
}) => {
  const dropdownRef = useRef<HTMLDivElement>(null);
  const [isOpenDropdown, setIsOpenDropdown] = useState(false);
  const { options, updateValue } = store.getState();
  const { rowIndex, dataSource, handleShowDropdown } = props;
  const { languageId, userId, sectionName, languages } = dataSource[rowIndex];

  useEffect(() => {
    const handleClickOutside = (event: MouseEvent) => {
      if (
        dropdownRef.current &&
        !dropdownRef.current.contains(event.target as Node)
      ) {
        setIsOpenDropdown(false);
      }
    };
    document.addEventListener('mousedown', handleClickOutside);
    return () => document.removeEventListener('mousedown', handleClickOutside);
  }, [dropdownRef]);

  useEffect(() => {
    isOpenDropdown && handleShowDropdown && handleShowDropdown(isOpenDropdown);
    // eslint-disable-next-line no-warning-comments
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [isOpenDropdown]);

  const toggleLanguage = async (language: string) => {
    const prevSelectedValue =
      languageId
        ?.split(',')
        .map(item => item.trim())
        .filter(item => item.length > 0) ?? [];
    const newValue = handleAddLanguageId(prevSelectedValue, language);

    const langueLabel = options.languages.filter(
      x => x.value?.toString() === language
    )[0].label;
    const prevSelectedLabel =
      languages
        ?.split(',')
        .map(item => item.trim())
        .filter(item => item.length > 0) ?? [];
    const newLabel = handleAddLanguageId(prevSelectedLabel, langueLabel);

    updateValue(newValue.join(','), 'languageId', rowIndex);
    updateValue(newLabel.join(','), 'languages', rowIndex);

    updateUserInfo({
      ColumnName: 'Language',
      UserId: `${userId}`,
      Value: newValue.join(',')
    });
  };
  const handleAddLanguageId = (prevSelected: Array<string>, language: string) =>
    prevSelected?.includes(language)
      ? prevSelected.filter(l => l !== language)
      : [...prevSelected, language];

  return (
    <>
      <div className='language-cell'>
        <span className='language-label'>{languages}</span>
        <span
          className='language-dropdown-icon'
          onClick={() => setIsOpenDropdown(true)}>
          <DropdownIcon colorDropdown='#282d38' />
        </span>
      </div>
      {isOpenDropdown && hasPermissionUser(sectionName) && (
        <div ref={dropdownRef} className='dropdown-multi-language is-show'>
          <ul>
            {options.languages.map(language => {
              const temp = language.value?.toString() ?? '';
              return (
                <li key={language.label}>
                  <label>
                    <input
                      type='checkbox'
                      checked={languageId
                        ?.split(',')
                        .map(item => item.trim())
                        .some(x => x.trim() === temp)}
                      onChange={() => toggleLanguage(temp)}
                    />
                    {language.label}
                  </label>
                </li>
              );
            })}
          </ul>
        </div>
      )}
    </>
  );
};

export default LanguageCell;
