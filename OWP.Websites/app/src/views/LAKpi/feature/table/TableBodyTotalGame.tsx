import { TTotalGameInfo } from '../type/TableType.native';
import { uniqueId } from 'lodash';
import { keyOfTotalGameInfo } from '../../TableConst.json';

const TableBodyTotalGame = (props: TTotalGameInfo) => {
  return (
    <>
      {keyOfTotalGameInfo.map(x => {
        const value = props[x.key as keyof TTotalGameInfo];
        return value < 0 ? (
          <td key={uniqueId()} className='value-negative'>
            {Math.abs(value)}
          </td>
        ) : (
          <td key={uniqueId()}>{value}</td>
        );
      })}
    </>
  );
};

export default TableBodyTotalGame;
