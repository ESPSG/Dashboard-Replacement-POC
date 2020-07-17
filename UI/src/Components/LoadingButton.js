import React from 'react';
import { Button } from 'reactstrap';
import PulseLoader from 'react-spinners/PulseLoader';

function LoadingButton(props) {
  const {loading, ...others} = props;

  return (
    <Button disabled={loading} {...others}>
      {loading && <PulseLoader color={"#ffffff"} loading={loading} size={12} margin={8} />}
      {!loading && props.children}
    </Button>
  )
}

export default LoadingButton;