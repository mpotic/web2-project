import { UserContextProvider } from './UserContext';

const ContextProvider = ({ children }) => {
  return <UserContextProvider>{children}</UserContextProvider>;
};

export default ContextProvider;
