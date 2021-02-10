import React from "react";
import NumberFormat from "react-number-format";
import faker from "faker";
import { StudentMetrics, MetricNode } from "./StudentModelTypes";
import { ROOT_METRIC_ID } from "./Constants";
import { renderMetric } from "./ValueRenderers";

export const toggledClass = (name: string, condition: boolean) => {
  return condition ? " " + name : "";
};

export const renderPercent = (value: string) => {
  return (
    <NumberFormat
      value={parseFloat(value) * 100}
      displayType={"text"}
      decimalScale={1}
      suffix="%"
      fixedDecimalScale
    />
  );
};

export const randomEnumElement = (obj: any) => {
  return faker.random.arrayElement(Object.keys(obj).map((key) => obj[key]));
};

export const formatRoute = (name: string, parameter: string) => {
  if (name[0] !== "/") {
    name = `/${name}`;
  }
  var parameterString = parameter ? `/${parameter}` : "";
  return `${name}${parameterString}`;
};

export function setArrayTypes<T>(type: new () => T, array: any[]): T[] {
  var typedArray: T[] = [];
  typedArray = array.map((obj: any) => {
    var typedObj = new type();
    Object.assign(typedObj, obj);
    return typedObj;
  });
  return typedArray;
}

function clone<T extends object>(type: new () => T, obj: T) {
  if (null == obj || "object" != typeof obj) return obj;
  var copy = new type();
  for (var attr in obj) {
    if (obj.hasOwnProperty(attr)) copy[attr] = obj[attr];
  }
  return copy;
}

const arrayToObject = (array: Array<any>) =>
  array.reduce((obj, item) => {
    obj[item.id.toString()] = item;
    return obj;
  }, {});

export const setStudentMetricsObjects = (students: StudentMetrics[]) => {
  var studentObjects = students.map((s) => {
    var { metrics, ...studentData } = s;
    var metricObj = arrayToObject(metrics || []);
    s = { ...studentData, ...metricObj };
    return s;
  });

  return studentObjects;
};

export const genericSorter = (dataIndex: string | string[]) => {
  if (Array.isArray(dataIndex)) {
    return (a: any, b: any) => {
      let aVal = dataIndex.reduce(
        (obj: any, prop: string) => obj && obj[prop],
        a
      );
      let bVal = dataIndex.reduce(
        (obj: any, prop: string) => obj && obj[prop],
        b
      );
      return aVal - bVal;
    };
  }
  return (a: any, b: any) => {
    if (!a[dataIndex]) {
      return -1;
    } else if (!b[dataIndex]) {
      return 1;
    }

    if (
      a[dataIndex]?.hasOwnProperty("value") &&
      b[dataIndex]?.hasOwnProperty("value")
    ) {
      if (
        parseFloat(a[dataIndex].value) !== NaN &&
        parseFloat(b[dataIndex].value) !== NaN
      ) {
        return parseFloat(a[dataIndex].value) - parseFloat(b[dataIndex].value);
      }
      return a[dataIndex].value?.localeCompare(b[dataIndex].value);
    } else if (
      typeof a[dataIndex] === "string" &&
      typeof b[dataIndex] === "string"
    ) {
      return a[dataIndex].localeCompare(b[dataIndex]);
    }

    return 0;
  };
};

export const sortGeneric = (a: any, b: any, dataIndex: string | string[]) => {
  var sorter = genericSorter(dataIndex);
  return sorter(a, b);
};

export const removeDuplicateIds = (metrics: MetricNode[]) => {
  return metrics.filter(
    (m, i, self) => i === self.findIndex((mc) => mc.id === m.id)
  );
};

export const metricsToGridConfig = (metrics: MetricNode[]) => {
  var map: any = {},
    node: MetricNode,
    roots = [] as MetricNode[],
    i = 0;

  for (i = 0; i < metrics.length; i += 1) {
    if (metrics[i].id !== undefined) {
      map[metrics[i].id || 0] = i; // initialize the map
      metrics[i].children = []; // initialize the children
    }
  }
  for (i = 0; i < metrics.length; i += 1) {
    node = metrics[i];
    if (node.parentId !== ROOT_METRIC_ID && node.parentId !== undefined) {
      // if you have dangling branches check that map[node.parentId] exists
      metrics[map[node.parentId || 0]].children.push(node);
    } else {
      roots.push(node);
    }
  }

  var config = roots.map((x) => {
    x.className = "group-header";
    x.children = ([] as MetricNode[]).concat(
      ...x.children.map((c) => {
        c.className = "group-header";
        c.children = ([] as MetricNode[]).concat(
          ...c.children.map((sc) => {
            sc.className = "group-header";
            sc.children = sc.children.map((dataMetric) => {
              dataMetric.sorter = (a: any, b: any) =>
                dataMetric.sortDataIndex(a, b);
              dataMetric.align = dataMetric.value?.includes("%")
                ? "right"
                : "center";
              dataMetric.render = (metric: MetricNode) => {
                if (metric) return renderMetric(metric);
                return "";
              };
              return dataMetric;
            });
            return sc;
          })
        );
        return c.children;
      })
    );
    return x;
  });

  return config;
};

export const filterTree = (
  tree: readonly MetricNode[],
  filter: (m: MetricNode) => boolean
): MetricNode[] => {
  var filteredTree = tree
    .map((m) => {
      var newNode = clone(MetricNode, m);
      return newNode;
    })
    .filter((node) => {
      if (node.children && node.children.length > 0) {
        node.children = filterTree(node.children, filter);
      }
      return filter(node);
    });

  return filteredTree;
};

export function updatedArray<T>(array: any[]) {
  return ([] as T[]).concat(array);
}

export const refreshTokenSetup = (res: any) => {
  // Timing to renew access token
  let refreshTiming = (res.tokenObj.expires_in || 3600 - 5 * 60) * 1000;

  const refreshToken = async () => {
    const newAuthRes = await res.reloadAuthResponse();
    refreshTiming = (newAuthRes.expires_in || 3600 - 5 * 60) * 1000;
    console.log("newAuthRes:", newAuthRes);
    // saveUserToken(newAuthRes.access_token);  <-- save new token
    localStorage.setItem("authToken", newAuthRes.id_token);

    // Setup the other timer after the first one
    setTimeout(refreshToken, refreshTiming);
  };

  // Setup first refresh timer
  setTimeout(refreshToken, refreshTiming);
};
