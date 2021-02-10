import { AzureAuthProvider } from "Context/AzureAuthContext";
import { GoogleAuthProvider } from "Context/GoogleAuthContext";
import React from "react";
import AzureAD from "react-aad-msal";
import ReactDOM from "react-dom";
import { HashRouter } from "react-router-dom";
import App from "./App";
import { authProvider } from "./authProvider";
import ScrollToTop from "./Components/ScrollToTop";
import * as serviceWorker from "./serviceWorker";
import { DashboardProvider, useDashboardState } from "Context/DashboardContext";
import LoadingSection from "Components/LoadingSection";

declare global {
  interface Window {
    GOOGLE_CLIENT_SECRET: string;
    GOOGLE_CLIENT_ID: string;
    CLIENT_ID: string;
    TENANT: string;
    INSTANCE: string;
    BASE_URL: string;
    AUTH_TYPE: string;
  }
}

const AppContent: React.FC = () => {
  const { authToken } = useDashboardState();
  return (
    <LoadingSection isLoading={authToken === undefined}>
      <HashRouter>
        <ScrollToTop />
        <App />
      </HashRouter>
    </LoadingSection>
  );
};

const AuthContent: React.FC = () => {
  const authType =
    process.env.NODE_ENV === "development"
      ? process.env.REACT_APP_AUTH_TYPE
      : window.AUTH_TYPE;
  switch (authType) {
    case "Google":
    case "google":
      return (
        <GoogleAuthProvider>
          <AppContent />
        </GoogleAuthProvider>
      );
    case "Azure":
    case "azure":
    case undefined:
    default:
      return (
        <AzureAD provider={authProvider} forceLogin>
          <AzureAuthProvider>
            <AppContent />
          </AzureAuthProvider>
        </AzureAD>
      );
  }
};

ReactDOM.render(
  <DashboardProvider
    baseUrl={
      process.env.NODE_ENV === "development"
        ? `${process.env.REACT_APP_BASE_URL}`
        : `${window.BASE_URL}`
    }
  >
    <AuthContent />
  </DashboardProvider>,
  document.getElementById("root")
);

// If you want your app to work offline and load faster, you can change
// unregister() to register() below. Note this comes with some pitfalls.
// Learn more about service workers: https://bit.ly/CRA-PWA
serviceWorker.unregister();
