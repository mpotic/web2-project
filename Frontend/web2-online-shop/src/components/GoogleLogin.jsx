import { GoogleLogin } from '@react-oauth/google';
import { toasterUtil } from '../utils/toasterUtil';
import useServices from '../services/useServices';

function handleLoginSuccess(response) {
  const idToken = response.tokenId;
}

function handleLoginFailure(error) {
  toasterUtil.handleError('Google login failed:', error);
}

function GoogleLoginApi() {
  const { google } = useServices();

  return (
    <GoogleLogin
      buttonText='Login with Google'
      onSuccess={handleLoginSuccess}
      onFailure={handleLoginFailure}
    />
  );
}

export default GoogleLoginApi;
