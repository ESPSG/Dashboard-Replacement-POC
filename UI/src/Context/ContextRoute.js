import React from 'react';
import { Route } from 'react-router-dom';

const ContextRoute = ({ component, ...others }) => {
  const Component = component;

  return (
    <Route {...others}>
      <Component />
    </Route>
  );
};

export default ContextRoute;