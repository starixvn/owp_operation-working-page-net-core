import { Component } from 'react';
import { Collapse, Button, CardHeader } from 'reactstrap';
import ChevronDownIcon from 'mdi-react/ChevronDownIcon';
import ChevronRightIcon from 'mdi-react/ChevronRightIcon';
import PropTypes from 'prop-types';
import axios from 'axios';
import LiveDetailService from './LiveDetailService';
import AssignmentService from './Service';
import MatchSettingService from '../MatchSetting/Service';
import TableAssignment from './TableAssignment';

const propTypes = {
  policy: PropTypes.object.isRequired,
  matchId: PropTypes.number.isRequired
};

class LiveAssignment extends Component {
  constructor(props) {
    super(props);
    this.toggle = this.toggle.bind(this);
    this.state = {
      isOpen: true,
      isLoading: true,
      liveAssignmentDataSource: []
    };
  }

  componentDidMount() {
    (async () => {
      const { matchId } = this.props;
      const [matchSettings, assignments] = await axios.all([
        MatchSettingService.query({ OWPMatchId: matchId }),
        AssignmentService.query({ OWPMatchId: matchId })
      ]);
      this.setState({
        isLoading: false,
        liveAssignmentDataSource: LiveDetailService.getAll(
          matchSettings,
          assignments
        )
      });
    })();
  }

  toggle() {
    this.setState({ isOpen: !this.state.isOpen });
  }

  render() {
    const { isLoading, liveAssignmentDataSource, isOpen } = this.state;
    const { policy, matchId } = this.props;
    return (
      <div className='widget-wrap'>
        <CardHeader className={isOpen ? 'card-expand' : 'card-collapse'}>
          <Button
            data-testid="toggle-testId"
            className='title'
            color='default'
            onClick={this.toggle}>
            {isOpen ? (
              <ChevronDownIcon size={16} />
            ) : (
              <ChevronRightIcon size={16} />
            )}{' '}
            Live Assignment
          </Button>
        </CardHeader>
        <Collapse isOpen={isOpen}>
          {isLoading ? (
            'Loading...'
          ) : (
            <TableAssignment
              policy={policy}
              dataSource={liveAssignmentDataSource}
              matchId={matchId}
            />
          )}
        </Collapse>
      </div>
    );
  }
}
LiveAssignment.propTypes = propTypes;
export default LiveAssignment;
