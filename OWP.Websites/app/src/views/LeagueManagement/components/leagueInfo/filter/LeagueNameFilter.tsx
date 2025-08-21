import { KeyboardEvent, useEffect, useState } from 'react';

interface LeagueNameFilterProps {
  onSearchValueChange: (value: string, field: string) => void;
}

const LeagueNameFilter = ({ onSearchValueChange }: LeagueNameFilterProps) => {
  const searchParams = new URLSearchParams(document.location.search);
  const owpLeagueName = searchParams.get('leagueName');
  const [value, setValue] = useState(owpLeagueName || '');

  const handleKeyDown = (e: KeyboardEvent<HTMLInputElement>) => {
    if (e.key === 'Enter') {
      onSearchValueChange(value, 'leagueName');
    }
  };

  const handleInputChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const newValue = e.target.value;
    setValue(newValue);
    onSearchValueChange(newValue, 'leagueName');
  };

  useEffect(() => {
    if (owpLeagueName) {
      setValue(owpLeagueName);
      onSearchValueChange(owpLeagueName, 'leagueName');
    }
  }, [owpLeagueName, onSearchValueChange]);

  return (
    <div className='league-filter'>
      <span> League search</span>
      <input
        type='text'
        onKeyDown={handleKeyDown}
        value={value}
        onChange={handleInputChange}
      />
    </div>
  );
};

export default LeagueNameFilter;
