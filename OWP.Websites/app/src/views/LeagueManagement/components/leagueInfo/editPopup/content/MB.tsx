import { InputItem } from '../component/InputItem';

const MB = (props: {
  maxbetWD_HDP: string,
  wD_NoOfHDP: string,
  maxbetWK_HDP: string,
  wK_NoOfHDP: string
}) => {
  const {
    maxbetWD_HDP,
    wD_NoOfHDP,
    maxbetWK_HDP,
    wK_NoOfHDP
  } = props;

  return (
    <div className='list'>
      <div className='title'>MB</div>
      <div className='content-column-2'>
        <InputItem
          label='Weekday HDP (K)'
          inputKey='maxbetWD_HDP'
          value={maxbetWD_HDP} />
        <InputItem
          label='Weekday NO. HDP'
          inputKey='wD_NoOfHDP'
          value={wD_NoOfHDP} />
      </div>
      <div className='content-column-2'>
        <InputItem
          label='Weekend HDP (K)'
          inputKey='maxbetWK_HDP'
          value={maxbetWK_HDP} />
        <InputItem
          label='Weekend NO. HDP'
          inputKey='wK_NoOfHDP'
          value={wK_NoOfHDP} />
      </div>
    </div>
  );
};

export default MB;