import * as React from "react";
import { useGoogleLogin } from "react-google-login";
import { refreshTokenSetup } from "Utilities/HelperFunctions";
import { DashboardActionTypes, useDashboardDispatch } from "./DashboardContext";

export interface GoogleAuthState {
  baseUrl?: string;
  clientId?: string;
  token?: string;
  isSignedIn?: boolean;
  username?: string;
}

export enum GoogleAuthActionTypes {
  SetConfig = "setConfig",
  SetToken = "setToken",
}

export type GoogleAuthAction =
  | { type: GoogleAuthActionTypes.SetToken; payload: string }
  | { type: GoogleAuthActionTypes.SetConfig; payload: any };
type GoogleAuthDispatch = (action: GoogleAuthAction) => void;

const GoogleAuthStateContext = React.createContext<GoogleAuthState | undefined>(
  undefined
);
const GoogleAuthDispatchContext = React.createContext<
  GoogleAuthDispatch | undefined
>(undefined);

function googleAuthReducer(
  state: GoogleAuthState,
  action: GoogleAuthAction
): GoogleAuthState {
  switch (action.type) {
    case GoogleAuthActionTypes.SetToken: {
      return { ...state, token: action.payload };
    }
    case GoogleAuthActionTypes.SetConfig: {
      return { ...state, ...action.payload };
    }
    default: {
      throw new Error("Invalid action type");
    }
  }
}

export const GoogleAuthProvider: React.FC = ({ children }) => {
  const [state, dispatch] = React.useReducer(googleAuthReducer, {});
  const [loggedIn, setLoggedIn] = React.useState<boolean>(false);
  const [initialCall, setInitial] = React.useState<boolean>(true);
  const dashboardDispatch = useDashboardDispatch();
  const clientId =
    process.env.NODE_ENV === "development"
      ? process.env.REACT_APP_GOOGLE_CLIENT_ID || ""
      : window.GOOGLE_CLIENT_ID;

  const onSuccess = (res: any) => {
    setLoggedIn(true);
    var configObj =
      process.env.NODE_ENV === "development"
        ? {
            clientId: process.env.REACT_APP_GOOGLE_CLIENT_ID,
            baseUrl: process.env.REACT_APP_BASE_URL,
            isSignedIn: true,
            username: res.profileObj.name,
          }
        : {
            clientId: window.GOOGLE_CLIENT_ID,
            baseUrl: window.BASE_URL,
            username: res.profileObj.name,
          };
    dashboardDispatch({
      type: DashboardActionTypes.SetUserName,
      payload: res.profileObj.name,
    });

    dispatch({ type: GoogleAuthActionTypes.SetConfig, payload: configObj });
    dispatch({
      type: GoogleAuthActionTypes.SetToken,
      payload: res.tokenId,
    });
    dashboardDispatch({
      type: DashboardActionTypes.SetToken,
      payload: res.tokenId,
    });

    refreshTokenSetup(res);
  };

  const onFailure = (res: any) => {
    signIn();
  };

  const { signIn, loaded } = useGoogleLogin({
    onSuccess,
    onFailure,
    clientId: clientId,
    isSignedIn: true,
    //uxMode: "redirect",
    //responseType: "code",
    // prompt: 'consent',
  });

  React.useEffect(() => {
    if (loaded) {
      setInitial(false);
    }
    if (!loggedIn && !initialCall) {
      signIn();
    }
  }, [loaded, loggedIn, initialCall]);

  return (
    <GoogleAuthStateContext.Provider value={state}>
      <GoogleAuthDispatchContext.Provider value={dispatch}>
        {children}
      </GoogleAuthDispatchContext.Provider>
    </GoogleAuthStateContext.Provider>
  );
};

export const useGoogleAuthState = () => {
  const googleAuthStateContext = React.useContext(GoogleAuthStateContext);
  if (googleAuthStateContext === undefined) {
    throw new Error(
      "useGoogleAuthState must be used within a GoogleAuthProvider"
    );
  }
  return googleAuthStateContext;
};

export const useGoogleAuthDispatch = () => {
  const googleAuthDispatchContext = React.useContext(GoogleAuthDispatchContext);
  if (googleAuthDispatchContext === undefined) {
    throw new Error(
      "useGoogleAuthDispatch must be used within a GoogleAuthProvider"
    );
  }
  return googleAuthDispatchContext;
};

export const useGoogleAuthContext = () => {
  const state = useGoogleAuthState();
  const dispatch = useGoogleAuthDispatch();

  return [state, dispatch];
};
