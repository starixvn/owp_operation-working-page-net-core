import { createRef, Component } from 'react';
import PropTypes from 'prop-types';
import { Table, UncontrolledTooltip } from 'reactstrap';
import EditableButtonGroupForLiveDetail from '../../components/Assignment/EditableButtonGroupForLiveDetail';
import { isNil } from 'lodash';

const propTypes = {
  dataSource: PropTypes.any.isRequired,
  policy: PropTypes.object.isRequired
};

class TableAssignment extends Component {
  constructor(props) {
    super(props);
    this.mappingRowGroup = this.mappingRowGroup.bind(this);
    this.state = {
      dataSource: []
    };
    this.tbody = createRef();
  }

  changeBackgroundTableRow(event, color) {
    const groupName = event.currentTarget.getAttribute('data-group');
    const rows = this.tbody.current.querySelectorAll(
      `tr[data-group=${groupName}]`
    );
    rows.forEach(element => {
      element.style.background = color;
    });
  }

  renderMaxbetSpread(maxbet, spread, id, className, contentTooltip) {
    if (!isNil(maxbet) || !isNil(spread)) {
      return (
        <>
          <span id={id} className={className}>
            {maxbet} {spread}
          </span>
          <UncontrolledTooltip placement='right' target={id}>
            {contentTooltip}
          </UncontrolledTooltip>
        </>
      );
    } else {
      return null;
    }
  }

  mappingRowGroup(item) {
    const { policy } = this.props;
    return item.DataSource.map((entity, key) => {
      const mbcContentTooltips = 'Maxbet / Spread of MBC ';
      return (
        <tr
          key={'assignmentRow_' + key}
          data-group={item.Group}
          onMouseEnter={event =>
            this.changeBackgroundTableRow(event, '#FFEFD5')
          }
          onMouseLeave={event => this.changeBackgroundTableRow(event, null)}>
          <td className='bettype-name'>{entity.Name}</td>
          <td className='maxbet'>
            {this.renderMaxbetSpread(
              entity.MaxBetFT.MaxBet,
              entity.MaxBetFT.Spread,
              `MBCFT-${item.Group}-${key}`,
              'value-center',
              mbcContentTooltips
            )}
          </td>
          <td className='maxbet'>
            {this.renderMaxbetSpread(
              entity.MaxBetHT.MaxBet,
              entity.MaxBetHT.Spread,
              `MBCHT-${item.Group}-${key}`,
              'value-center',
              mbcContentTooltips
            )}
          </td>
          <td>
            <EditableButtonGroupForLiveDetail
              policy={policy}
              main={entity.FT.main}
              others={entity.FT.others}
              matchId={entity.OWPMatchId}
              isAllowAdd={entity.FT.isAllowAdd}
            />{' '}
          </td>
          <td>
            <EditableButtonGroupForLiveDetail
              policy={policy}
              main={entity.HT.main}
              others={entity.HT.others}
              matchId={entity.OWPMatchId}
              isAllowAdd={entity.HT.isAllowAdd}
            />
          </td>
          {key === 0 && (
            <td rowSpan={item.TotalRows}>
              <EditableButtonGroupForLiveDetail
                policy={policy}
                main={entity.LA.main}
                others={entity.LA.others}
                matchId={entity.OWPMatchId}
                isAllowAdd={entity.LA.isAllowAdd}
              />
            </td>
          )}
        </tr>
      );
    });
  }

  render() {
    const { dataSource } = this.props;
    const displayGroup = dataSource.map(this.mappingRowGroup);
    return (
      <Table bordered>
        <thead>
          <tr>
            <th className='bettype'>Bet Type</th>
            <th colSpan='2' className='maxbet'>
              Max Bet
            </th>
            <th className='assign-col'>FT</th>
            <th className='assign-col'>1H</th>
            <th className='assign-col'>LA</th>
          </tr>
        </thead>
        <tbody className='live-assignment' ref={this.tbody}>
          {displayGroup}
        </tbody>
      </Table>
    );
  }
}

TableAssignment.propTypes = propTypes;
export default TableAssignment;
