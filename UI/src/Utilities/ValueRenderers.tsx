import { Metric, MetricState, MetricTrendDirection } from "./StudentModelTypes";
import React from 'react';
import {CaretUpOutlined, CaretDownOutlined, PauseOutlined} from "@ant-design/icons";
import { Tooltip } from "antd";

export const renderMetric = (value: Metric) => {
  const color = metricColorClass(value.state);
  const improvementIcon = getImprovementIcon(value.trendDirection);
  var textValue:any = value.value; 
  return <Tooltip overlayClassName="auto-width" destroyTooltipOnHide={{keepParent: false}} title={getTooltipText(value)}>
      <div className={color + " d-inline-block" + (improvementIcon === null ? " no-icon" : "")}>{textValue}{improvementIcon}</div>
    </Tooltip>
}

export const getTooltipText = (value: Metric) => {
  return <div className="flex-column text-nowrap">
    <div>{value.state}</div>
    {value.trendDirection !== undefined && value.trendDirection !== null ? MetricTrendDirection[value.trendDirection] : ""}
    </div>;
}

export const metricColorClass = (goal?: MetricState | null) => {
  if(goal === MetricState.Bad) {
    return "color-danger";
  } else if (goal === MetricState.Good) {
    return "color-success";
  } 
  return "";
}

export const getImprovementIcon = (improvement?: MetricTrendDirection | null) => {
  const iconClass = "ml-1 font-size-normal";

  if(improvement === MetricTrendDirection.Better) {
    return <CaretUpOutlined className={iconClass + " color-success"}/>
  } else if(improvement === MetricTrendDirection.Worse) {
    return <CaretDownOutlined className={iconClass + " color-danger"}/>
  } else if(improvement === MetricTrendDirection.Same) {
    return <PauseOutlined className={iconClass + " rotate-90 color-primary"}/>
  }
  return null;
}