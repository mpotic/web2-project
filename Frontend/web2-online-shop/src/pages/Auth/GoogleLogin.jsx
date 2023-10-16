import { GoogleLogin } from '@react-oauth/google';
import { toasterUtil } from '../../utils/toasterUtil';

function GoogleLoginApi({handleGoogleLogin}) {

  function handleLoginSuccess(response) {
    const idToken = response.credential;
    handleGoogleLogin(idToken, null);
  }
  
  function handleLoginFailure(error) {
    toasterUtil.handleError('Google login failed:', error);
    handleGoogleLogin(null, error);
  }

  return (
    <GoogleLogin
      buttonText='Login with Google'
      onSuccess={handleLoginSuccess}
      onFailure={handleLoginFailure}
    />
  );
}

export default GoogleLoginApi;
