import './LoadingSection.scss';
import React from 'react';
import { CircularProgress } from '@material-ui/core';
import { toggledClass } from 'Utilities/HelperFunctions';

type LoadingSectionProps = {
  isLoading: boolean,
  mask?: boolean,
  [x: string]: any
}

const LoadingSection: React.FC<LoadingSectionProps> = ({isLoading, mask, children}) => {
  return (
    mask ?
      <div className={"full-size animate-fade-in"}>
        {
          isLoading && <div className="loading-mask"><LoadingSpinner /></div>
        }
        {children}
      </div>
    :
      <div className="animate-slide-down full-size position-relative">
        {
          isLoading &&
          <LoadingSpinner />
        }
        {
          !isLoading &&
          children
        }
      </div>
  )
}

const LoadingSpinner: React.FC = () => {
  return (
    <div className="full-width flex-center loading-wrapper animate-fade-in mt-3">
      <div className="flex-center flex-normal full-width">
        <CircularProgress color="primary" className="mr-3"/>
        <span>Loading...</span>
      </div>
    </div>
  )
}

export default LoadingSection;