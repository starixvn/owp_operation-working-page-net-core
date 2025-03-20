import { useLayoutEffect, useRef, useState } from 'react';
import { shallow } from 'zustand/shallow';
import useStore from '../../app/useStoreNative';
import TableHeader from './TableHeader';
import TableBody from './TableBody';
import { numberElement, headerHeight } from './../../const/Const.json';
import { getTotalRecords } from '../helper/AssigneeSuggestionHelper';

const SuggestionTable = () => {
  const [maxHeight, setMaxHeight] = useState(window.innerHeight - headerHeight);
  const tableRef = useRef<HTMLDivElement>(null);
  const { workScheduleInfo } = useStore(
    state => ({
      workScheduleInfo: state.workScheduleInfo
    }),
    shallow
  );

  const handleResizeBrowser = () => {
    const { innerHeight } = window;
    setMaxHeight(innerHeight - headerHeight);
  };

  useLayoutEffect(() => {
    const abort = new AbortController();
    window.addEventListener('resize', handleResizeBrowser, abort);
    return () => abort.abort();
  }, []);

  useLayoutEffect(() => {
    if (tableRef.current) {
      const element = tableRef.current;
      const currentTimeElement = element.querySelector('.current-timeline');
      const freezeElement = element.querySelector('.working-scheduler-header');
      if (
        currentTimeElement instanceof HTMLElement &&
        freezeElement instanceof HTMLElement
      ) {
        element.scrollLeft =
          currentTimeElement?.offsetLeft -
          freezeElement?.offsetWidth -
          currentTimeElement?.offsetWidth * numberElement;
      }
    }
  }, []);

  return (
    <div>
      <div ref={tableRef} className='table-content' style={{ maxHeight }}>
        <table>
          <thead>
            <TableHeader
              filterDate={workScheduleInfo.matchInfo.filterDate}
              eventDate={workScheduleInfo.matchInfo.koTime}></TableHeader>
          </thead>
          <tbody>
            <TableBody workingShifts={workScheduleInfo}></TableBody>
          </tbody>
        </table>
      </div>
      <span className='total-record'>
        Total records: {getTotalRecords(workScheduleInfo)}
      </span>
    </div>
  );
};

export default SuggestionTable;
