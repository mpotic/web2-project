import { OrderContextProvider } from './OrderContext';
import { UserContextProvider } from './UserContext';

const ContextProvider = ({ children }) => {
  return (
    <UserContextProvider>
      <OrderContextProvider>{children}</OrderContextProvider>
    </UserContextProvider>
  );
};

export default ContextProvider;
