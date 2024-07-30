
const buildUrlRedirectToLogin = () => {
  if (!process.env.NODE_ENV || process.env.NODE_ENV === 'development') {
    return ``;
  }
  return '/Auth';
};

export { buildUrlRedirectToLogin };
