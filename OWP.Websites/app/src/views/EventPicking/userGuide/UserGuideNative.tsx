import { Modal, ModalHeader, ModalBody } from 'reactstrap';
import { USER_GUIDE } from '../const/const.json';
import ExportOnlyImg from '../../../assets/img/user_guide_export_only.png';
import ExportOnlyImgCheckAll from '../../../assets/img/user_guide_export_check_all.png';
import TipsIcon from '../../../assets/svg-element/TipsIcon';

type TProps = {
    onShow: (prop: false) => void
}

type TList = {
    title: {
        class: string,
        content: string
    },
    items: Array<{
        class: string,
        content: string
    }>
}
const UserGuideNative = (props: TProps) => {
    const renderList = (elements: TList) => {
        return <>
            <p className={elements.title.class}><strong>{elements.title.content}</strong></p>
            {
                elements.items.map((item, index) => <ul key={index}>
                    <li className={item.class} dangerouslySetInnerHTML={{ __html: item.content }} />
                </ul>)
            }
        </>;
    };

    const renderExportSummary = () => renderList({
        title: {
            class: 'user__guide',
            content: 'To export Event Picking, follow the steps below, in two ways:'
        },
        items: [{
            class: 'user__guide',
            content: 'Export only',
        }, {
            class: 'user__guide',
            content: 'Export and Check All',
        }]
    });

    const renderExportOnly = () => renderList({
        title: {
            class: 'user__guide',
            content: 'Export only'
        },
        items: [{
            class: 'user__guide',
            content: 'Step 1.1: In the filter area, select fields that you want to export',
        }, {
            class: 'user__guide',
            content: 'Step 1.2: Click on <strong>Export only</strong>',
        },
        {
            class: 'user__guide',
            content: 'Step 1.3: Select all checkbox of matches which you checked (optional step)',
        }]
    });

    const renderExportAndCheckAll = () => renderList({
        title: {
            class: 'user__guide export__and--check-all',
            content: 'Export and Check All'
        },
        items: [{
            class: 'user__guide',
            content: 'Step 2.1: In the filter area, select fields that you want to export',
        }, {
            class: 'user__guide',
            content: 'Step 2.2: Click on <strong>Export and Check All</strong>',
        }]
    });

    const renderImage = (src: Required<string>) => <div className='user__guide__img'>
        <img width={600} height={221} className='image' src={src} alt="export" />
    </div>;

    const renderTips = () => <div className='user__guide--tips'>
        <div className='wrapper'>
            <span>
                <TipsIcon />
            </span>
            <span className='tips'>Export and Check All will replace step 1.2 and 1.3. Checkboxes shall be automatically checked</span>
        </div>
    </div>;

    return (
        <Modal tag='div' style={{ maxWidth: '800px', width: '100%' }} isOpen className='modal-dark-title'>
            <ModalHeader toggle={() => props.onShow(false)}>
                {USER_GUIDE}
            </ModalHeader>
            <ModalBody tag='div'>
                <p className='user__guide'>You can export the list of matches picked to the excel format. All changed and unchecked information in the file will be highlighted in pink</p>
                {renderExportSummary()}
                {renderExportOnly()}
                {renderImage(ExportOnlyImg)}
                {renderExportAndCheckAll()}
                {renderTips()}
                {renderImage(ExportOnlyImgCheckAll)}
            </ModalBody>
        </Modal>
    );
};

export default UserGuideNative;
