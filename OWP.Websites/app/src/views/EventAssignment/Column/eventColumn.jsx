import { Column } from 'fixed-data-table-2';
import FilterHeader from '../../Shared/FilterHeader/FilterHeaderNative';
import EventCell from '../Cell/EventCell';
import {
  Event
} from '../constants/Column';
import { getKeyword } from '../FilterHelper';
import { v4 as uuidv4 } from 'uuid';

const renderEventColumn = (filterModel, rest) => {
  const {
    dataSource,
    isAllowAddEvent,
    policy,
    addSpecialEvent,
    openDetailInformation,
    detailInformationPolicy,
    closePopup
  } = rest;
  const { onClickFilter, onClearTextFilter, columnFilters } = filterModel;
  return (
    <Column
      key={Event.columnKey}
      columnKey={Event.columnKey}
      header={
        <FilterHeader
          key={closePopup ? uuidv4() : Event.columnKey}
          name={Event.name}
          title={Event.title}
          keyword={getKeyword(columnFilters, Event.name)}
          onClear={() => onClearTextFilter([])}
          onSubmit={(name, value) => onClickFilter(name, value)}
        />
      }
      width={Event.width}
      fixed
      cell={({ rowIndex }) => (
        <EventCell
          isAllowAddEvent={isAllowAddEvent}
          policy={policy}
          addSpecialEvent={addSpecialEvent}
          openDetailInformation={openDetailInformation}
          detailInformationPolicy={detailInformationPolicy}
          data={dataSource}
          rowIndex={rowIndex}
        />
      )}
    />
  );
};

export {
  renderEventColumn
};
