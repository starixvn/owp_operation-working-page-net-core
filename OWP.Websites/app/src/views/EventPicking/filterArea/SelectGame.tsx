import { SELECT_GAME_OPTIONS, SELECT_GAME } from './filterConst.json';
import MultipleSelectionTemplate from './MultipleSelectionTemplate';

const SelectGame = () => {
  return (
    <MultipleSelectionTemplate
      label='Select game'
      options={SELECT_GAME_OPTIONS}
      value={SELECT_GAME_OPTIONS}
      filterKey={SELECT_GAME}
      disabled={false} />
  );
};

export default SelectGame;