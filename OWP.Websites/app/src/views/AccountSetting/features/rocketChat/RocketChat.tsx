import axios from "axios";
import { useEffect, useState } from "react";
import ToastrWrapper from "../../../components/ToastrWrapper";

const RocketChat = () => {
  const [accountAlreadyExists, setAccountAlreadyExists] = useState<boolean | null>(null);
  useEffect(() => {
    const checkAccount = async () => {
      const response = await axios.get('api/owp/chat/check-account-exists');

      setAccountAlreadyExists(response.data);
    };
    checkAccount();
    // Placeholder for any side effects or initializations needed for RocketChat
  }, []);

  const handleCreateAccount = async () => {
    await axios.post('api/owp/chat/create-account-rc');
    ToastrWrapper.showNotifySuccess('RocketChat account created successfully');
  };

  if (accountAlreadyExists === null || accountAlreadyExists === true) {
    return null;
  }

  return (
    <div className='rocket-chat'>
      <h4 className='title'>RC account</h4>
      <div className='right'>
        <button className='button-submit' onClick={handleCreateAccount} >
          Create
        </button>
      </div>
    </div>
  );
};

export default RocketChat;