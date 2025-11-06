import { createAsyncThunk } from '@reduxjs/toolkit';
import { TErrorMessage, TTicketInfo, TValidationResult, TValidateSingleTicketResult } from '../../type';

const validateGhostName = (ticket: TTicketInfo, errorMessage: TValidateSingleTicketResult) => {
  if (!ticket.ghostName) {
    errorMessage.valid = false;
    errorMessage.message.ghostName = 'Ghost Name is required';
  }
};

const validateGhostDetail = (ticket: TTicketInfo, errorMessage: TValidateSingleTicketResult) => {
  if (!ticket.ghostDetail) {
    errorMessage.valid = false;
    errorMessage.message.ghostDetail = 'Ghost Detail is required';
  }
};
const validateBettype = (ticket: TTicketInfo, errorMessage: TValidateSingleTicketResult) => {
  if (!ticket.betType) {
    errorMessage.valid = false;
    errorMessage.message.bettype = 'Bet Type is required';
  }
};

const validateBetOn = (ticket: TTicketInfo, errorMessage: TValidateSingleTicketResult) => {
  if (!ticket.betOn) {
    errorMessage.valid = false;
    errorMessage.message.betOn = 'Bet On is required';
  }
};

const validateHdpOu = (ticket: TTicketInfo, errorMessage: TValidateSingleTicketResult) => {
  if (!ticket.hdpOu) {
    errorMessage.valid = false;
    errorMessage.message.hdp_ou = 'HDP/OU is required';
  }
};

const validateOdds = (ticket: TTicketInfo, errorMessage: TValidateSingleTicketResult) => {
  if (!ticket.odds) {
    errorMessage.valid = false;
    errorMessage.message.odds = 'Odds is required';
  }
};

const validateNote = (ticket: TTicketInfo, errorMessage: TValidateSingleTicketResult) => {
  if (!ticket.note) {
    errorMessage.valid = false;
    errorMessage.message.note = 'Note is required';
  }
};

export const validateTicketInfo = createAsyncThunk<
  TValidationResult,
  TTicketInfo[]>(
    'ticket/validateData',
    async data => {
      const result: TValidationResult = {
        valid: true,
        message: [] as TErrorMessage[]
      };

      for (const ticket of data) {
        const errorMessage: TValidateSingleTicketResult = {
          valid: true,
          message: {
            no: ticket.no
          }
        };
        validateGhostName(ticket, errorMessage);
        validateGhostDetail(ticket, errorMessage);
        validateBettype(ticket, errorMessage);
        validateBetOn(ticket, errorMessage);
        validateHdpOu(ticket, errorMessage);
        validateOdds(ticket, errorMessage);
        validateNote(ticket, errorMessage);

        if (!errorMessage.valid) {
          result.valid = false;
          result.message.push(errorMessage.message);
        }
      }
      return result;
    }
  );