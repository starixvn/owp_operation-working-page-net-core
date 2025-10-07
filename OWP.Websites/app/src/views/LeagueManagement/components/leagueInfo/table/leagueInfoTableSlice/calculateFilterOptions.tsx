import { uniqBy } from 'lodash';
import { TCondition, TDataSource } from '../TableType';
import { Option } from '../../../../../Shared/MultipleSelection/type';

const buildLeagueGroupOptions = (entities: TDataSource[]) =>
  swapBlanks(
    uniqBy(
      [...entities].map(o => ({
        value: o.leagueGroup ?? null,
        label: o.leagueGroup ?? '(Blanks)'
      })),
      i => i.label
    )
  );
const buildLeagueOptions = (entities: TDataSource[]) =>
  swapBlanks(
    uniqBy(
      [...entities]
        .sort((first, second) => first.league.localeCompare(second.league))
        .map(o => ({
          value: o.league ?? null,
          label: o.league ?? '(Blanks)'
        })),
      i => i.label
    )
  );

const buildTvOptions = (entities: TDataSource[]) => {
  const hasTv = entities.some(o => o.condition?.hasTv && !o.condition?.hasIpTv);
  const hasIpTv = entities.some(o => o.condition?.hasIpTv && !o.condition?.hasTv);
  const hasTvAndIpTv = entities.some(o => o.condition?.hasTv && o.condition?.hasIpTv);
  const blanks = entities.some(o => !o.condition?.hasTv && !o.condition?.hasIpTv);
  const options = [];
  if (hasIpTv) {
    options.push({
      value: 'IPTV',
      label: 'IPTV'
    });
  }
  if (hasTv) {
    options.push({
      value: 'TV',
      label: 'TV'
    });
  }
  if (hasTvAndIpTv) {
    options.push({
      value: 'TV/IPTV',
      label: 'TV/IPTV'
    });
  }
  if (blanks) {
    options.push({
      value: null,
      label: '(Blanks)'
    });
  }
  return options;
};

const buildConditionOptions = (first: TCondition[][], second: TCondition[]) => {
  const options: Option[] = uniqBy(
    [...second]
      .sort((a, b) => (a?.value || '').localeCompare(b?.value || ''))
      .map(
        o =>
          ({
            value: o?.value?.length > 0 ? o.value : null,
            label:
              o?.value?.length > 0
                ? o.type === 'Contains'
                  ? `*${o.value}*`
                  : o.value
                : null
          } as Option)
      ),
    i => i.label
  );

  const checkEmpty = first.some(x => x.length === 0);

  checkEmpty &&
    options.push({
      value: null,
      label: '(Blanks)'
    });
  return options;
};
const buildRemarkOptions = (data: string[]) =>
  swapBlanks(
    uniqBy(
      [...data].map(o => ({
        value: o ?? null,
        label: o?.length ? o : '(Blanks)'
      })),
      i => i.label
    )
  );
const buildSpreadHDPAndOUOptions = (entities: TDataSource[]) =>
  swapBlanks(
    uniqBy(
      [...entities].map(o => ({
        value: o.spreadShow ?? null,
        label: o.spreadShow ? o.spreadShow : '(Blanks)'
      })),
      i => i.label
    )
  );

const buildMaxBetWeekdayHDPGroupOptions = (entities: TDataSource[]) =>
  swapBlanks(
    uniqBy(
      [...entities]
        .sort((first, second) =>
          (first.maxbetWD_HDP_Show || '').localeCompare(
            second.maxbetWD_HDP_Show || ''
          )
        )
        .map(o => ({
          value: o.maxbetWD_HDP_Show ?? null,
          label:
            o.maxbetWD_HDP_Show && o.maxbetWD_HDP_Show.length > 0
              ? o.maxbetWD_HDP_Show
              : '(Blanks)'
        })),
      i => i.label
    )
  );
const buildMaxBetWeekdayNoHDPGroupOptions = (entities: TDataSource[]) =>
  swapBlanks(
    uniqBy(
      [...entities].map(o => ({
        value: o.wD_NoOfHDP_Show ?? null,
        label:
          o.wD_NoOfHDP_Show && o.wD_NoOfHDP_Show.length
            ? o.wD_NoOfHDP_Show
            : '(Blanks)'
      })),
      i => i.label
    )
  );
const buildMaxBetWeekendHDPGroupOptions = (entities: TDataSource[]) =>
  swapBlanks(
    uniqBy(
      [...entities]
        .sort((first, second) =>
          (first.maxbetWK_HDP_Show || '').localeCompare(
            second.maxbetWK_HDP_Show || ''
          )
        )
        .map(o => ({
          value: o.maxbetWK_HDP_Show ?? null,
          label:
            o.maxbetWK_HDP_Show && o.maxbetWK_HDP_Show.length
              ? o.maxbetWK_HDP_Show
              : '(Blanks)'
        })),
      i => i.label
    )
  );
const buildMaxBetWeekendNoHDPGroupOptions = (entities: TDataSource[]) =>
  swapBlanks(
    uniqBy(
      [...entities].map(o => ({
        value: o.wK_NoOfHDP_Show ?? null,
        label:
          o.wK_NoOfHDP_Show && o.wK_NoOfHDP_Show.length
            ? o.wK_NoOfHDP_Show
            : '(Blanks)'
      })),
      i => i.label
    )
  );

const buildLSTNoteOptions = (entities: TDataSource[]) =>
  swapBlanks(
    uniqBy(
      [...entities].map(o => ({
        value: o.lstNote ?? null,
        label: o.lstNote && o.lstNote.length ? o.lstNote : '(Blanks)'
      })),
      i => i.label
    )
  );
const buildRemark2Options = (entities: TDataSource[]) =>
  swapBlanks(
    uniqBy(
      [...entities].map(o => ({
        value: o.remark2 ?? null,
        label: o.remark2 && o.remark2.length ? o.remark2 : '(Blanks)'
      })),
      i => i.label
    )
  );
const buildCommentOptions = (entities: TDataSource[]) =>
  swapBlanks(
    uniqBy(
      [...entities].map(o => ({
        value: o.comment ?? null,
        label: o.comment && o.comment.length ? o.comment : '(Blanks)'
      })),
      i => i.label
    )
  );
const swapBlanks = (options: Option[]) => {
  const indexBlanks = options.findIndex(o => o.label === '(Blanks)');
  if (indexBlanks > -1) {
    const { length } = options;
    const temp = options[indexBlanks];
    options[indexBlanks] = options[length - 1];
    options[length - 1] = temp;
  }
  return options;
};
const helper = {
  League_Group: (dataSource: TDataSource[]) =>
    buildLeagueGroupOptions(dataSource),
  League: (dataSource: TDataSource[]) => buildLeagueOptions(dataSource),
  Tv: (dataSource: TDataSource[]) => buildTvOptions(dataSource),
  Round: (dataSource: TDataSource[]) =>
    buildConditionOptions(
      dataSource.map(x => x.condition.round),
      dataSource.flatMap(x => x.condition.round)
    ),
  League_Division: (dataSource: TDataSource[]) =>
    buildConditionOptions(
      dataSource.map(x => x.condition.division),
      dataSource.flatMap(x => x.condition.division)
    ),
  Remark: (dataSource: TDataSource[]) =>
    buildRemarkOptions(dataSource.map(x => x.condition.remark)),
  Spread_HDP_OU: (dataSource: TDataSource[]) =>
    buildSpreadHDPAndOUOptions(dataSource),
  MB_WD_HDP: (dataSource: TDataSource[]) =>
    buildMaxBetWeekdayHDPGroupOptions(dataSource),
  MB_WD_NO_HDP: (dataSource: TDataSource[]) =>
    buildMaxBetWeekdayNoHDPGroupOptions(dataSource),
  MB_WK_HDP: (dataSource: TDataSource[]) =>
    buildMaxBetWeekendHDPGroupOptions(dataSource),
  MB_WK_NO_HDP: (dataSource: TDataSource[]) =>
    buildMaxBetWeekendNoHDPGroupOptions(dataSource),
  LST_Note: (dataSource: TDataSource[]) => buildLSTNoteOptions(dataSource),
  Remark2: (dataSource: TDataSource[]) => buildRemark2Options(dataSource),
  Comment: (dataSource: TDataSource[]) => buildCommentOptions(dataSource)
};

export default helper;
