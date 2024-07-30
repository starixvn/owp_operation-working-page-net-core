import { isEqual } from 'lodash';
import {
  DataSource,
  SortHeaderProps,
  SortState,
  TSectionOption,
  TTotalGameInfo
} from '../feature/type/TableType.native';
import {
  keyOfTotalGameInfo,
  orderOfLevel,
  asc,
  desc
} from '../TableConst.json';

const sortData = (
  dataSource: DataSource[],
  property: string,
  columnKey: string,
  orderType: string
) => {
  const columnKeyTotalMatch = keyOfTotalGameInfo.map(x => x.key);
  let dataSorted: DataSource[] = [];

  if (columnKeyTotalMatch.includes(columnKey)) {
    dataSorted = [...dataSource].sort((src, dest) => {
      const first = src[property as keyof DataSource] as TTotalGameInfo;
      const second = dest[property as keyof DataSource] as TTotalGameInfo;
      return correctComparer(
        first[columnKey as keyof TTotalGameInfo],
        second[columnKey as keyof TTotalGameInfo],
        orderType
      );
    });
  } else {
    dataSorted = [...dataSource].sort((src, dest) =>
      correctComparer(
        orderOfLevel.indexOf(src[property as keyof DataSource] as string),
        orderOfLevel.indexOf(dest[property as keyof DataSource] as string),
        orderType
      )
    );
  }

  return [...dataSorted];
};

const correctComparer = (
  source: number,
  dest: number,
  directionArg: string
): number => {
  const schemaComparer: Record<string, number> = {
    [desc]: dest - source,
    [asc]: source - dest
  };

  return schemaComparer[directionArg];
};

const filterData = (dataSource: DataSource[], value: string) =>
  dataSource.filter((x: DataSource) =>
    x.userName.toLowerCase().includes(value.toLowerCase())
  );

const buildOrderSelection = (sortState: SortState, props: SortHeaderProps) => {
  const { columnKeySelected, orderType, propertySelected } = sortState;
  const { property, columnKey } = props;
  if (
    isEqual(propertySelected, property) &&
    isEqual(columnKey, columnKeySelected)
  ) {
    return isEqual(orderType, asc) ? desc : asc;
  }
  return desc;
};
const buildSectionInString = (sections: Array<TSectionOption>) =>
  sections.map(x => x.value).join(',');
export { sortData, filterData, buildOrderSelection, buildSectionInString };
