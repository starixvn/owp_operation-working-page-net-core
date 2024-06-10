import { useMemo } from 'react';
import { useSelector, useStore } from 'react-redux';
import { selectSpecialProductData } from '../../../selector/SelectorSpecialProduct';
import ReactTable from 'react-table';
import MaxbetCell from './cell/MaxbetCell';
import RemarkCell from './cell/RemarkCell';
import LastUpdatedCell from './cell/LastUpdatedCell';
import ActionCell from './cell/ActionCell';
import SpreadCell from './cell/SpreadCell';
import { nomalizeArraySpecialProductMaxbetSpreadResponse } from '../../../helper/nomalizeResponseHelper';

const SpecialProductTable = () => {
  const specialProductSelector = useSelector(selectSpecialProductData);
  const specialProductData = useMemo(
    () =>
      nomalizeArraySpecialProductMaxbetSpreadResponse(
        specialProductSelector.specialProductData
      ),
    [specialProductSelector.specialProductData]
  );
  const store = useStore();
  const { policyData } = store.getState().policyReducer;
  const hasWrite = policyData.generalPolicy.HasWrite;

  const renderCell = cellInfo => {
    const { classes, column, original } = cellInfo;

    switch (classes[1]) {
      case 'remark':
        return (
          <RemarkCell value={original.Remark} productId={original.ProductId} />
        );
      case 'action':
        return (
          <ActionCell
            productId={original.ProductId}
            maxbetFTHDP={original.Maxbet_FTHDP}
          />
        );
      case 'last-update':
        return (
          <LastUpdatedCell
            updatedBy={original.LastUpdate}
            productId={original.ProductId}
            maxbetFTHDP={original.Maxbet_FTHDP}
          />
        );
      default:
        if (column.Header === 'Spread') {
          return (
            <SpreadCell
              value={original[classes[1]]}
              productId={original.ProductId}
              propKey={classes[1]}
            />
          );
        } else {
          return (
            <MaxbetCell
              value={original[classes[1]]}
              productId={original.ProductId}
              propKey={classes[1]}
              rowIndex={cellInfo.row._index}
            />
          );
        }
    }
  };
  const generateColumns = useMemo(() => {
    const generateSubCols = header => {
      return [
        {
          Header: 'MB',
          width: 95,
          Cell: renderCell,
          className: `Maxbet_${header}`
        },
        {
          Header: 'Spread',
          width: 95,
          Cell: renderCell,
          className: `Spread_${header}`
        }
      ];
    };

    const cols = [
      {
        Header: 'MB of FT.HDP',
        width: 95,
        Cell: renderCell,
        className: 'Maxbet_FTHDP'
      },
      {
        Header: 'No.of Corner',
        columns: generateSubCols('NoOfCorner')
      },
      {
        Header: 'Next Corner',
        columns: generateSubCols('NextCorner')
      },
      {
        Header: '10-15 Mins',
        columns: generateSubCols('10_15Mins')
      },
      {
        Header: 'Total Booking',
        columns: generateSubCols('TB')
      },
      {
        Header: 'Next Booking',
        columns: generateSubCols('NextTB')
      },
      {
        Header: 'Penalty Shootout',
        columns: generateSubCols('PS')
      },
      {
        Header: 'Winner',
        columns: generateSubCols('Winner')
      },
      {
        columns: [
          {
            Header: 'Remark',
            width: hasWrite ? 205 : 285,
            className: 'remark',
            Cell: renderCell
          }
        ]
      },
      {
        columns: [
          {
            Header: 'Last Update',
            width: 120,
            className: 'last-update',
            Cell: renderCell
          }
        ]
      }
    ];

    if (hasWrite) {
      cols.push({
        columns: [
          {
            Header: 'Action',
            width: 80,
            className: 'action',
            Cell: renderCell
          }
        ]
      });
    }

    return cols;
  }, [hasWrite]);

  return (
    <div className='table'>
      <ReactTable
        data={specialProductData}
        columns={generateColumns}
        resizable={false}
        sortable={false}
        filterable={false}
        className='-striped'
        showPagination={false}
        pageSize={specialProductData.length}
        NoDataComponent={() => null}
      />
    </div>
  );
};

export default SpecialProductTable;
