import {useEffect, useCallback, useState} from 'react';
import axios from 'axios';
import { useDashboardState } from 'Context/DashboardContext';

export const useApiWorker = () => {
  const {baseUrl} = useDashboardState();

  const apiWorker = axios.create({
    baseURL: baseUrl,
    headers: {
      'Accept': '*/*',
      'Access-Control-Allow-Origin': '*'
    }
  });

  return apiWorker;
}

export const useAxios = (url="", isPost=false, initialData=null, requestData, transformResponse) => {
  const API = useApiWorker();
  const [error, setError] = useState(null);
  const [isLoading, setIsLoading] = useState(false);
  const [data, setData] = useState(initialData);
  const CancelToken = axios.CancelToken;
  var cancelRequest = () => {};

  const makeRequest = useCallback(async(queryBody) => {
    try {
      setIsLoading(true);
      await API.request({
        ...{url}, 
        method: isPost ? "POST" : "GET",
        cancelToken: new CancelToken((c) => {cancelRequest = c}),
        data: queryBody || requestData
      })
        .catch((error) => {
          console.log(error);
          setError(error);
        })
        .then((response) => {
          if(response) {
            if(transformResponse) {
              setData(transformResponse(response));
            }
            else {
              setData(response);
            }
          }
        })
        .finally(() => {
          setIsLoading(false);
        });
    } catch (err) {
      if(axios.isCancel(err)) {
        console.log("Request cancelled");
      }
      console.log(err.message);
    }
  }, []);

  return [makeRequest, data, isLoading, {error, cancelRequest}]
}

export const useGraphQLQuery = (queryString, initialData=null, transformResponse, condition=true) => {
  const [sendQuery, data, isLoading, {error, cancelRequest}] = useAxios("", true, initialData, {query: queryString}, transformResponse);

  useEffect(() => {
    if(condition && queryString && queryString !== null && queryString !== "") {
      sendQuery({query: queryString});
      return cancelRequest;
    }
  }, [queryString]);

  return [data, isLoading, {error, cancelRequest, sendQuery}];
}

export const useDataFetcher = (url, initialData=null) => {
  const [getData, data, isLoading, {error, cancelRequest}] = useAxios(url, false, initialData);

  useEffect(() => {
    getData();
    return cancelRequest;
  }, [url]);

  return [data, isLoading, {error, cancelRequest}];
}