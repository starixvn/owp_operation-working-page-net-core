import { InputItem } from '../component/InputItem';

const FtSpread = ({ spread }: {
  spread: string
}) => {
  return (
    <div className='list'>
      <div className='title'>FT Spread</div>
      <div className='content-column-2'>
        <InputItem
          label='HDP-OU(%)'
          inputKey='spread'
          value={spread}
        />
      </div>
    </div >
  );
};

export default FtSpread;