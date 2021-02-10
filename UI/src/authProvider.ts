import { CacheLocation } from "msal";
import { MsalAuthProvider, LoginType } from "react-aad-msal";

const development = process.env.NODE_ENV === "development";

// Msal Configurations
const config = {
  auth: {
    authority: development
      ? `${process.env.REACT_APP_INSTANCE}${process.env.REACT_APP_TENANT}`
      : `${window.INSTANCE}${window.TENANT}`,
    clientId: development
      ? `${process.env.REACT_APP_CLIENT_ID}`
      : `${window.CLIENT_ID}`,
  },
  cache: {
    cacheLocation: "localStorage" as CacheLocation,
    storeAuthStateInCookie: true,
  },
};

// Authentication Parameters
const authenticationParameters = {
  scopes: [
    (development
      ? `${process.env.REACT_APP_CLIENT_ID}`
      : `${window.CLIENT_ID}`) + `/.default`,
  ],
};

// Options
const options = {
  loginType: LoginType.Redirect,
  tokenRefreshUri: window.location.origin,
};

export const authProvider = new MsalAuthProvider(
  config,
  authenticationParameters,
  options
);
