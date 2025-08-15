import { Modal, DatePicker } from 'antd';
import { useState, useMemo, useEffect } from 'react';
import dayjs from 'dayjs';
import { exportExcel } from '../../../Shared/common/ExportExcel/Helper';
import Utils from '../../../Shared/Utils';
import ToastrWrapper from '../../../components/ToastrWrapper';
import { Option } from '../../../Shared/MultipleSelection/type';

const { RangePicker } = DatePicker;

interface ModalExportExcelProps {
  isModalOpen: boolean;
  setIsModalOpen: (isOpen: boolean) => void;
  onlyShowIsMismatchEvent: boolean;
  confirmProvider: Option[];
  lstBookProvider: Option[];
  confirmProviderOption: Option[];
  LSTBookProviderOption: Option[];
  eventDate: Date;
}

const ModalExportExcel = ({
  isModalOpen,
  setIsModalOpen,
  onlyShowIsMismatchEvent,
  confirmProvider,
  lstBookProvider,
  confirmProviderOption,
  LSTBookProviderOption,
  eventDate
}: ModalExportExcelProps) => {
  const defaultDateRange = useMemo(() => {
    const eventDayjs = dayjs(eventDate);
    return [eventDayjs, eventDayjs] as [dayjs.Dayjs, dayjs.Dayjs];
  }, [eventDate]);

  const [dateRange, setDateRange] = useState<[dayjs.Dayjs, dayjs.Dayjs] | null>(
    defaultDateRange
  );

  useEffect(() => {
    setDateRange(defaultDateRange);
  }, [defaultDateRange]);

  const getProviderValue = (selectedProviders: Option[], allProviders: Option[]) => {
    if (!selectedProviders?.length) {
      return null;
    }
    const providerValue = selectedProviders.map(x => x.value).join(',');
    return selectedProviders.length === allProviders?.length
      ? null
      : providerValue;
  };

  const handleOk = () => {
    if (!dateRange) {
      ToastrWrapper.showWarning(
        <span>Please select both start date and end date!</span>
      );
      return;
    }

    const [startDate, endDate] = dateRange;

    const daysDiff = endDate.diff(startDate, 'day');
    if (daysDiff > 31) {
      ToastrWrapper.showWarning(<span>Date range cannot exceed 31 days!</span>);
      return;
    }

    const date = Utils.formatDate(new Date());
    const confirmProviderValue = getProviderValue(
      confirmProvider,
      confirmProviderOption
    );
    const lstBookProviderValue = getProviderValue(
      lstBookProvider,
      LSTBookProviderOption
    );

    const params = new URLSearchParams({
      startDate: Utils.formatDate(startDate),
      endDate: Utils.formatDate(endDate),
      onlyShowIsMismatchEvent: onlyShowIsMismatchEvent.toString(),
      ...(lstBookProviderValue !== null && {
        lstBookProvider: lstBookProviderValue
      }),
      ...(confirmProviderValue !== null && {
        confirmProvider: confirmProviderValue
      })
    }).toString();
    const url = `api/ProviderVerification/export?${params}`;
    const fileName = `ProviderVerification - ${dayjs(date).format(
      'MMDDYYYY'
    )}.xlsx`;

    exportExcel(url, fileName);
    setIsModalOpen(false);
  };

  const handleCancel = () => {
    setIsModalOpen(false);
  };

  return (
    <Modal
      title='Please select a date range'
      open={isModalOpen}
      onOk={handleOk}
      onCancel={handleCancel}
      width={400}
      style={{ top: 130 }}>
      <RangePicker
        value={dateRange}
        onChange={dates =>
          setDateRange(dates as [dayjs.Dayjs, dayjs.Dayjs] | null)
        }
        format='MMM DD, YYYY'
      />
    </Modal>
  );
};

export default ModalExportExcel;
