import React, { useEffect, useState } from 'react';
import { Fragment } from 'react';
import axios from 'axios';

const defaultConfig = {
  baseUrl: "https://localhost:5001/graphql"
}

function ConfigLoader (props) {
  const {ready, configFile} = props;

  const [isLoaded, setIsLoaded] = useState(false);
  const [config, setConfig] = useState(defaultConfig);

  useEffect(() => {
    const getConfig = async () => {
      const res = await axios.get(configFile, {baseURL: process.env.PUBLIC_URL}).catch((error) => {console.log(error);});
      const {data} = res || {};
      setConfig({...defaultConfig, ...data});
      setIsLoaded(true);
    }
    getConfig();
  }, []);

  return (
    <Fragment>
      {isLoaded && ready(config)}
    </Fragment>
  );
}

ConfigLoader.defaultProps = {
  ready: () => {},
  configFile: "apiConfig.json"
}

export default ConfigLoader;