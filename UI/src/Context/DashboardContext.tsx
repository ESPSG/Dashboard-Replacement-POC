import * as React from "react";
import { setArrayTypes } from "Utilities/HelperFunctions";
import { MetricNode } from "Utilities/StudentModelTypes";

export interface DashboardState {
  shownColumnKeys?: string[];
  parentColumnKeys?: string[];
  metrics?: MetricNode[];
  baseUrl: string;
  authToken?: string;
  username?: string;
  userEmail?: string;
  authType?: string;
}

export enum DashboardActionTypes {
  SetColumnKeys = "setColumnKeys",
  SetParentKeys = "setParentKeys",
  SetMetrics = "setMetrics",
  SetConfig = "setConfig",
  SetUserName = "setUserName",
  SetToken = "setToken",
}

export type DashboardAction =
  | { type: DashboardActionTypes.SetMetrics; payload: MetricNode[] }
  | { type: DashboardActionTypes.SetParentKeys; payload: string[] }
  | { type: DashboardActionTypes.SetColumnKeys; payload: string[] }
  | { type: DashboardActionTypes.SetUserName; payload: string }
  | { type: DashboardActionTypes.SetToken; payload: string }
  | { type: DashboardActionTypes.SetConfig; payload: any };
type DashboardDispatch = (action: DashboardAction) => void;

const DashboardStateContext = React.createContext<DashboardState | undefined>(
  undefined
);
const DashboardDispatchContext = React.createContext<
  DashboardDispatch | undefined
>(undefined);

function dashboardReducer(
  state: DashboardState,
  action: DashboardAction
): DashboardState {
  switch (action.type) {
    case DashboardActionTypes.SetColumnKeys: {
      localStorage.setItem("shownColumnKeys", JSON.stringify(action.payload));
      return { ...state, shownColumnKeys: action.payload };
    }
    case DashboardActionTypes.SetParentKeys: {
      localStorage.setItem("parentColumnKeys", JSON.stringify(action.payload));
      return { ...state, parentColumnKeys: action.payload };
    }
    case DashboardActionTypes.SetConfig: {
      return { ...state, ...action.payload };
    }
    case DashboardActionTypes.SetUserName: {
      return { ...state, username: action.payload };
    }
    case DashboardActionTypes.SetToken: {
      return { ...state, authToken: action.payload };
    }
    case DashboardActionTypes.SetMetrics: {
      localStorage.setItem("metrics", JSON.stringify(action.payload));
      return { ...state, metrics: action.payload };
    }
    default: {
      throw new Error("Invalid action type");
    }
  }
}

export const DashboardProvider: React.FC<{ baseUrl: string }> = ({
  baseUrl,
  children,
}) => {
  const shownColumnKeys: string[] = JSON.parse(
    localStorage.getItem("shownColumnKeys") || "[]"
  );
  const parentColumnKeys: string[] = JSON.parse(
    localStorage.getItem("parentColumnKeys") || "[]"
  );
  const metrics: [] = JSON.parse(localStorage.getItem("metrics") || "[]");
  const typedMetrics: MetricNode[] = setArrayTypes<MetricNode>(
    MetricNode,
    metrics
  );
  const [state, dispatch] = React.useReducer(dashboardReducer, {
    shownColumnKeys: shownColumnKeys,
    parentColumnKeys: parentColumnKeys,
    metrics: typedMetrics,
    baseUrl: baseUrl,
  });

  React.useEffect(() => {
    var configObj =
      process.env.NODE_ENV === "development"
        ? {
            baseUrl: process.env.REACT_APP_BASE_URL,
            authType: process.env.REACT_APP_AUTH_TYPE,
          }
        : {
            baseUrl: window.BASE_URL,
            authType: window.AUTH_TYPE,
          };

    dispatch({ type: DashboardActionTypes.SetConfig, payload: configObj });
  }, []);
  return (
    <DashboardStateContext.Provider value={state}>
      <DashboardDispatchContext.Provider value={dispatch}>
        {children}
      </DashboardDispatchContext.Provider>
    </DashboardStateContext.Provider>
  );
};

export const useDashboardState = () => {
  const dashboardStateContext = React.useContext(DashboardStateContext);
  if (dashboardStateContext === undefined) {
    throw new Error(
      "useDashboardState must be used within a DashboardProvider"
    );
  }
  return dashboardStateContext;
};

export const useDashboardDispatch = () => {
  const dashboardDispatchContext = React.useContext(DashboardDispatchContext);
  if (dashboardDispatchContext === undefined) {
    throw new Error(
      "useDashboardDispatch must be used within a DashboardProvider"
    );
  }
  return dashboardDispatchContext;
};

export const useDashboardContext = () => {
  const state = useDashboardState();
  const dispatch = useDashboardDispatch();

  return [state, dispatch];
};
