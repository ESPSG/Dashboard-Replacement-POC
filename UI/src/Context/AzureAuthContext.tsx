import LoadingSection from "Components/LoadingSection";
import * as React from "react";
import { AuthenticationState } from "react-aad-msal";
import { authProvider } from "../authProvider";
import { DashboardActionTypes, useDashboardDispatch } from "./DashboardContext";

export interface AzureAuthState {
  baseUrl?: string;
  tenant?: string;
  clientId?: string;
  authenticationState?: AuthenticationState;
  token?: string;
  username?: string;
}

export enum AzureAuthActionTypes {
  SetConfig = "setConfig",
  SetToken = "setToken",
  SetAuthenticationState = "setAuthenticationState",
}

export type AzureAuthAction =
  | { type: AzureAuthActionTypes.SetToken; payload: string }
  | {
      type: AzureAuthActionTypes.SetAuthenticationState;
      payload: AuthenticationState;
    }
  | { type: AzureAuthActionTypes.SetConfig; payload: any };
type AzureAuthDispatch = (action: AzureAuthAction) => void;

const AzureAuthStateContext = React.createContext<AzureAuthState | undefined>(
  undefined
);
const AzureAuthDispatchContext = React.createContext<
  AzureAuthDispatch | undefined
>(undefined);

function azureAuthReducer(
  state: AzureAuthState,
  action: AzureAuthAction
): AzureAuthState {
  switch (action.type) {
    case AzureAuthActionTypes.SetToken: {
      return { ...state, token: action.payload };
    }
    case AzureAuthActionTypes.SetConfig: {
      return { ...state, ...action.payload };
    }
    case AzureAuthActionTypes.SetAuthenticationState: {
      return { ...state, authenticationState: action.payload };
    }
    default: {
      throw new Error("Invalid action type");
    }
  }
}

export const AzureAuthProvider: React.FC = ({ children }) => {
  const [state, dispatch] = React.useReducer(azureAuthReducer, {});
  const dashboardDispatch = useDashboardDispatch();

  const getToken = async () => {
    var token = await authProvider.getAccessToken();
    dispatch({
      type: AzureAuthActionTypes.SetToken,
      payload: `${token?.accessToken}`,
    });
    dashboardDispatch({
      type: DashboardActionTypes.SetToken,
      payload: `${token?.accessToken}`,
    });
  };

  React.useEffect(() => {
    var account = authProvider.getAccount();

    var configObj =
      process.env.NODE_ENV === "development"
        ? {
            tenant: process.env.REACT_APP_TENANT,
            clientId: process.env.REACT_APP_CLIENT_ID,
            baseUrl: process.env.REACT_APP_BASE_URL,
            username: account?.name,
          }
        : {
            tenant: window.TENANT,
            clientId: window.CLIENT_ID,
            baseUrl: window.BASE_URL,
            username: account?.name,
          };

    dashboardDispatch({
      type: DashboardActionTypes.SetUserName,
      payload: account?.name,
    });
    dispatch({ type: AzureAuthActionTypes.SetConfig, payload: configObj });
    getToken();
    dispatch({
      type: AzureAuthActionTypes.SetAuthenticationState,
      payload: authProvider.authenticationState,
    });
  }, []);

  return (
    <AzureAuthStateContext.Provider value={state}>
      <AzureAuthDispatchContext.Provider value={dispatch}>
        {children}
      </AzureAuthDispatchContext.Provider>
    </AzureAuthStateContext.Provider>
  );
};

export const useAzureAuthState = () => {
  const azureAuthStateContext = React.useContext(AzureAuthStateContext);
  if (azureAuthStateContext === undefined) {
    throw new Error(
      "useAzureAuthState must be used within a AzureAuthProvider"
    );
  }
  return azureAuthStateContext;
};

export const useAzureAuthDispatch = () => {
  const azureAuthDispatchContext = React.useContext(AzureAuthDispatchContext);
  if (azureAuthDispatchContext === undefined) {
    throw new Error(
      "useAzureAuthDispatch must be used within a AzureAuthProvider"
    );
  }
  return azureAuthDispatchContext;
};

export const useAzureAuthContext = () => {
  const state = useAzureAuthState();
  const dispatch = useAzureAuthDispatch();

  return [state, dispatch];
};
