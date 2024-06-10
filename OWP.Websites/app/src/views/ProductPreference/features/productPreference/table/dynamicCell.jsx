import {
  tableSchema,
  SECTION_ID_LIVE_ASSISTANT_VN,
  SECTION_ID_LIVE_ASSISTANT_PH
} from '../../../Const.json';
import BettypeCell from './cell/BettypeCell';
import LevelCell from './cell/LevelCell';
import renderName from './cell/renderName';
import RoleCell from './cell/RoleCell';
import MultipleGameCell from './cell/MultipleGameCell';
import RemarkCell from './cell/RemarkCell';
import { Cell } from 'fixed-data-table-2';

const renderLevel = (productInfo, rowIndex) => {
  const props = {
    productInfo,
    rowIndex
  };
  return <LevelCell {...props} />;
};

const renderRole = (productInfo, rowIndex) => {
  const props = {
    productInfo,
    rowIndex
  };
  return <RoleCell {...props} />;
};

const renderBettype = (productInfo, rowIndex, onShowDropdownListOption) => {
  const props = {
    productInfo,
    rowIndex,
    onShowDropdownListOption
  };
  return <BettypeCell {...props} />;
};

const renderMaxGame = (productInfo, rowIndex) => {
  const props = {
    productInfo,
    rowIndex
  };
  const { sectionId } = productInfo;
  if (
    sectionId === SECTION_ID_LIVE_ASSISTANT_VN ||
    sectionId === SECTION_ID_LIVE_ASSISTANT_PH
  ) {
    return <Cell />;
  }
  return <MultipleGameCell {...props} />;
};

const renderRemark = (productInfo, rowIndex) => {
  const props = {
    productInfo,
    rowIndex
  };
  return <RemarkCell {...props} />;
};

const cellFactory = {
  [tableSchema[0].columnKey]: renderName,
  [tableSchema[1].columnKey]: renderLevel,
  [tableSchema[2].columnKey]: renderRole,
  [tableSchema[3].columnKey]: renderBettype,
  [tableSchema[4].columnKey]: renderMaxGame,
  [tableSchema[5].columnKey]: renderRemark
};

const dynamicCell = (rowIndex, cellModel) => {
  const { dataSource, columnKey, onShowDropdownListOption } = cellModel;
  return cellFactory[columnKey](
    dataSource[rowIndex],
    rowIndex,
    onShowDropdownListOption
  );
};

export default dynamicCell;
