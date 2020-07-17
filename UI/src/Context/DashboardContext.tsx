import * as React from "react";
import { MetricNode } from "Utilities/StudentModelTypes";
import { setArrayTypes } from "Utilities/HelperFunctions";

export interface DashboardState {
  shownColumnKeys?: string[],
  parentColumnKeys?: string[],
  metrics?: MetricNode[],
  baseUrl: string
}

export enum DashboardActionTypes {
  SetColumnKeys = "setColumnKeys",
  SetParentKeys = "setParentKeys",
  SetMetrics = "setMetrics"
}

export type DashboardAction =
  | { type: DashboardActionTypes.SetMetrics; payload: MetricNode[] }
  | { type: DashboardActionTypes.SetParentKeys; payload: string[] }
  | { type: DashboardActionTypes.SetColumnKeys; payload: string[] };
type DashboardDispatch = (action: DashboardAction) => void;

const DashboardStateContext = React.createContext<DashboardState | undefined>(undefined);
const DashboardDispatchContext = React.createContext<DashboardDispatch | undefined>(undefined);

function dashboardReducer(state: DashboardState, action: DashboardAction): DashboardState {
  switch (action.type) {
    case DashboardActionTypes.SetColumnKeys: {
      localStorage.setItem("shownColumnKeys", JSON.stringify(action.payload));
      return { ...state, shownColumnKeys: action.payload };
    }
    case DashboardActionTypes.SetParentKeys: {
      localStorage.setItem("parentColumnKeys", JSON.stringify(action.payload));
      return { ...state, parentColumnKeys: action.payload };
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

export const DashboardProvider: React.FC<{baseUrl: string}> = ({ baseUrl, children }) => {
  const shownColumnKeys: string[] = JSON.parse(localStorage.getItem("shownColumnKeys") || "[]");
  const parentColumnKeys: string[] = JSON.parse(localStorage.getItem("parentColumnKeys") || "[]");
  const metrics: [] = JSON.parse(localStorage.getItem("metrics") || "[]");
  const typedMetrics: MetricNode[] = setArrayTypes<MetricNode>(MetricNode, metrics);
  const [state, dispatch] = React.useReducer(dashboardReducer, { shownColumnKeys: shownColumnKeys, parentColumnKeys: parentColumnKeys, metrics: typedMetrics, baseUrl: baseUrl });

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
    throw new Error("useDashboardState must be used within a DashboardProvider");
  }
  return dashboardStateContext;
};

export const useDashboardDispatch = () => {
  const dashboardDispatchContext = React.useContext(DashboardDispatchContext);
  if (dashboardDispatchContext === undefined) {
    throw new Error("useDashboardDispatch must be used within a DashboardProvider");
  }
  return dashboardDispatchContext;
};

export const useDashboardContext = () => {
  const state = useDashboardState();
  const dispatch = useDashboardDispatch();

  return [state, dispatch];
}