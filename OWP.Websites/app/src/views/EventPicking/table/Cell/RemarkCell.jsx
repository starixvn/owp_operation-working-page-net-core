import PropTypes from 'prop-types';
import { UncontrolledTooltip } from 'reactstrap';

const propTypes = {
    dataSource: PropTypes.array.isRequired,
    rowIndex: PropTypes.number.isRequired
};

const RemarkCell = props => {
    const { rowIndex, dataSource } = props;
    const {
        lstNote,
        remark,
        remark2,
        homeRemark,
        awayRemark,
        isHomeBadRanking,
        isAwayBadRanking,
        isAwayDowngrade,
        isHomeDowngrade,
        infoSiteBlackList
    } = dataSource[rowIndex];

    const renderLstNote = () =>
        lstNote && (
            <span>
                <strong>LST Note: </strong>
                {lstNote}
            </span>
        );

    const renderRemark = () =>
        remark && (
            <span>
                <strong>Remark: </strong>
                {remark}
            </span>
        );

    const renderRemark2 = () =>
        remark2 && (
            <span>
                <strong>Information: </strong>
                {remark2}
            </span>
        );

    const renderTeamRemark = (teamRemark, isBadRanking, isDowngrade, label) => {
        if (teamRemark.toLowerCase().includes('bad ranking strike out')) {
            return <span style={{ display: 'flex' }}>
                <strong>{label}: </strong>
                <span>
                    <span className='team-bad-ranking'>{teamRemark}</span>
                    {isDowngrade && <span className='team-downgrade'>Regelation</span>}
                </span>
            </span>;
        }

        if (isBadRanking || isDowngrade) {
            return <span style={{ display: 'flex' }}>
                <strong>{label}: </strong>
                <span>
                    {teamRemark}
                    {isBadRanking && <span className='team-bad-ranking'>Bad Ranking</span>}
                    {isDowngrade && <span className='team-downgrade'>Regelation</span>}
                </span>
            </span>;
        }

        return <>
            {
                teamRemark !== '' &&
                <span style={{ display: 'flex' }}>
                    <strong>{label}: </strong>
                    <span>{teamRemark}</span>
                </span>
            }
        </>;
    };

    const renderAwayRemark = () => renderTeamRemark(awayRemark, isAwayBadRanking, isAwayDowngrade, 'Away Remark');
    const renderHomeRemark = () => renderTeamRemark(homeRemark, isHomeBadRanking, isHomeDowngrade, 'Home Remark');

    const renderBlackListInfoSite = () =>
        infoSiteBlackList.length > 0 && (
            <>
                {infoSiteBlackList.map((blackList, i) => {
                    return (
                        <span key={i} className='black-list'>
                            {blackList}
                        </span>
                    );
                })}
            </>
        );

    return (
        <>
            <div className='remark-cell' id={`remark${rowIndex}`}>
                {renderLstNote()}
                {renderRemark()}
                {renderRemark2()}
                {renderHomeRemark()}
                {renderAwayRemark()}
                {renderBlackListInfoSite()}
            </div>
            <UncontrolledTooltip
                className='tooltip-remark'
                target={`remark${rowIndex}`}>
                {renderLstNote()}
                {renderRemark()}
                {renderRemark2()}
                {renderHomeRemark()}
                {renderAwayRemark()}
            </UncontrolledTooltip>
        </>
    );
};

RemarkCell.propTypes = propTypes;
export default RemarkCell;
